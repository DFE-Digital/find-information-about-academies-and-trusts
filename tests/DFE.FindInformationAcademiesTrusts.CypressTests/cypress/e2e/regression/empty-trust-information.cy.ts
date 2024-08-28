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

describe("Checking the details of a trust that does not have any information", () => {
    beforeEach(() => {
        cy.login();
    });

    it("Should be able to find the trust and verify the empty information", () => {
        searchTrustPage
            .enterSearchText("THE EMPTY TRUST")
            .getOption("THE EMPTY TRUST")
            .then((option) => {
                option
                    .hasName("THE EMPTY TRUST")
                    .select();
            });

        searchTrustPage.search();

        trustPage
            .hasName("THE EMPTY TRUST")
            .hasType("Single-academy trust");

        trustDetailsPage
            .hasAddress("")
            .hasOpenedOn("")
            .hasRegionAndTerritory("")
            .hasUid("91313")
            .hasTrustReferenceNumber("TR3943")
            .hasUkprn("")
            .hasCompaniesHouseNumber("");

        Logger.log("Checking the contacts page");
        trustPage.viewContacts();

        trustContactsPage
            .hasEmptyContacts();

        Logger.log("Checking the overview page");
        trustPage.viewOverview();

        trustOverviewPage
            .hasTotalAcademies("0")
            .hasAcademiesInEachAuthority("")
            .hasNumberOfPupils("0")
            .hasPupilCapacity("0", "0")
            .hasOfstedRatingOutstanding("0")
            .hasOfstedRatingsGood("0")
            .hasOfstedRatingRequiresImprovement("0")
            .hasOfstedRatingInadequate("0")
            .hasOfstedRatingNotInspectedYet("0");

        Logger.log("Checking the academies details");
        trustPage.viewAcademies();

        academiesDetailsTable.hasNoRows();

        Logger.log("Checking the academies ofsted ratings");
        trustPage.viewAcademyOfstedRatings();

        academiesOfstedRatingsTable.hasNoRows();

        Logger.log("Checking the academies pupil numbers");
        trustPage.viewAcademyPupilNumbers();

        academiesPupilNumbersTable.hasNoRows();

        Logger.log("Checking free school meals");
        trustPage.viewFreeSchoolMeals();

        academiesFreeSchoolMealsTable.hasNoRows();
    });
});