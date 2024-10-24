using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data;

[ExcludeFromCodeCoverage]
public record ExternalServiceLink(string ServiceName, string ServiceDescription, string Url);
