import paginationPage from "../../pages/paginationPage";

describe('Pagination Tests', () => {


    beforeEach(() => {
        cy.login()
        cy.visit('/search?keywords=west')
    });

    it('Should display multiple pagination buttons', () => {
        paginationPage.getTotalPaginationButtons().should('be.greaterThan', 1);
    });


    it('Should navigate to a specific page when a page number is clicked on a large result page', () => {

        cy.visit('/search?keywords=tru')
        paginationPage
            .clickPageNumber(2)
            .checkCurrentURLIsCorrect('pagenumber=2')

            .clickPageNumber(73)
            .checkCurrentURLIsCorrect('pagenumber=73')

            .clickPageNumber(72)
            .checkCurrentURLIsCorrect('pagenumber=72')
    });

    it('Should navigate to the next page on next button click', () => {
        paginationPage.getResults().then(firstPageFirstResult => {
            const firstPageFirstResultText = firstPageFirstResult.first().text();

            paginationPage
                .clickNext()
                .checkCurrentURLIsCorrect('pagenumber=2')

            paginationPage
                .getResults()
                .first()
                .should('not.have.text', firstPageFirstResultText);

            paginationPage
                .clickNext()
                .checkCurrentURLIsCorrect('pagenumber=3')
        });
    });

    it('Checks that the previous page button is not present on the first page of results', () => {
        paginationPage
            .checkPreviousButtonNotPresent()
    });

    it('Checks that the next page button is not present on the first page of results', () => {
        paginationPage
            .clickPageNumber(3)
            .checkNextButtonNotPresent()
    });

    it('Checks that the previous and next page buttons are not present on the no results found page', () => {
        cy.visit('/search?keywords=knowhere')

        paginationPage
            .checkPreviousButtonNotPresent()
            .checkNextButtonNotPresent()
    });

    it('Checks that I see the pages I would expect mid pagination and dont see the ones that should be hidden', () => {
        cy.visit('/search?keywords=tru&pagenumber=30')

        paginationPage
            .checkExpectedPageNumberInPaginationBar(1)
            .checkResultIsNotInPaginationBar(2)
            .checkResultIsNotInPaginationBar(28)
            .checkExpectedPageNumberInPaginationBar(29)
            .checkExpectedPageNumberInPaginationBar(30)
            .checkExpectedPageNumberInPaginationBar(31)
            .checkResultIsNotInPaginationBar(32)
            .checkResultIsNotInPaginationBar(72)
            .checkExpectedPageNumberInPaginationBar(73)
    });

    it('Checks that on a single result page only the page number is present', () => {
        cy.visit('/search?keywords=henley-in-arden')

        paginationPage
            .checkPreviousButtonNotPresent()
            .checkNextButtonNotPresent()
            .checkSingleResultOnlyHasOnePage(1)
    });

    it('Should navigate to the previous page on previous button click', () => {
        paginationPage
            .clickNext()
            .checkCurrentURLIsCorrect('pagenumber=2')

        paginationPage.getResults().then(secondPageResults => {
            const secondPageFirstResultText = secondPageResults.first().text();

            paginationPage.clickPrevious()
                .checkCurrentURLIsCorrect('pagenumber=1')

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
