import { Logger } from "cypress/common/logger";
import academiesDetailsTable from "cypress/pages/academiesDetailsTable";
import academiesFreeSchoolMealsTable from "cypress/pages/academiesFreeSchoolMealsTable";
import academiesOfstedRatingsTable from "cypress/pages/academiesOfstedRatingsTable";
import academiesPupilNumbersTable from "cypress/pages/academiesPupilNumbersTable";
import searchTrustPage from "cypress/pages/searchTrustPage";
import trustOverviewPage from "cypress/pages/trustOverviewPage";
import trustPage from "cypress/pages/trustPage";

describe("Testing a multi-academy trust", () => {
    beforeEach(() => {
        cy.login();
    });

    it("Should display a trust with many academies", () => {
        searchTrustPage
            .enterSearchText("PROSPERITY EDUCATION TRUST")
            .getOption("PROSPERITY EDUCATION TRUST")
            .then((option) => {
                option.select();
            });

        searchTrustPage.search();

        trustPage
            .hasName("PROSPERITY EDUCATION TRUST")
            .hasType("Multi-academy trust");

        Logger.log("Checking the overview page");
        trustPage.viewOverview();

        trustOverviewPage
            .hasTotalAcademies("11")
            .hasAcademiesInEachAuthority("4 in Barnsley")
            .hasAcademiesInEachAuthority("4 in Rotherham")
            .hasAcademiesInEachAuthority("3 in South Tyneside")
            .hasNumberOfPupils("12,144")
            .hasPupilCapacity("15,061", "81%")
            .hasOfstedRatingOutstanding("2")
            .hasOfstedRatingsGood("1")
            .hasOfstedRatingRequiresImprovement("3")
            .hasOfstedRatingInadequate("3")
            .hasOfstedRatingNotInspectedYet("2");

        // Check a selection of the academies, since there are so many
        Logger.log("Checking some of the academies details in the trust");
        assertGeorgeAbbeySchool();
        assertQueensbridgeSchool();
    });

    function assertGeorgeAbbeySchool() {

        const georgeAbbeySchool = "George Abbey Middle Deemed Primary School"

        Logger.log(`Checking the academies details for ${georgeAbbeySchool}`);
        trustPage.viewAcademies();

        academiesDetailsTable
            .getRow(georgeAbbeySchool)
            .then((row) => {
                row
                    .hasName(georgeAbbeySchool)
                    .hasUrn("12729225")
                    .hasLocalAuthority("Barnsley")
                    .hasTypeOfEstablishment("Voluntary Controlled School")
                    .hasUrbanOrRural("Rural village in a sparse setting");
            });

        Logger.log("Checking the academies ofsted ratings");
        trustPage.viewAcademyOfstedRatings();

        academiesOfstedRatingsTable
            .getRow(georgeAbbeySchool)
            .then(row => {
                row
                    .hasDateJoined("12 Apr 2022")
                    .hasPreviousOfstedRating("Requires improvement", "8 May 2022", "After joining")
                    .hasCurrentOfstedRating("Good", "23 Mar 2023", "After joining");
            });

        Logger.log("Checking the academies pupil numbers");
        trustPage.viewAcademyPupilNumbers();

        academiesPupilNumbersTable
            .getRow(georgeAbbeySchool)
            .then((row) => {
                row
                    .hasPhase("Middle Deemed Primary")
                    .hasAgeRange("11 - 18")
                    .hasPupilNumbers("2,074")
                    .hasPupilCapacity("1,704")
                    .hasPercentageFull("122%");
            });

        Logger.log("Checking free school meals");
        trustPage.viewFreeSchoolMeals();

        academiesFreeSchoolMealsTable
            .getRow(georgeAbbeySchool)
            .then((row) => {
                row
                    .hasPupilsEligible("5.0%")
                    .hasLocalAuthorityAverage("29.4%")
                    .hasNationalAverage("24.9%");
            });
    }

    function assertQueensbridgeSchool() {
        const queensbridgeSchool = "Queensbridge Middle Deemed Primary Academy";

        Logger.log(`Checking the academies details for ${queensbridgeSchool}`);
        trustPage.viewAcademies();

        academiesDetailsTable
            .getRow(queensbridgeSchool)
            .then((row) => {
                row
                    .hasName(queensbridgeSchool)
                    .hasUrn("12727489")
                    .hasLocalAuthority("Rotherham")
                    .hasTypeOfEstablishment("Community School")
                    .hasUrbanOrRural("Urban major conurbation");
            });

        Logger.log("Checking the academies ofsted ratings");
        trustPage.viewAcademyOfstedRatings();

        academiesOfstedRatingsTable
            .getRow(queensbridgeSchool)
            .then(row => {
                row
                    .hasDateJoined("14 Jan 2022")
                    .hasPreviousOfstedRating("Not yet inspected", "", "")
                    .hasCurrentOfstedRating("Outstanding", "18 May 2022", "After joining");
            });

        Logger.log("Checking the academies pupil numbers");
        trustPage.viewAcademyPupilNumbers();

        academiesPupilNumbersTable
            .getRow(queensbridgeSchool)
            .then((row) => {
                row
                    .hasPhase("Middle Deemed Primary")
                    .hasAgeRange("11 - 18")
                    .hasPupilNumbers("745")
                    .hasPupilCapacity("1,690")
                    .hasPercentageFull("44%");
            });

        Logger.log("Checking free school meals");
        trustPage.viewFreeSchoolMeals();

        academiesFreeSchoolMealsTable
            .getRow(queensbridgeSchool)
            .then((row) => {
                row
                    .hasPupilsEligible("29.0%")
                    .hasLocalAuthorityAverage("27.6%")
                    .hasNationalAverage("24.9%");
            });
    }
});