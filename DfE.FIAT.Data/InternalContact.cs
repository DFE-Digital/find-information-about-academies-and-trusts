namespace DfE.FIAT.Data;

public record InternalContact(
    string FullName,
    string Email,
    DateTime LastModifiedAtTime,
    string LastModifiedByEmail
) : Person(FullName, Email);
