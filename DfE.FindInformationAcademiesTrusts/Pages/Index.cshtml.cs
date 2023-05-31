using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DfE.FindInformationAcademiesTrusts.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IAcademiesApi _academiesApi;

    public IndexModel(ILogger<IndexModel> logger, IAcademiesApi academiesApi)
    {
        _logger = logger;
        _academiesApi = academiesApi;
    }

    public async Task OnGetAsync()
    {
        var trusts = await _academiesApi.GetTrusts();
        _logger.LogInformation(trusts);
    }
}
