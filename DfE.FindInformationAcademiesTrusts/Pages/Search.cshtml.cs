using DfE.FindInformationAcademiesTrusts.Data;
using DfE.FindInformationAcademiesTrusts.Pages.Shared;
using DfE.FindInformationAcademiesTrusts.Services.Academy;
using DfE.FindInformationAcademiesTrusts.Services.Trust;
using Microsoft.AspNetCore.Mvc;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class SearchModel : ContentPageModel, IPageSearchFormModel, IPaginationModel
{
    public record AutocompleteEntry(string Address, string Name, string? Id, string Type);


    private readonly ITrustSearch _trustSearch;
    private readonly ITrustService _trustService;
    public string PageName { get; } = "Search";
    public IPageStatus PageStatus { get; set; }
    public Dictionary<string, string> PaginationRouteData { get; set; } = new();
    public string PageSearchFormInputId => "search";
    [BindProperty(SupportsGet = true)] public string Uid { get; set; } = string.Empty;
    [BindProperty(SupportsGet = true)] public int PageNumber { get; set; } = 1;

    public IPaginatedList<TrustSearchEntry> Trusts { get; set; } =
        PaginatedList<TrustSearchEntry>.Empty();
    [BindProperty(SupportsGet = true)] public string Urn { get; set; } = string.Empty;

    public IPaginatedList<AcademyDetailsServiceModel> Academies { get; set; } = PaginatedList<AcademyDetailsServiceModel>.Empty();

    private readonly IAcademyService _academyService;

    public SearchModel(
        ITrustService trustService,
        ITrustSearch trustSearch,
        IAcademyService academyService)
    {
        _trustSearch = trustSearch;
        _trustService = trustService;
        _academyService = academyService;
        PageStatus = Trusts.PageStatus;
        ShowHeaderSearch = false;
    }

    public IActionResult OnPost()
    {
        return RedirectToPage("/Search", new { KeyWords, Uid, Urn });
    }


    public async Task<IActionResult> OnGetAsync()
    {
        if (!string.IsNullOrWhiteSpace(Uid))
        {
            var trust = await _trustService.GetTrustSummaryAsync(Uid);
            if (trust != null && string.Equals(trust.Name, KeyWords, StringComparison.CurrentCultureIgnoreCase))
            {
                return RedirectToPage("/Trusts/Details", new { Uid });
            }
        }

        if (!string.IsNullOrWhiteSpace(Urn))
        {
            var academy = await _academyService.GetAcademyDetailsAsync(Urn);
            if (academy != null && string.Equals(academy.EstablishmentName, KeyWords, StringComparison.CurrentCultureIgnoreCase))
            {
                return RedirectToPage("/Academies/Details", new { Urn });
            }
        }

        Trusts = await GetTrustsForKeywords();
        Academies = await GetAcademiesForKeywords();

        PageStatus = Trusts.PageStatus;
        PaginationRouteData = new Dictionary<string, string>
{
    { "Keywords", KeyWords ?? string.Empty },
    { "Uid", Uid },
    { "Urn", Urn }
};

        return Page();
    }
    private async Task<IPaginatedList<AcademyDetailsServiceModel>> GetAcademiesForKeywords()
    {
        if (string.IsNullOrEmpty(KeyWords))
        {
            return PaginatedList<AcademyDetailsServiceModel>.Empty();
        }

        var academies = await _academyService.SearchAcademiesAsync(KeyWords, PageNumber);
        return academies;
    }


    private async Task<IPaginatedList<TrustSearchEntry>> GetTrustsForKeywords()
    {
        return !string.IsNullOrEmpty(KeyWords)
            ? await _trustSearch.SearchAsync(KeyWords, PageNumber)
            : PaginatedList<TrustSearchEntry>.Empty();
    }

    public async Task<IActionResult> OnGetPopulateAutocompleteAsync()
    {
        var trusts = await GetTrustsForKeywords();
        var academies = await GetAcademiesForKeywords();

        var autocompleteEntries = trusts.Select(trust => new AutocompleteEntry(
            trust.Address,
            trust.Name,
            trust.Uid,
            Type: "Trust"
        )).Concat(academies.Select(academy => new AutocompleteEntry(
            string.Empty,
            academy?.EstablishmentName ?? string.Empty,
            academy?.Urn,
            Type: "Academy"
        )));

        return new JsonResult(autocompleteEntries);
    }


    public string Title
    {
        get
        {
            if (string.IsNullOrWhiteSpace(KeyWords))
            {
                return "Search";
            }

            if (PageStatus.TotalPages > 1)
            {
                return $"Search (page {PageStatus.PageIndex} of {PageStatus.TotalPages}) - {KeyWords}";
            }

            return $"Search - {KeyWords}";
        }
    }
}
