import commonPage from "../../pages/commonPage";
import navigation from "../../pages/navigation";

describe("Testing the data sources component", () => {

    beforeEach(() => {
        cy.login();
    });

    describe("Content pages", () => {
        ['/', '/search', '/accessibility', '/cookies', '/privacy', '/error'].forEach((url) => {
            it(`Should not have a data sources component on ${url}`, () => {
                cy.visit(url); // don't turn off fail on status code because we want the test to fail if visit returns 404 as that means our test urls are incorrect

                commonPage
                    .checkPageContentHasLoaded() // We need to make sure the page content has finished loading before we check that something is *not* on the page
                    .checkDoesNotHaveDataSourcesComponent();
            });
        });

        it(`Should not have a data sources component on /notfound`, () => {
            cy.visit('/notfound', { failOnStatusCode: false });

            commonPage
                .checkPageContentHasLoaded() // We need to make sure the page content has finished loading before we check that something is *not* on the page
                .checkDoesNotHaveDataSourcesComponent();
        });
    });

    describe("Trust pages", () => {
        const uid = 5712;
        const trustPages = [
            {
                pageName: "Overview",
                subpages: [
                    { subpageName: "Trust details", url: `/trusts/overview/trust-details?uid=${uid}` },
                    { subpageName: "Trust summary", url: `/trusts/overview/trust-summary?uid=${uid}` },
                    { subpageName: "Reference numbers", url: `/trusts/overview/reference-numbers?uid=${uid}` },
                ]
            },
            {
                pageName: "Contacts",
                subpages: [
                    { subpageName: "In DfE", url: `/trusts/contacts/in-dfe?uid=${uid}` },
                    { subpageName: "In the trust", url: `/trusts/contacts/in-the-trust?uid=${uid}` },
                ]
            },
            {
                pageName: "Ofsted",
                subpages: [
                    { subpageName: "Current ratings", url: `/trusts/ofsted/current-ratings?uid=${uid}` },
                    { subpageName: "Previous ratings", url: `/trusts/ofsted/previous-ratings?uid=${uid}` },
                    { subpageName: "Important dates", url: `/trusts/ofsted/important-dates?uid=${uid}` },
                    { subpageName: "Safeguarding and concerns", url: `/trusts/ofsted/safeguarding-and-concerns?uid=${uid}` }
                ]
            },
            {
                pageName: "Governance",
                subpages: [
                    { subpageName: `Trust leadership`, url: `/trusts/governance/trust-leadership?uid=${uid}` },
                    { subpageName: "Trustees", url: `/trusts/governance/trustees?uid=${uid}` },
                    { subpageName: "Members", url: `/trusts/governance/members?uid=${uid}` },
                    { subpageName: "Historic members", url: `/trusts/governance/historic-members?uid=${uid}` }
                ]
            },
        ];
        trustPages.forEach(({ pageName, subpages }) => {
            const subpageNames = subpages.map(s => s.subpageName);

            describe(pageName, () => {

                subpages.forEach(({ subpageName, url }) => {
                    it(`Should have a data sources component on ${pageName} > ${subpageName}`, () => {
                        // Go to the given subpage
                        cy.visit(url);

                        // Check that the given subpage list is up to date (so we don't miss any if new pages are added)
                        navigation.checkSubpageNavMatches(subpages);

                        // Check that the data sources component has a subheading for each subnav
                        commonPage
                            .checkHasDataSourcesComponent()
                            .checkDataSourcesComponentHasSubpageHeadings(subpageNames);
                    });
                });
            });
        });
    });
});
