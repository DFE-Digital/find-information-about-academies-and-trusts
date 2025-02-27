using System.Diagnostics.CodeAnalysis;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Exceptions;

[ExcludeFromCodeCoverage] //Simple exception class
public class DataIntegrityException : Exception
{
    public DataIntegrityException()
    {
    }

    public DataIntegrityException(string? message) : base(message)
    {
    }

    public DataIntegrityException(string? message, Exception? innerException) : base(message,
        innerException)
    {
    }
}
