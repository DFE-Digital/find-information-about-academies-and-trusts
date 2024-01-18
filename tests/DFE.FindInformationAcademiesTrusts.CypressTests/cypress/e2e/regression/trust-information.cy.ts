import { Logger } from "cypress/common/logger";
import academiesDetailsTable from "cypress/pages/academiesDetailsTable";
import academiesFreeSchoolMealsTable from "cypress/pages/academiesFreeSchoolMealsTable";
import academiesOfstedRatingsTable from "cypress/pages/academiesOfstedRatingsTable";
import academiesPupilNumbersTable from "cypress/pages/academiesPupilNumbersTable";
import searchTrustPage from "cypress/pages/searchTrustPage";
import trustContactsPage from "cypress/pages/trustContactsPage";
import trustDetailsPage from "cypress/pages/trustDetailsPage";
import trustOverviewPage from "cypress/pages/trustOverviewPage";
import trustPage from "cypress/pages/trustPage";

describe("Searching for a trust and checking information", () => {
    beforeEach(() => {
        cy.login();
    });

    it("Should be able to find the trust and it should have the correct information", () => {
        Logger.log("Searching for a trust");

        searchTrustPage
            .enterSearchText("St Mary's")
            .withOption("ST MARY'S ACADEMY TRUST")
            .then((option) => {
                option
                    .hasName("ST MARY'S ACADEMY TRUST")
                    .hasHint("150 Madison Ville, Lempi Junction, Darrellport, HR52 1BO")
                    .select();
            });

        cy.excuteAccessibilityTests();

        searchTrustPage.search();

        trustPage
            .hasName("ST MARY'S ACADEMY TRUST")
            .hasType("Single-academy trust");

        Logger.log("Checking the trust details");
        trustDetailsPage
            .hasAddress("150 Madison Ville, Lempi Junction, Darrellport, HR52 1BO")
            .hasOpenedOn("18 Jul 2014")
            .hasRegionAndTerritory("North West")
            .hasUid("1283")
            .hasTrustReferenceNumber("TR1471")
            .hasUkprn("10042719")
            .hasCompaniesHouseNumber("07892880")

        cy.excuteAccessibilityTests();

        Logger.log("Checking the contacts page");
        trustPage.viewContacts();

        trustContactsPage
            .hasTrustRelationshipManager("Cindy Bergstrom", "Cindy.Bergstrom@education.gov.uk")
            .hasSfsoLead("Vickie Gulgowski", "Vickie.Gulgowski@education.gov.uk")
            .hasAccountingOfficer("Grant Schroeder", "Grant.Schroeder@thetrust.com")
            .hasChiefFinancialOfficer("Brandi Bernier", "Brandi.Bernier@thetrust.com");

        cy.excuteAccessibilityTests();

        Logger.log("Checking the overview page");
        trustPage.viewOverview();

        trustOverviewPage
            .hasTotalAcademies("1")
            .hasAcademiesInEachAuthority("1 in Barnsley")
            .hasNumberOfPupils("3,052")
            .hasPupilCapacity("2,511", "122%")
            .hasOfstedRatingOutstanding("0")
            .hasOfstedRatingsGood("0")
            .hasOfstedRatingRequiresImprovement("1")
            .hasOfstedRatingInadequate("0")
            .hasOfstedRatingNotInspectedYet("0");

        cy.excuteAccessibilityTests();

        Logger.log("Checking the academies details");
        trustPage.viewAcademies();

        const academyName = "St. Mary's Academy";
        const academyUrn = "12836225";

        academiesDetailsTable
            .getRow(academyName)
            .then((row) => {
                row
                    .hasName(academyName)
                    .hasUrn(academyUrn)
                    .hasLocalAuthority("Barnsley")
                    .hasTypeOfEstablishment("Academy 16-19 Converter")
                    .hasUrbanOrRural("Urban minor conurbation");
            });

        cy.excuteAccessibilityTests();

        Logger.log("Checking the academies ofsted ratings");

        trustPage.viewAcademyOfstedRatings();

        academiesOfstedRatingsTable
            .getRow(academyName)
            .then((row) => {
                row
                    .hasName(academyName)
                    .hasUrn(academyUrn)
                    .hasDateJoined("18 Apr 2020")
                    .hasPreviousOfstedRating("Outstanding", "9 Jun 2018", "Before joining")
                    .hasCurrentOfstedRating("Requires improvement", "30 Dec 2019", "Before joining");
            });

        cy.excuteAccessibilityTests();

        Logger.log("Checking the academies pupil numbers");
        trustPage.viewAcademyPupilNumbers();

        academiesPupilNumbersTable
            .getRow(academyName)
            .then((row) => {
                row
                    .hasName(academyName)
                    .hasUrn(academyUrn)
                    .hasPhase("Not applicable")
                    .hasAgeRange("11 - 18")
                    .hasPupilNumbers("3,052")
                    .hasPupilCapacity("2,511")
                    .hasPercentageFull("122%");
            });

        cy.excuteAccessibilityTests();

        Logger.log("Checking free school meals");
        trustPage.viewFreeSchoolMeals();

        academiesFreeSchoolMealsTable
            .getRow(academyName)
            .then((row) => {
                row
                    .hasName(academyName)
                    .hasUrn(academyUrn)
                    .hasPupilsEligible("20.0%")
                    .hasLocalAuthorityAverage("27.8%")
                    .hasNationalAverage("22.7%");
            });
    });
});