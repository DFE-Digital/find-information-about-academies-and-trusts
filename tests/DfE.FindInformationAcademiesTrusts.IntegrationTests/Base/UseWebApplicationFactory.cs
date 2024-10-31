using Microsoft.AspNetCore.Mvc.Testing;

namespace DfE.FindInformationAcademiesTrusts.IntegrationTests.Base;

[CollectionDefinition(nameof(UseWebApplicationFactory))]
public class UseWebApplicationFactory : ICollectionFixture<WebApplicationFactory<Program>>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}
