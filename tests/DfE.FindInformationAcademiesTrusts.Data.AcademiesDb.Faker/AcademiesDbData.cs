using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Cdm;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Gias;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mis;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Mstr;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Models.Ops;
using DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.UnitTests.Mocks;
using Microsoft.EntityFrameworkCore;

namespace DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker;

public class AcademiesDbData
{
    public List<CdmAccount> CdmAccounts { get; } = new();
    public List<CdmSystemuser> CdmSystemusers { get; } = new();
    public List<GiasEstablishment> GiasEstablishments { get; } = new();
    public List<GiasGovernance> GiasGovernances { get; } = new();
    public List<GiasGroupLink> GiasGroupLinks { get; } = new();
    public List<GiasGroup> GiasGroups { get; } = new();
    public List<MisEstablishment> MisEstablishments { get; } = new();
    public List<MisFurtherEducationEstablishment> MisFurtherEducationEstablishments { get; } = new();
    public List<MstrTrust> MstrTrusts { get; } = new();
    public List<MstrTrustGovernance> MstrTrustGovernances { get; } = new();
    public List<ApplicationEvent> ApplicationEvents { get; } = new();
    public List<ApplicationSetting> ApplicationSettings { get; } = new();

    public IAcademiesDbContext AsAcademiesDbContext()
    {
        return new AcademiesDbDataContext(GiasEstablishments, GiasGovernances, GiasGroupLinks, GiasGroups, MstrTrusts,
            CdmAccounts, MisEstablishments, MisFurtherEducationEstablishments, CdmSystemusers, MstrTrustGovernances, ApplicationEvents,
            ApplicationSettings);
    }

    private class AcademiesDbDataContext : IAcademiesDbContext
    {
        public DbSet<GiasEstablishment> GiasEstablishments { get; }
        public DbSet<GiasGovernance> GiasGovernances { get; }
        public DbSet<GiasGroupLink> GiasGroupLinks { get; }
        public DbSet<GiasGroup> Groups { get; }
        public DbSet<MstrTrust> MstrTrusts { get; }
        public DbSet<CdmAccount> CdmAccounts { get; }
        public DbSet<MisEstablishment> MisEstablishments { get; }
        public DbSet<MisFurtherEducationEstablishment> MisFurtherEducationEstablishments { get; }
        public DbSet<CdmSystemuser> CdmSystemusers { get; }
        public DbSet<MstrTrustGovernance> MstrTrustGovernances { get; }
        public DbSet<ApplicationEvent> ApplicationEvents { get; }
        public DbSet<ApplicationSetting> ApplicationSettings { get; }

        public AcademiesDbDataContext(
            IEnumerable<GiasEstablishment> giasEstablishments,
            IEnumerable<GiasGovernance> giasGovernances,
            IEnumerable<GiasGroupLink> giasGroupLinks,
            IEnumerable<GiasGroup> giasGroups,
            IEnumerable<MstrTrust> mstrTrusts,
            IEnumerable<CdmAccount> cdmAccounts,
            IEnumerable<MisEstablishment> misEstablishments,
            IEnumerable<MisFurtherEducationEstablishment> misFurtherEducationEstablishment,
            IEnumerable<CdmSystemuser> cdmSystemusers,
            IEnumerable<MstrTrustGovernance> mstrTrustGovernances,
            IEnumerable<ApplicationEvent> applicationEvents,
            IEnumerable<ApplicationSetting> applicationSettings)
        {
            ApplicationEvents = new MockDbSet<ApplicationEvent>(applicationEvents).Object;
            ApplicationSettings = new MockDbSet<ApplicationSetting>(applicationSettings).Object;
            GiasEstablishments = new MockDbSet<GiasEstablishment>(giasEstablishments).Object;
            GiasGovernances = new MockDbSet<GiasGovernance>(giasGovernances).Object;
            GiasGroupLinks = new MockDbSet<GiasGroupLink>(giasGroupLinks).Object;
            Groups = new MockDbSet<GiasGroup>(giasGroups).Object;
            MstrTrusts = new MockDbSet<MstrTrust>(mstrTrusts).Object;
            CdmAccounts = new MockDbSet<CdmAccount>(cdmAccounts).Object;
            MisEstablishments = new MockDbSet<MisEstablishment>(misEstablishments).Object;
            MisFurtherEducationEstablishments =
                new MockDbSet<MisFurtherEducationEstablishment>(misFurtherEducationEstablishment).Object;
            CdmSystemusers = new MockDbSet<CdmSystemuser>(cdmSystemusers).Object;
            MstrTrustGovernances = new MockDbSet<MstrTrustGovernance>(mstrTrustGovernances).Object;
        }
    }
}
