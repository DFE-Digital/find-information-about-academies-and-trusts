namespace DfE.FindInformationAcademiesTrusts.Services.Academy;

public record AcademyFreeSchoolMealsServiceModel(
    string Urn,
    string? EstablishmentName,
    double? PercentageFreeSchoolMeals,
    double LaAveragePercentageFreeSchoolMeals,
    double NationalAveragePercentageFreeSchoolMeals
);
