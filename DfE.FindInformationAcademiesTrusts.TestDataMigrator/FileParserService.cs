using System.Text.Json;

namespace DfE.FindInformationAcademiesTrusts.TestDataMigrator;

public class FileParserService
{
    public enum FileType
    {
        Group = 1,
        GroupLink = 2,
        Establishment = 3
    }

    public async Task<List<T>> ParseFiles<T>(FileType fileType)
    {
        List<T> fileData = [];

        var fileName = fileType switch
        {
            FileType.GroupLink => "GroupLink.json",
            FileType.Establishment => "Establishment.json",
            _ => "Group.json"
        };

        var files = Directory.GetFiles("Data//", "*" + fileName + "*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
            var data = await ReadFileDataAsync<T>(file);

            if (data != null) fileData.AddRange(data);
        }

        return fileData;
    }

    private async Task<List<T>?> ReadFileDataAsync<T>(string fileName)
    {
        using var r = new StreamReader(fileName);
        var json = await r.ReadToEndAsync();

        return JsonSerializer.Deserialize<List<T>>(json);
    }
}
