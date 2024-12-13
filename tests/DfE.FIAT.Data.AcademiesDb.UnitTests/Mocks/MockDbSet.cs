using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace DfE.FIAT.Data.AcademiesDb.UnitTests.Mocks;

/// <summary>
/// Use this to create a mock DbSet from a collection of items. The mock DbSet points to the collection so changes to
/// the original collection will be reflected in the mock DbSet and vice versa
/// Adapted from https://jason-ge.medium.com/mock-async-data-repository-in-asp-net-core-3-1-634cb19a3013
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public class MockDbSet<TEntity> : Mock<DbSet<TEntity>> where TEntity : class
{
    public MockDbSet(IEnumerable<TEntity> items)
    {
        var itemsAsQueryable = items.AsQueryable();

        As<IAsyncEnumerable<TEntity>>()
            .Setup(x => x.GetAsyncEnumerator(default))
            .Returns(new TestAsyncEnumerator<TEntity>(itemsAsQueryable.GetEnumerator()));
        As<IQueryable<TEntity>>()
            .Setup(m => m.Provider)
            .Returns(new TestAsyncQueryProvider<TEntity>(itemsAsQueryable.Provider));
        As<IQueryable<TEntity>>()
            .Setup(m => m.Expression).Returns(itemsAsQueryable.Expression);
        As<IQueryable<TEntity>>()
            .Setup(m => m.ElementType).Returns(itemsAsQueryable.ElementType);
        As<IQueryable<TEntity>>()
            .Setup(m => m.GetEnumerator()).Returns(itemsAsQueryable.GetEnumerator());
    }

    public sealed override Mock<TInterface> As<TInterface>()
    {
        return base.As<TInterface>();
    }

    private class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _enumerator;

        public TestAsyncEnumerator(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public T Current => _enumerator.Current;

        public ValueTask DisposeAsync()
        {
            return new ValueTask(Task.Run(() => _enumerator.Dispose()));
        }

        public ValueTask<bool> MoveNextAsync()
        {
            return new ValueTask<bool>(_enumerator.MoveNext());
        }
    }

    private class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
    {
        public TestAsyncEnumerable(Expression expression)
            : base(expression)
        {
        }

        IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);

        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken token)
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
                .Invoke(null, new[] { executionResult }) ?? throw new Exception());
        }
    }
}
