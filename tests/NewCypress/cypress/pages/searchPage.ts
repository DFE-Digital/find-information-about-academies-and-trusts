class SearchPage {

    elements = {
        searchResults: () => cy.get('#main-content'),
        mainAutocomplete: () => cy.get('#home-search__listbox'),
        searchPageSearchBox: () => cy.get('#search'),
        searchPageSearchButton: () => cy.get('[data-testid="search"]'),
        resultsInfo: () => cy.get('#results-details'),
        resultItems: () => cy.get('.govuk-list li'),
        nextButton: () => cy.get('[data-testid="next-page"]'),
        hasNextButton: () => cy.$$('[data-testid="next-page"]').length > 0
    };

    public checkSearchResultsReturned(searchText: string): this {
        this.elements.searchResults().each(($el) => {
            cy.wrap($el).should('include.text', searchText);
        });
        return this;
    }

    public checkNoSearchResultsFound(): this {
        this.elements.searchResults().should('include.text', "0 results");
        return this;
    }

    public checkMainAutocompleteIsPresent(): this {
        this.elements.mainAutocomplete().should('be.visible');
        return this;
    }

    public checkAutocompleteContainsTypedText(searchText: string): this {
        this.elements.mainAutocomplete().should(($listbox) => {
            expect($listbox.children().length).to.be.greaterThan(0);
            const textFound = $listbox.children().toArray().some(item =>
                item.innerText.toLowerCase().includes(searchText.toLowerCase())
            );
            expect(textFound).to.be.true;
        });
        return this;
    }

    public enterSearchResultsSearchText(searchText: string): this {
        this.elements.searchPageSearchBox().type(searchText);
        return this;
    }

    public clicSearchPageSearchButton(): this {
        this.elements.searchPageSearchButton().click();
        return this;
    }

    public validateSearchResultsCountWithPagination(): this {
        const countResultsOnPage = (expectedTotalResults: number, accumulatedResults: number): void => {
            this.elements.resultItems().then(($items: JQuery<HTMLElement>) => {
                accumulatedResults += $items.length;
                cy.log(`Accumulated Results: ${accumulatedResults}`);

                if (this.elements.hasNextButton()) {
                    this.elements.nextButton().then(($next) => {
                        if ($next.is(':visible') && !$next.is(':disabled')) {
                            cy.wrap($next).click();
                            countResultsOnPage(expectedTotalResults, accumulatedResults);
                        } else {
                            assert.fail('Next button is invisible or disabled');
                        }
                    });
                } else {
                    expect(accumulatedResults).to.equal(expectedTotalResults);
                }
            });
        };

        this.elements.resultsInfo().invoke('text').then((text: string) => {
            const match = /(\d+)/.exec(text);
            if (match?.[0]) {
                const expectedTotalResults: number = parseInt(match[0], 10);
                let accumulatedResults: number = 0;
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
