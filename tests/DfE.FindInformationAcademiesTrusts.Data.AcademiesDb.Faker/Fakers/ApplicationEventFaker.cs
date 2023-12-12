using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Ops;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker.Fakers;

public class ApplicationEventFaker
{
    private static int _appEventId;
    private readonly DateTime _refDate;

    public ApplicationEventFaker(DateTime refDate)
    {
        _refDate = refDate;
    }

    private ApplicationEvent CreateApplicationEvent(DateTime? dateTime, string description,
        string? message = "Finished",
        string? source = "adf-t1ts-sips-dataflow", char? eventType = 'I')
    {
        return new ApplicationEvent
        {
            Id = _appEventId++,
            DateTime = dateTime,
            Source = source,
            UserName = "Test User",
            EventType = eventType,
            Level = 1,
            Code = 1,
            Severity = 'S',
            Description = description,
            Message = message,
            Trace = "test trace",
            ProcessID = 1,
            LineNumber = 2
        };
    }

    public IEnumerable<ApplicationEvent> Generate()
    {
        return new List<ApplicationEvent>
        {
            CreateApplicationEvent(_refDate.AddDays(-1), "GIAS_Daily"),
            CreateApplicationEvent(_refDate.AddDays(-2), "MSTR_Daily"),
            CreateApplicationEvent(_refDate.AddDays(-3), "CDM_Daily")
        };
    }
}
