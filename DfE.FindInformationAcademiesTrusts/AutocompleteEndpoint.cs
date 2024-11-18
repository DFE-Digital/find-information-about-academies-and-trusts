using DfE.FindInformationAcademiesTrusts.Data;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DfE.FindInformationAcademiesTrusts;

public record AutocompleteEntry(string Address, string Name, string? TrustId);

public class AutocompleteHandler(ITrustSearch trustSearch)
{
    public async Task<JsonHttpResult<AutocompleteEntry[]>> OnGet(string keyWords)
    {
        if (string.IsNullOrEmpty(keyWords))
            return TypedResults.Json(Array.Empty<AutocompleteEntry>());

        var searchResults = await trustSearch.SearchAutocompleteAsync(keyWords);

        return TypedResults.Json(searchResults
                .Select(trust =>
                    new AutocompleteEntry(
                        trust.Address,
                        trust.Name,
                        trust.Uid
                    ))
                .ToArray(),
            SourceGenerationContext.Default.AutocompleteEntryArray);
    }
}
