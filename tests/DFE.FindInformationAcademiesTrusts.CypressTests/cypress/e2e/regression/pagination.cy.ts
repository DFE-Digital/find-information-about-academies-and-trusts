import commonPage from "../../pages/commonPage";
import navigation from "../../pages/navigation";
import paginationPage from "../../pages/paginationPage";

describe('Pagination Tests', () => {


    beforeEach(() => {
        cy.login();
        cy.visit('/search?keywords=west');
    });

    it('Should display multiple pagination buttons', () => {
        paginationPage.getTotalPaginationButtons().should('be.greaterThan', 1);
    });


    it('Should navigate to a specific page when a page number is clicked on a large result page', () => {

        cy.visit('/search?keywords=tru');
        paginationPage
            .clickPageNumber(2);
        navigation
            .checkCurrentURLIsCorrect('pagenumber=2');
        commonPage
            .checkThatBrowserTitleMatches('Search (page 2 of 71) - tru - Find information about academies and trusts');

        paginationPage
            .clickPageNumber(71);

        navigation
            .checkCurrentURLIsCorrect('pagenumber=71');
        commonPage
            .checkThatBrowserTitleMatches('Search (page 71 of 71) - tru - Find information about academies and trusts');

        paginationPage
            .clickPageNumber(70);

        navigation
            .checkCurrentURLIsCorrect('pagenumber=70');
        commonPage
            .checkThatBrowserTitleMatches('Search (page 70 of 71) - tru - Find information about academies and trusts');
    });

    it('Should navigate to the next page on next button click', () => {
        paginationPage
            .clickNext();

        navigation
            .checkCurrentURLIsCorrect('pagenumber=2');

        paginationPage
            .clickNext();

        navigation
            .checkCurrentURLIsCorrect('pagenumber=3');

    });

    it('Should navigate to the previous page on previous button click', () => {
        paginationPage
            .clickNext();

        navigation
            .checkCurrentURLIsCorrect('pagenumber=2');

        paginationPage
            .clickPrevious();

        navigation
            .checkCurrentURLIsCorrect('pagenumber=1');

    });

    it('Checks that the previous page button is not present on the first page of results', () => {
        paginationPage
            .checkPreviousButtonNotPresent();
    });

    it('Checks that the next page button is not present on the first page of results', () => {
        paginationPage
            .clickPageNumber(3)
            .checkNextButtonNotPresent();
    });

    it('Checks that the previous and next page buttons are not present on the no results found page', () => {
        cy.visit('/search?keywords=knowhere');

        paginationPage
            .checkPreviousButtonNotPresent()
            .checkNextButtonNotPresent();
    });

    it('Checks that I see the pages I would expect mid pagination and dont see the ones that should be hidden', () => {
        cy.visit('/search?keywords=tru&pagenumber=30');

        paginationPage
            .checkExpectedPageNumberInPaginationBar(1)
            .checkResultIsNotInPaginationBar(2)
            .checkResultIsNotInPaginationBar(28)
            .checkExpectedPageNumberInPaginationBar(29)
            .checkExpectedPageNumberInPaginationBar(30)
            .checkExpectedPageNumberInPaginationBar(31)
            .checkResultIsNotInPaginationBar(32)
            .checkResultIsNotInPaginationBar(70)
            .checkExpectedPageNumberInPaginationBar(71);
    });

    it('Checks that on a single result page only the page number is present', () => {
        cy.visit('/search?keywords=henley-in-arden');

        paginationPage
            .checkPreviousButtonNotPresent()
            .checkNextButtonNotPresent()
            .checkSingleResultOnlyHasOnePage(1);

        commonPage
            .checkThatBrowserTitleMatches('Search - henley-in-arden - Find information about academies and trusts');
    });

    it('Should navigate to the previous page on previous button click', () => {
        paginationPage
            .clickNext();

        navigation
            .checkCurrentURLIsCorrect('pagenumber=2');

        paginationPage.getResults().then(secondPageResults => {
            const secondPageFirstResultText = secondPageResults.first().text();

            paginationPage.clickPrevious();

            navigation
                .checkCurrentURLIsCorrect('pagenumber=1');

            paginationPage.getResults().first().should('not.have.text', secondPageFirstResultText);
        });
    });

    it('Should iterate through all pagination pages and verify results are different', () => {
        paginationPage.getTotalPaginationButtons().then(totalPages => {
            let previousFirstResultText = '';

            for (let page = 1; page <= totalPages; page++) {
                paginationPage.clickPageNumber(page);

                paginationPage.getResults().first().then(currentFirstResult => {
                    const currentFirstResultText = currentFirstResult.text();

                    if (page > 1) {
                        expect(currentFirstResultText).to.not.equal(previousFirstResultText);
                    }

                    previousFirstResultText = currentFirstResultText;
                });
            }
        });
    });
});
