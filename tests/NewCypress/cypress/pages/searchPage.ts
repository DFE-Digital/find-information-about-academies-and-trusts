class SearchPage {

    public checkSearchResultsReturned(searchText: string): this {
        const getSearchResults = () => cy.get('#main-content');

        getSearchResults().each(($el) => {
            cy.wrap($el).should('include.text', searchText);
        });

        return this;
    }

    public checkNoSearchResultsFound(): this {
        const noSearchResultBAnner = () => cy.get('#main-content');

        noSearchResultBAnner().should('include.text', "0 results")

        return this;
    }

    public checkMainAutocompleteIsPresent(): this {
        cy.get('#home-search__listbox').should('be.visible');

        return this;
    }

    public checkAutocompleteContainsTypedText(searchText: string): this {
        cy.get('#home-search__listbox').should(($listbox) => {
            // Ensure there are items present in the listbox
            expect($listbox.children().length).to.be.greaterThan(0);

            // Ensure at least one item contains the typed text (case insensitive)
            const textFound = $listbox.children().toArray().some(item =>
                item.innerText.toLowerCase().includes(searchText.toLowerCase())
            );
            expect(textFound).to.be.true;
        });
        return this;
    }

    public enterSearchResultsSearchText(searchText: string): this {
        const getSearchPageSearchBox = () => cy.get('#search');
        getSearchPageSearchBox().type(searchText);

        return this;
    }

    public clicSearchPageSearchButton(): this {
        const headerSearchButton = () => cy.get('[data-testid="search"]');

        headerSearchButton().click();

        return this;
    }

    public validateSearchResultsCountWithPagination(): this {
        const getResultsInfo = () => cy.get('#results-details');
        const getResultItems = () => cy.get('.govuk-list li');
        const getNextButton = () => cy.get('[data-testid="next-page"]');
        const hasNextButton = () => cy.$$('[data-testid="next-page"]').length > 0;

        // Function to count results on each page
        const countResultsOnPage = (expectedTotalResults: number, accumulatedResults: number): void => {
            getResultItems().then(($items: JQuery<HTMLElement>) => {
                accumulatedResults += $items.length;
                cy.log(`Accumulated Results: ${accumulatedResults}`);

                if (hasNextButton()) {
                    getNextButton().then(($next) => {
                        // Is the next button enabled?
                        if ($next.is(':visible') && !$next.is(':disabled')) {
                            // Click "Next" to load more results
                            cy.wrap($next).click();
                            countResultsOnPage(expectedTotalResults, accumulatedResults); // Recursively count results on the next page
                        } else {
                            assert.fail('Next button is invisible or disabled')
                        }
                    });
                } else {
                    // No more pages, validate the accumulated results
                    cy.log(`Final Accumulated Results: ${accumulatedResults}, Expected Total Results: ${expectedTotalResults}`);
                    expect(accumulatedResults).to.equal(expectedTotalResults);
                }
            });
        };

        // Get the total number of results displayed in the summary
        getResultsInfo()
            .invoke('text')
            .then((text: string) => {
                const match = /(\d+)/.exec(text); // Extract the total number of results from the text
                if (match?.[0]) {
                    const expectedTotalResults: number = parseInt(match[0], 10); // Convert to an integer
                    let accumulatedResults: number = 0;

                    // Start counting results from the first page
                    countResultsOnPage(expectedTotalResults, accumulatedResults);
                } else {
                    throw new Error('Could not extract the total number of results.');
                }
            });

        return this;
    }

}

const searchPage = new SearchPage();

export default searchPage;