using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages.ManageMyCasework.Overview
{
    public class IndexModel : BasePageModel
    {

        [BindProperty]
        public ProjectListFilters Filters { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int TotalProjects { get; set; }

        public void OnGet()
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
                "Record concerns and support for trusts"
            }; ;
        }
    }
}
