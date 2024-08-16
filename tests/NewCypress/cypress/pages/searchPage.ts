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

    
}

const searchPage = new SearchPage();

export default searchPage;
