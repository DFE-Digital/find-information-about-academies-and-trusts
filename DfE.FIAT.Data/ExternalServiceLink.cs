using System.Diagnostics.CodeAnalysis;

namespace DfE.FIAT.Data;

[ExcludeFromCodeCoverage]
public record ExternalServiceLink(string ServiceName, string ServiceDescription, string Url);
