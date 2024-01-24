class SearchTrustPage {
    public enterSearchText(searchText: string): this {
        cy.getById("home-search").clear().type(searchText);
        return this;
    }

    public getOption(value: string): Cypress.Chainable<AutoCompleteSearchOption> {

        cy.getById("home-search__listbox")
            .contains("li", value)
            .as("targetedOption");

        return cy.get("@targetedOption")
            .then((el) => {
                return new AutoCompleteSearchOption(el);
            });
    }

    public selectFirstResult(): Cypress.Chainable<TrustSearchResult> {
        cy.getByTestId("trust-result")
            .first()
            .as("targetedResult");

        return cy.get("@targetedResult")
            .then((el) => {
                return new TrustSearchResult(el);
            });
    }

    public hasNumberOfResults(value: string): this {
        cy.getById("results-details").should("contain.text", value);

        return this;
    }

    public hasResults(): this {
        cy.getByTestId("trust-result").should("have.length.above", 0);

        return this;
    }

    public getSearchResult(value: string): Cypress.Chainable<TrustSearchResult> {
        cy.getByTestId("trust-result")
            .contains("a", value)
            .parent()
            .as("targetedResult");

        return cy.get("@targetedResult")
            .then((el) => {
                return new TrustSearchResult(el);
            });
    }

    public getAllTrustResults(): Cypress.Chainable<Array<string>> {

        const result: Array<string> = [];

        return cy.getByTestId("trust-name")
            .each($el => {
                var text = $el.text();
                result.push(text);
            })
            .then(() => {
                return result;
            })
    }

    public search(): this {
        cy.getByTestId("search").click();
        return this;
    }
}

class TrustSearchResult {
    constructor(private element: JQuery<Element>) {
    }

    public getName(): string {
        return this.element.find("[data-testid='trust-name']").text();
    }

    public hasName(text: string): this {
        this.containsText("trust-name", text);
        return this;
    }

    public hasAddress(text: string): this {
        this.containsText("address", text);
        return this;
    }

    public hasTrn(text: string): this {
        this.containsText("trn", text);
        return this;
    }

    public hasUid(text: string): this {
        this.containsText("uid", text);
        return this;
    }

    public select(): this {
        cy.wrap(this.element)
            .within(() => {
                cy.getByTestId("trust-name").click();
            });

        return this;
    }

    private containsText(id: string, value: string) {
        cy.wrap(this.element)
            .within(() => {
                cy.getByTestId(id).should("contain.text", value);
            });

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