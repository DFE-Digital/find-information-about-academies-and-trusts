class SearchTrustPage {
    public enterSearchText(searchText: string): this {
        cy.getById("home-search").clear().type(searchText);
        return this;
    }

    public withOption(value: string): Cypress.Chainable<AutoCompleteSearchOption> {

        cy.getById("home-search__listbox")
            .contains("li", value)
            .as("targetedOption");

        return cy.get("@targetedOption")
            .then((el) => {
                return new AutoCompleteSearchOption(el);
            });
    }

    public search(): this {
        cy.getByTestId("search").click();
        return this;
    }
}

class AutoCompleteSearchOption {
    constructor(private element: JQuery<Element>) {
    }

    public hasName(text: string): this {
        cy.wrap(this.element).contains(text);
        return this;
    }

    public hasHint(text: string): this {
        cy.wrap(this.element).contains(text);
        return this;
    }

    public select(): this {
        cy.wrap(this.element).click();
        return this;
    }
}

const searchTrustPage = new SearchTrustPage();

export default searchTrustPage;