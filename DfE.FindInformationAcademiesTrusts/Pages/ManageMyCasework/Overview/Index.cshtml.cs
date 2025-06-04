using Dfe.CaseAggregationService.Client.Contracts;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.ManageMyCasework;
using DocumentFormat.OpenXml.Office.PowerPoint.Y2021.M06.Main;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.ManageMyCasework.Overview
{
    public class IndexModel : BasePageModel, IPaginationModel
    {
        private readonly IGetCasesService _getCasesService;

        [BindProperty]
        public ProjectListFilters Filters { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string Sorting { get; set; } = ResultSorting.createdDesc;

        [BindProperty]
        public int TotalProjects { get; set; }

        public string PageName => "ManageMyCasework/Overview/Index";

        public IPageStatus PageStatus => Cases.PageStatus;
        
        public Dictionary<string, string> PaginationRouteData { get; set; } = new();
        
        [BindProperty(SupportsGet = true)] 
        public int PageNumber { get; set; } = 1;
        
        public IPaginatedList<UserCaseInfo> Cases { get; set; } = PaginatedList<UserCaseInfo>.Empty();

        public IndexModel(IGetCasesService getCasesService)
        {
            _getCasesService = getCasesService;
            ShowHeaderSearch = false;
        }

        public async Task OnGetAsync()
        {
            Filters.PersistUsing(TempData).PopulateFrom(Request.Query);

            Filters.AvailableProjectTypes = new List<string>()
            {
                "Conversion",
                "Form a MAT",
                "Governance capability",
                "Non-compliance",
                "Pre-opening",
                "Pre-opening - not included in figures",
                "Safeguarding non-compliance",
                "Transfer"
            };

            Filters.AvailableSystems = new List<string>() 
            {
                "Complete conversions, transfers and changes",
                "Manage free school projects",
                "Prepare conversions and transfers",
                "Record concerns and support for trusts",
            };
            
            Cases = await _getCasesService.GetCasesAsync("Paul Lockwood", "paul.lockwood@education.gov.uk", IncludeSigChange(), IncludePrepare(), false, false,false, false, Filters.Title, PageNumber, 25, Filters.SelectedProjectTypes, ConvertSortCriteria());

            TotalProjects = Cases.Count();

            PaginationRouteData = new Dictionary<string, string> { { nameof(Sorting), Sorting } };
        }

        private bool IncludePrepare() => Include("Prepare conversions and transfers");
        private bool IncludeSigChange() => Include("Significant change");

        private bool Include(string system)
        {
            if (Filters.SelectedSystems.Length == 0)
            {
                return true;
            }

            return Filters.SelectedSystems.Contains(system);
        }

        private SortCriteria ConvertSortCriteria()
        {
            return Sorting switch
            {
                ResultSorting.createdAsc => SortCriteria.CreatedDateAscending,
                ResultSorting.createdDesc => SortCriteria.CreatedDateDescending,
                ResultSorting.updatedAsc => SortCriteria.UpdatedDateAscending,
                ResultSorting.updatedDesc => SortCriteria.UpdatedDateDescending,
                _ => SortCriteria.CreatedDateDescending
            };
        }

        
    }


}
