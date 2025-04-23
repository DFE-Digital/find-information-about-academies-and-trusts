using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;

/// <summary>
/// Adapted from https://jason-ge.medium.com/mock-async-data-repository-in-asp-net-core-3-1-634cb19a3013
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class MockDbSet<TEntity> where TEntity : class
{
    private readonly List<TEntity> _items = [];
    public DbSet<TEntity> Object { get; } = Substitute.For<DbSet<TEntity>, IAsyncEnumerable<TEntity>, IQueryable>();

    public MockDbSet()
    {
        var itemsAsQueryable = _items.AsQueryable();

        ((IAsyncEnumerable<TEntity>)Object).GetAsyncEnumerator()
            .Returns(new TestAsyncEnumerator<TEntity>(itemsAsQueryable.GetEnumerator()));
        ((IQueryable)Object).Provider.Returns(new TestAsyncQueryProvider<TEntity>(itemsAsQueryable.Provider));
        ((IQueryable)Object).Expression.Returns(itemsAsQueryable.Expression);
        ((IQueryable)Object).ElementType.Returns(itemsAsQueryable.ElementType);
        ((IQueryable)Object).GetEnumerator().Returns(itemsAsQueryable.GetEnumerator());
    }

    private class TestAsyncEnumerator<T>(IEnumerator<T> enumerator) : IAsyncEnumerator<T>
    {
        public T Current => enumerator.Current;

        public ValueTask DisposeAsync()
        {
            return new ValueTask(Task.Run(enumerator.Dispose));
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(enumerator.MoveNext());
        }
    }

    private class TestAsyncEnumerable<T>(Expression expression)
        : EnumerableQuery<T>(expression), IAsyncEnumerable<T>, IQueryable<T>
    {
        IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        {
            return new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
        }
    }

    private class TestAsyncQueryProvider<T> : IAsyncQueryProvider
    {
        private readonly IQueryProvider _innerQueryProvider;

        internal TestAsyncQueryProvider(IQueryProvider innerQueryProvider)
        {
            _innerQueryProvider = innerQueryProvider;
        }

        public IQueryable CreateQuery(Expression expression)
        {
            return new TestAsyncEnumerable<T>(expression);
        }

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        {
            return new TestAsyncEnumerable<TElement>(expression);
        }

        public object? Execute(Expression expression)
        {
            return _innerQueryProvider.Execute(expression);
        }

        public TResult Execute<TResult>(Expression expression)
        {
            return _innerQueryProvider.Execute<TResult>(expression);
        }

        public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = new())
        {
            var expectedResultType = typeof(TResult).GetGenericArguments()[0];
            var executionResult = Execute(expression);

            return (TResult)(typeof(Task).GetMethod(nameof(Task.FromResult))!
                .MakeGenericMethod(expectedResultType)
                .Invoke(null, [executionResult]) ?? throw new Exception());
        }
    }

    public int Count => _items.Count;

    public void Add(TEntity item)
    {
        _items.Add(item);
    }

    public void AddRange(IEnumerable<TEntity> items)
    {
        _items.AddRange(items);
    }

    public int GetNextId(Func<TEntity, int> identifier, int? specifiedId = null)
    {
        var nextId = specifiedId ?? _items.Count + 1;

        //Don't allow duplicate IDs
        if (_items.Any(entity => identifier(entity) == nextId))
            _items.Remove(_items.Single(entity => identifier(entity) == nextId));

        return nextId;
    }

    public string GetNextId(Func<TEntity, string> identifier, string? specifiedId = null)
    {
        var nextId = specifiedId ?? (_items.Count + 1).ToString();

        //Don't allow duplicate IDs
        if (_items.Any(entity => identifier(entity) == nextId))
            _items.Remove(_items.Single(entity => identifier(entity) == nextId));

        return nextId;
    }
}
