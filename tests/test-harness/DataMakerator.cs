using System.Collections;
using System.Reflection;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.Trust;

namespace test_harness;

public static class DataMakerator
{
    public static Task<T> CreateTaskOfTypeFromId<T>(string id)
    {
        return CreateTaskOfTypeFromId<T>(int.Parse(id));
    }

    public static Task<T> CreateTaskOfTypeFromId<T>(int id)
    {
        return Task.FromResult(CreateInstanceOfTypeFromId<T>(id));
    }

    public static T CreateInstanceOfTypeFromId<T>(int id)
    {
        return (T)CreateInstanceOfTypeFromId(typeof(T), id, null);
    }

    private static object[] GetArguments(int id, ConstructorInfo constructorInfo)
    {
        return constructorInfo.GetParameters()
            .Select<ParameterInfo, object>(p =>
                CreateInstanceOfTypeFromId(p.ParameterType, id, p.Name))
            .ToArray();
    }

    private static string GetStringForPropertyFromId(string? propertyName, int id)
    {
        return propertyName switch
        {
            null or nameof(TrustSummaryServiceModel.Uid)
                or nameof(AcademyDetailsServiceModel.Urn) => $"{id}",
            nameof(TrustSummaryServiceModel.Type) => IsSingleAcademyTrust(id)
                ? "Single-academy trust"
                : "Multi-academy trust",
            nameof(TrustOverviewServiceModel.GroupId) => $"TRN{id:d5}",
            nameof(TrustOverviewServiceModel.CompaniesHouseNumber) => $"{id:d8}",
            _ => $"{propertyName} {id}"
        };
    }

    private static bool IsSingleAcademyTrust(int uid)
    {
        return uid % 2 == 0;
    }

    private static int GetIntForPropertyFromUid(string? propertyName, int id)
    {
        return propertyName switch
        {
            nameof(TrustSummaryServiceModel.NumberOfAcademies)
                or nameof(TrustOverviewServiceModel.TotalAcademies) => GetNumberOfAcademiesForUid(id),
            _ => id
        };
    }

    public static int GetNumberOfAcademiesForUid(int uid)
    {
        return IsSingleAcademyTrust(uid) ? 1 : uid % 20;
    }

    private static object CreateInstanceOfTypeFromId(Type typeToCreate, int id, string? propertyName)
    {
        if (typeToCreate.IsGenericType)
        {
            var genericTypeDefinition = typeToCreate.GetGenericTypeDefinition();
            if (genericTypeDefinition == typeof(Nullable<>))
            {
                return CreateInstanceOfTypeFromId(typeToCreate.GenericTypeArguments.Single(), id, propertyName);
            }

            if (genericTypeDefinition == typeof(Array))
            {
                return CreateArray(typeToCreate);
            }

            if (genericTypeDefinition == typeof(IEnumerable))
            {
                return Array.Empty<object>();
            }

            if (genericTypeDefinition == typeof(IEnumerable<>))
            {
                return Array.CreateInstance(typeToCreate.GetGenericArguments()[0], 0);
            }

            if (genericTypeDefinition == typeof(IQueryable))
            {
                return Array.Empty<object>().AsQueryable();
            }

            if (genericTypeDefinition == typeof(IQueryable<>))
            {
                return CreateQueryableOf(typeToCreate);
            }

            if (genericTypeDefinition == typeof(Dictionary<,>) ||
                genericTypeDefinition == typeof(IReadOnlyDictionary<,>))
            {
                return CreateDictionaryOf(typeToCreate);
            }

            throw new ArgumentOutOfRangeException(nameof(typeToCreate));
        }

        if (typeToCreate == typeof(string))
        {
            return GetStringForPropertyFromId(propertyName, id);
        }

        if (typeToCreate == typeof(int))
        {
            return GetIntForPropertyFromUid(propertyName, id);
        }

        if (typeToCreate == typeof(double))
        {
            return id / 10000d + id % 10;
        }

        if (typeToCreate == typeof(bool))
        {
            return false;
        }

        if (typeToCreate == typeof(DateTime))
        {
            return DateTime.UtcNow.AddDays(-id % 500);
        }

        if (typeToCreate.IsEnum)
        {
            return Enum.GetValues(typeToCreate).GetValue(0) ?? 0;
        }

        if (IsFiatRecord(typeToCreate))
        {
            var constructorInfo = typeToCreate.GetConstructors().First();

            var parameters = GetArguments(id, constructorInfo);

            return constructorInfo.Invoke(parameters);
        }

        throw new ArgumentOutOfRangeException(nameof(typeToCreate));
    }

    private static bool IsFiatRecord(Type t)
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
