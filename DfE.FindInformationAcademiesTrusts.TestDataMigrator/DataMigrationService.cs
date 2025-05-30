using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.TestDataMigrator.Queries;
using DfE.FindInformationAcademiesTrusts.TestDataMigrator.Repositories;

namespace DfE.FindInformationAcademiesTrusts.TestDataMigrator;

public class DataMigrationService(FileParserService fileParserService, GenericRepository repository)
{
    public async Task StartMigrations()
    {
        var groupsTask = fileParserService.ParseFiles<GiasGroup>(FileParserService.FileType.Group);
        var groupLinksTask = fileParserService.ParseFiles<GiasGroupLink>(FileParserService.FileType.GroupLink);
        var establishmentsTask =
            fileParserService.ParseFiles<GiasEstablishment>(FileParserService.FileType.Establishment);

        await Task.WhenAll(groupsTask, groupLinksTask, establishmentsTask);

        var groups = await groupsTask;
        var groupLinks = await groupLinksTask;
        var establishments = await establishmentsTask;

        var insertGroupsTask = repository.InsertAsync(GroupQueries.Insert, groups);
        var insertGroupLinksTask = repository.InsertAsync(GroupLinkQueries.Insert, groupLinks);
        var insertEstablishmentsTask = repository.InsertAsync(EstablishmentsQueries.Insert, establishments);

        await Task.WhenAll(insertGroupsTask, insertGroupLinksTask, insertEstablishmentsTask);
    }
}
