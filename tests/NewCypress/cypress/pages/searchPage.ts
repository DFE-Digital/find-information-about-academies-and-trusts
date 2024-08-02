class SearchPage {

    public checkSearchResultsReturned(searchText: string): this {
        const getSearchResults = () => cy.get('#main-content');

        getSearchResults().each(($el) => {
            cy.wrap($el).should('include.text', searchText);
        });

        return this;
    }
}

const searchPage = new SearchPage();

export default searchPage;
