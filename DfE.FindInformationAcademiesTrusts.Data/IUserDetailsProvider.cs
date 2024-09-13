namespace DfE.FindInformationAcademiesTrusts.Data;

public interface IUserDetailsProvider
{
    public (string Name, string Email) GetUserDetails();
}
