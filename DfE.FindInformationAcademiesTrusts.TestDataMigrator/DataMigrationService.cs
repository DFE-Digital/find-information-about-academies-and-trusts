using System.Text.Json;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.TestDataMigrator.Queries;
using DfE.FindInformationAcademiesTrusts.TestDataMigrator.Repositories;

namespace DfE.FindInformationAcademiesTrusts.TestDataMigrator;

public class DataMigrationService(GenericRepository repository)
{
    public async Task StartMigrations()
    {
        var groupsTask = ParseJsonFileAsync<GiasGroup>("Group.json");
        var groupLinksTask = ParseJsonFileAsync<GiasGroupLink>("GroupLink.json");
        var establishmentsTask = ParseJsonFileAsync<GiasEstablishment>("Establishment.json");

        await Task.WhenAll(groupsTask, groupLinksTask, establishmentsTask);

        var groups = await groupsTask;
        var groupLinks = await groupLinksTask;
        var establishments = await establishmentsTask;

        if (groups != null)
        {
            await repository.InsertAsync(GroupQueries.Insert, groups);
        }

        if (groupLinks != null)
        {
            await repository.InsertAsync(GroupLinkQueries.Insert, groupLinks);
        }

        if (establishments != null)
        {
            await repository.InsertAsync(EstablishmentsQueries.Insert, establishments);
        }
    }

    private async Task<List<T>?> ParseJsonFileAsync<T>(string fileName)
    {
        using var r = new StreamReader($"Data//{fileName}");
        var json = await r.ReadToEndAsync();

        return JsonSerializer.Deserialize<List<T>>(json);
    }
}
