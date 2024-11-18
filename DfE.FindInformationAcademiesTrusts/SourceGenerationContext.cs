using System.Text.Json;
using System.Text.Json.Serialization;

namespace DfE.FindInformationAcademiesTrusts;

/// <summary>
/// Use source generation to speed up JSON serialization
/// See https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/source-generation
/// </summary>
[JsonSourceGenerationOptions(JsonSerializerDefaults.Web)]
[JsonSerializable(typeof(AutocompleteEntry[]))]
[JsonSerializable(typeof(AutocompleteEntry))]
[JsonSerializable(typeof(string))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}
