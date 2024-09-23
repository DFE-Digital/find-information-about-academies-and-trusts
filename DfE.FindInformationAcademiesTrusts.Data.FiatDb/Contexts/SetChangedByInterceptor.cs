using System.Diagnostics.CodeAnalysis;
using DfE.FindInformationAcademiesTrusts.Data.FiatDb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DfE.FindInformationAcademiesTrusts.Data.FiatDb.Contexts;

[ExcludeFromCodeCoverage(Justification = "Tests are long running, don't want these to be mutated by Stryker")]
public class SetChangedByInterceptor(IUserDetailsProvider userDetailsProvider) : ISaveChangesInterceptor
{
    public ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        SetChangedBy(eventData.Context!);

        return ValueTask.FromResult(result);
    }

    public InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        SetChangedBy(eventData.Context!);

        return result;
    }

    private void SetChangedBy(DbContext context)
    {
        var (userName, email) = userDetailsProvider.GetUserDetails();

        context.ChangeTracker.DetectChanges();

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is BaseEntity baseEntity)
            {
                baseEntity.LastModifiedByName = userName;
                baseEntity.LastModifiedByEmail = email;
            }
        }
    }
}
