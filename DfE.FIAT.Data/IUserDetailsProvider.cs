namespace DfE.FIAT.Data;

public interface IUserDetailsProvider
{
    public (string Name, string Email) GetUserDetails();
}
