namespace DfE.FIAT.Web.Services.Academy;

public record AcademyFreeSchoolMealsServiceModel(
    string Urn,
    string? EstablishmentName,
    double? PercentageFreeSchoolMeals,
    double LaAveragePercentageFreeSchoolMeals,
    double NationalAveragePercentageFreeSchoolMeals
);
