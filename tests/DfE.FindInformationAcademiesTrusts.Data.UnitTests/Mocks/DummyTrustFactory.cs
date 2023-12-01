namespace DfE.FindInformationAcademiesTrusts.Data.UnitTests.Mocks;

public class DummyTrustFactory
{
    private int _numberTrustsGenerated;
    private const string OpenStatus = "Open";
    public const string ClosedStatus = "Closed";

    public Trust GetDummyTrust()
    {
        _numberTrustsGenerated++;
        return GetDummyTrust(_numberTrustsGenerated.ToString("0000"));
    }

    public static Trust GetDummyMultiAcademyTrust(string uid, string companiesHouseNumber = "test",
        string status = OpenStatus)
    {
        return GetDummyTrust(uid, "Multi-academy trust", companiesHouseNumber, status: status);
    }

    public static Trust GetDummySingleAcademyTrust(string uid, Academy? academy = null, string status = OpenStatus)
    {
        var academies = academy is not null ? new[] { academy } : Array.Empty<Academy>();
        return GetDummyTrust(uid, "Single-academy trust", academies: academies);
    }

    public static Trust GetDummyTrustWithGovernors()
    {
        return GetDummyTrust("1234", governors: ListOfGovernors());
    }


    public static Trust GetDummyTrust(string uid, string type = "test", string companiesHouseNumber = "test",
        Academy[]? academies = null, Governor[]? governors = null, string status = OpenStatus)
    {
        return new Trust(uid,
            $"Trust {uid}",
            "test",
            "test",
            type,
            "test",
            new DateTime(),
            companiesHouseNumber,
            "test",
            academies ?? Array.Empty<Academy>(),
            governors ?? Array.Empty<Governor>(),
            new Person("Present Trm", "trm@test.com"),
            new Person("Present Sfsolead", "Sfsolead@test.com"),
            status
        );
    }

    private static Governor[] ListOfGovernors()
    {
        Governor[] listOfGovernors =
        {
            new("1", "1", "Past Chair", Email: "pastchair@test.com", Role: "Chair Of Trustees",
                AppointingBody: null, DateOfAppointment: null, DateOfTermEnd: DateTime.Today.AddDays(-1)),
            new("2", "2", "Present Chair", Email: "presentchair@test.com", Role: "Chair Of Trustees",
                AppointingBody: null, DateOfAppointment: null, DateOfTermEnd: DateTime.Today),
            new("3", "3", "Past Accountingofficer", Email: "pastao@test.com", Role: "Accounting Officer",
                AppointingBody: null, DateOfAppointment: null, DateOfTermEnd: DateTime.Today.AddDays(-1)),
            new("4", "4", "Present Accountingofficer", Email: "presentao@test.com", Role: "Accounting Officer",
                AppointingBody: null, DateOfAppointment: null, DateOfTermEnd: DateTime.Today),
            new("5", "5", "Past Chieffinancialofficer", Email: "pastcfo@test.com", Role: "Chief Financial Officer",
                AppointingBody: null, DateOfAppointment: null, DateOfTermEnd: DateTime.Today.AddDays(-1)),
            new("6", "6", "Present Chieffinancialofficer", Email: "presentcfo@test.com",
                Role: "Chief Financial Officer", AppointingBody: null, DateOfAppointment: null, DateOfTermEnd: null)
        };

        return listOfGovernors;
    }
}
