namespace DfE.FindInformationAcademiesTrusts.IntegrationTests.Base;

[CollectionDefinition(nameof(UseApplicationWithMockedServices))]
public class UseApplicationWithMockedServices : ICollectionFixture<ApplicationWithMockedServices>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
