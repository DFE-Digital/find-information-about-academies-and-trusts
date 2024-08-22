class HeaderPage {

    public clickHeaderSearchButton(): this {
        const headerSearchButton = () => cy.get('.dfe-search__submit');

        headerSearchButton().click();

        return this;
    }

    public checkHeaderSearchButtonPresent(): this {
        const headerSearchButton = () => cy.get('.dfe-search__submit');

        headerSearchButton().should('be.visible');
        headerSearchButton().should('be.enabled');
        return this;
    }

    public checkHeaderAutocompleteIsPresent(): this {
        cy.get('#header-search__listbox').should('be.visible');

        return this;
    }

    public checkAutocompleteContainsTypedText(searchText: string): this {
        cy.get('#header-search__listbox').should(($listbox) => {
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

    public enterHeaderSearchText(searchText: string): this {
        const getMainSearchBox = () => cy.get('#header-search');
        getMainSearchBox().type(searchText);

        return this;
    }
}

const headerPage = new HeaderPage();

export default headerPage;
