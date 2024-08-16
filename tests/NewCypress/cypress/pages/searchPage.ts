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

    public mainAutocompleteIsPresent(): this {
        cy.get('#home-search__listbox').should('be.visible');

        return this;
    }

    public autocompleteContainsTypedText(searchText: string): this {
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
    
        // Step 1: Get the total number of results displayed in the summary
        getResultsInfo()
            .invoke('text')
            .then((text: string) => {
                const match = text.match(/(\d+)/); // Extract the total number of results from the text
                if (match && match[0]) {
                    const totalResults: number = parseInt(match[0], 10); // Convert to an integer
                    let accumulatedResults: number = 0;

                    // Step 2: Function to count results on each page
                    const countResultsOnPage = (): void => {
                        getResultItems().then(($items: JQuery<HTMLElement>) => {
                            accumulatedResults += $items.length;
                            cy.log(`Accumulated Results: ${accumulatedResults}`);

                            if (hasNextButton()) {
                                getNextButton().then(($next) => {
                                    // Is the next button enabled?
                                    if ($next.is(':visible') && !$next.is(':disabled')) {
                                        // Click "Next" to load more results
                                        cy.wrap($next).click();
                                        getResultsInfo().should('be.visible'); //dynamic wait to ensure each page is loaded before continue
                                        countResultsOnPage(); // Recursively count results on the next page
                                    } else {
                                        assert.fail('Next button is invisible or disabled')
                                    }
                                });
                            } else {
                                // No more pages, validate the accumulated results
                                cy.log(`Final Accumulated Results: ${accumulatedResults}, Expected Total Results: ${totalResults}`);
                                expect(accumulatedResults).to.equal(totalResults);
                            }
                        });
                    };

                    // Start counting results from the first page
                    countResultsOnPage();
                } else {
                    throw new Error('Could not extract the total number of results.');
                }
            });

        return this;
    }


}

const searchPage = new SearchPage();

export default searchPage;
