class HeaderPage {

    elements = {
        headerSearchButton: () => cy.get('.dfe-search__submit'),
        mainSearchBox: () => cy.get('#header-search'),
        headerAutocomplete: () => cy.get('#header-search__listbox')
    };

    public clickHeaderSearchButton(): this {
        this.elements.headerSearchButton().click();
        return this;
    }

    public checkHeaderSearchButtonPresent(): this {
        this.elements.headerSearchButton().should('be.visible').should('be.enabled');
        return this;
    }

    public checkHeaderAutocompleteIsPresent(): this {
        this.elements.headerAutocomplete().should('be.visible');
        return this;
    }

    public checkAutocompleteContainsTypedText(searchText: string): this {
        this.elements.headerAutocomplete().should(($listbox) => {
            expect($listbox.children().length).to.be.greaterThan(0);
            const textFound = $listbox.children().toArray().some(item =>
                item.innerText.toLowerCase().includes(searchText.toLowerCase())
            );
            expect(textFound).to.be.true;
        });
        return this;
    }

    public enterHeaderSearchText(searchText: string): this {
        this.elements.mainSearchBox().type(searchText);
        return this;
    }

}

const headerPage = new HeaderPage();
export default headerPage;
