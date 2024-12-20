using System.Collections;
using System.Reflection;
using DfE.FindInformationAcademiesTrusts.Data;

namespace DfE.FindInformationAcademiesTrusts.UnitTests.WebHost.Base;

public class FiatObjectDefaultValueProvider : LookupOrFallbackDefaultValueProvider
{
    public FiatObjectDefaultValueProvider()
    {
        //Collections have to be registered separately
        Register(typeof(Array), (type, _) => CreateArray(type));
        Register(typeof(IEnumerable), (_, _) => Array.Empty<object>());
        Register(typeof(IEnumerable<>), (type, _) => Array.CreateInstance(type.GetGenericArguments()[0], 0));
        Register(typeof(IQueryable), (_, _) => Array.Empty<object>().AsQueryable());
        Register(typeof(IQueryable<>), (type, _) => CreateQueryableOf(type));
        Register(typeof(Dictionary<,>), (type, _) => CreateDictionaryOf(type));
        Register(typeof(IReadOnlyDictionary<,>), (type, _) => CreateDictionaryOf(type));

        //Register the records used by FIAT
        foreach (var fiatRecord in FiatRecords)
        {
            Register(fiatRecord, CreateDefaultRecord);
        }
    }

    protected override object GetFallbackDefaultValue(Type type, Mock mock)
    {
        //If this is a nullable type then default it to null
        if (Nullable.GetUnderlyingType(type) is not null)
#pragma warning disable CS8603 // Possible null reference return. Our base type is from Moq library which is not strict about nulls
            return null;
#pragma warning restore CS8603 // Possible null reference return.

        //Otherwise select a pre-defined value
        if (type == typeof(string))
            return "Some string";
        if (type == typeof(int))
            return 42;
        if (type == typeof(bool))
            return false;
        if (type == typeof(DateTime))
            return DateTime.Now;

        //If no pre-defined value then get one from Moq
        return base.GetFallbackDefaultValue(type, mock) ??
               throw new InvalidOperationException($"Type {type.FullName} is not supported by mocking implementation.");
    }

    private static Type[] FiatRecords { get; }
        = typeof(Program).Assembly.GetTypes().Where(IsRecord)
            .Concat(typeof(Person).Assembly.GetTypes().Where(IsRecord))
            .ToArray();

    private object CreateDefaultRecord(Type recordType, Mock mock)
    {
        var recordParams = recordType.GetConstructors().First().GetParameters();
        var argumentValues = recordParams
            .Select(p => GetDefaultParameterValue(p, mock))
            .ToArray();

        var defaultRecord = Activator.CreateInstance(recordType, argumentValues);

        return defaultRecord ?? throw new InvalidOperationException($"Could not create record {recordType.FullName}");
    }

    private static bool IsRecord(Type t)
    {
        return t.IsClass
               && t.Namespace != null
               && t.Namespace.StartsWith("DfE.FindInformationAcademiesTrusts")
               && t.GetConstructor(Type.EmptyTypes) is null
               && t.GetConstructors().Length != 0
               && t.GetInterfaces().Any(i => i.Name == typeof(IEquatable<>).Name);
    }

    private static object CreateArray(Type type)
    {
        var elementType = type.GetElementType()!;
        var lengths = new int[type.GetArrayRank()];
        return Array.CreateInstance(elementType, lengths);
    }

    private static object CreateQueryableOf(Type type)
    {
        var elementType = type.GetGenericArguments()[0];
        var array = Array.CreateInstance(elementType, 0);

        return typeof(Queryable).GetMember("AsQueryable").OfType<MethodInfo>()
            .Single(x => x.IsGenericMethod)
            .MakeGenericMethod(elementType)
            .Invoke(null, [array]) ?? throw new ApplicationException($"Could not create Queryable {type}");
    }

    private static object CreateDictionaryOf(Type type)
    {
        var dictType = typeof(Dictionary<,>).MakeGenericType(type.GetGenericArguments());

        return Activator.CreateInstance(dictType) ??
               throw new ApplicationException($"Could not create dictionary {type}");
    }
}
