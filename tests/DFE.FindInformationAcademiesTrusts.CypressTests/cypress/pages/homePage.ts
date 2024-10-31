class HomePage {

    elements = {
        mainSearchBox: () => cy.get('#home-search'),
        mainSearchButton: () => cy.get('[data-testid="search"]'),
        //whatYouCanFindList: () => cy.get('[data-testid="what-you-can-find-list"]'),
        whatYouCanFindList: {
            mainBox: () => cy.get('.govuk-details'),
            list: () => cy.get('.govuk-list'),
            addressAndContactDetais: () => this.elements.whatYouCanFindList.list().contains('address and contact details'),

        }
    };

    public enterMainSearchText(searchText: string): this {
        this.elements.mainSearchBox().type(searchText);
        return this;
    }

    public clickMainSearchButton(): this {
        this.elements.mainSearchButton().click();
        return this;
    }

    public checkMainSearchButtonPresent(): this {
        this.elements.mainSearchButton().should('be.visible').should('be.enabled');
        return this;
    }

    public checkWhatYouCanFindPresent(): this {
        this.elements.whatYouCanFindList.mainBox().should('be.visible')
        return this;
    }

    public checkWhatYouCanFindListCollapsed(): this {
        this.elements.whatYouCanFindList.addressAndContactDetais().should('not.be.visible')
        return this;
    }



}

const homePage = new HomePage();
export default homePage;
