class HomePage {

    public enterMainSearchText(searchText: string): this {
        const getMainSearchBox = () => cy.get('#home-search');
        getMainSearchBox().type(searchText);

        return this;
    }

    public clickMainSearchButton(): this {
        const mainSearchButton = () => cy.get('[data-testid="search"]');

        mainSearchButton().click();

        return this;
    }

    public searchButtonPresent(): this {
        const mainSearchButton = () => cy.get('[data-testid="search"]');

        mainSearchButton().should('be.visible');
        mainSearchButton().should('be.enabled');
        return this;
    }

    public autocompleteIsPresent(): this {
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
}

const homePage = new HomePage();

export default homePage;
