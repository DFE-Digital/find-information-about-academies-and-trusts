using Dfe.CaseAggregationService.Client.Contracts;
using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.ManageProjectsAndCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using DfE.FindInformationAcademiesTrusts.Extensions;

namespace DfE.FindInformationAcademiesTrusts.Pages.ManageProjectsAndCases.Overview
{
    [Authorize(Roles = "User.Role.MPCViewer")]
    public class IndexModel : BasePageModel, IPaginationModel
    {
        private readonly IGetCasesService _getCasesService;
        private readonly IWebHostEnvironment _environment;

        [BindProperty]
        public ProjectListFilters Filters { get; init; } = new();

        [BindProperty(SupportsGet = true)]
        public string Sorting { get; set; } = ResultSorting.createdDesc;

        [BindProperty]
        public int TotalProjects { get; set; }

        public string PageName => "ManageMyProjectsAndCases/Overview/Index";

        public IPageStatus PageStatus => Cases.PageStatus;
        
        public Dictionary<string, string> PaginationRouteData { get; set; } = new();
        
        [BindProperty(SupportsGet = true)] 
        public int PageNumber { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public string? FakeEmail { get; set; } = null;

        public IPaginatedList<UserCaseInfo> Cases { get; set; } = PaginatedList<UserCaseInfo>.Empty();

        public IndexModel(IGetCasesService getCasesService,
                          IWebHostEnvironment environment)
        {
            _getCasesService = getCasesService;
            _environment = environment;
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

            var userEmail = User.Identity?.Name;

            if (!_environment.IsLiveEnvironment() && FakeEmail != null)
            {
                userEmail = FakeEmail;
            }

            Cases = await _getCasesService.GetCasesAsync(
                new GetCasesParameters
                (
                    userEmail,
                    userEmail,
                    IncludePrepare(),
                    IncludeComplete(),
                    IncludeManageFreeSchools(),
                    IncludeConcerns(),
                    PageNumber,
                    25,
                    Filters.SelectedProjectTypes,
                    ConvertSortCriteria()
                ));

            TotalProjects = Cases.Count();

            PaginationRouteData = new Dictionary<string, string> { { nameof(Sorting), Sorting } };
        }

        private bool IncludePrepare() => Include("Prepare conversions and transfers");
        private bool IncludeComplete() => Include("Complete conversions, transfers and changes");
        private bool IncludeManageFreeSchools() => Include("Manage free school projects");
        private bool IncludeConcerns() => Include("Record concerns and support for trusts");

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
