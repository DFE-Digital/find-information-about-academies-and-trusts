using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using System.Text.Json;
using DfE.FindInformationAcademiesTrusts.TestDataMigrator.Queries;
using DfE.FindInformationAcademiesTrusts.TestDataMigrator.Repositories;

namespace DfE.FindInformationAcademiesTrusts.TestDataMigrator;

public class DataMigrationService(GenericRepository repository)
{

    public async Task StartMigrations()
    {
        var groups = await ParseJsonFileAsync<GiasGroup>("Group.json");
        var groupLinks = await ParseJsonFileAsync<GiasGroupLink>("GroupLink.json");

        if (groups != null)
        {
            await repository.InsertAsync(GroupQueries.Insert, groups);
        }

        if (groupLinks != null)
        {
            await repository.InsertAsync(GroupLinkQueries.Insert, groupLinks);
        }
    }

    private async Task<List<T>?> ParseJsonFileAsync<T>(string fileName)
    {
        using StreamReader r = new StreamReader($"Data//{fileName}");
        string json = await r.ReadToEndAsync();

        return JsonSerializer.Deserialize<List<T>>(json);
    }

}