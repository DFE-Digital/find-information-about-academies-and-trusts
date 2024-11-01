class HomePage {

    elements = {
        mainSearchBox: () => cy.get('#home-search'),
        mainSearchButton: () => cy.get('[data-testid="search"]'),

        whatYouCanFindList: {
            mainBox: () => cy.get('.govuk-details'),
            list: () => cy.get('.govuk-list'),
            button: () => cy.get('.govuk-details__summary'),
            addressAndContactDetais: () => this.elements.whatYouCanFindList.list().contains('address and contact details'),
            pupilNumbersAndCapacities: () => this.elements.whatYouCanFindList.list().contains('pupil numbers and capacities'),
            ofstedRatings: () => this.elements.whatYouCanFindList.list().contains('Ofsted ratings'),
            freeSchoolMealPercentages: () => this.elements.whatYouCanFindList.list().contains('free school meal percentages'),
            trustGovernanceDetails: () => this.elements.whatYouCanFindList.list().contains('trust governance details'),
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

    public clickWhatYouCanFindList(): this {
        this.elements.whatYouCanFindList.button().click()
        return this;
    }

    public checkWhatYouCanFindPresent(): this {
        this.elements.whatYouCanFindList.mainBox().should('be.visible')
        return this;
    }

    public checkWhatYouCanFindListCollapsed(): this {
        this.elements.whatYouCanFindList.mainBox().should('not.have.attr', 'open')
        return this;
    }

    public checkWhatYouCanFindListOpen(): this {
        this.elements.whatYouCanFindList.mainBox().should('have.attr', 'open')
        return this;
    }

    public checkWhatYouCanFindListItemsPresent(): this {
        this.elements.whatYouCanFindList.addressAndContactDetais().should('be.visible')
        this.elements.whatYouCanFindList.pupilNumbersAndCapacities().should('be.visible')
        this.elements.whatYouCanFindList.ofstedRatings().should('be.visible')
        this.elements.whatYouCanFindList.freeSchoolMealPercentages().should('be.visible')
        this.elements.whatYouCanFindList.trustGovernanceDetails().should('be.visible')
        return this;
    }
}

const homePage = new HomePage();
export default homePage;
