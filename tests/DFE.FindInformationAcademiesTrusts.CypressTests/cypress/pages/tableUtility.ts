export class TableUtility {

    private static getTodayDateWithoutTime() {
        const today = new Date();
        today.setHours(0, 0, 0, 0);
        return today;
    }

    public static checkCellDateIsBeforeTodayOrHasNoData(cellElement: JQuery<HTMLElement>) {
        const cellContent = cellElement.text().trim();
        const cellDate = new Date(cellContent);

        // If cell is a valid date
        if (cellDate.valueOf()) {
            expect(cellDate).to.be.lessThan(this.getTodayDateWithoutTime(), `Date "${cellContent}" should be before today`);
            return;
        }

        // Otherwise
        expect(cellContent).to.equal("No Data", "Cell should have no data");
    }

    public static checkCellDateIsOnOrAfterTodayOrHasNoData(cellElement: JQuery<HTMLElement>) {
        const cellContent = cellElement.text().trim();
        const cellDate = new Date(cellContent);

        // If cell is a valid date
        if (cellDate.valueOf()) {
            expect(cellDate).to.be.at.least(this.getTodayDateWithoutTime(), `Date "${cellContent}" should be on or after today`);
            return;
        }

        // Otherwise
        expect(cellContent).to.equal("No Data", "Cell should have no data");
    }

    public static checkStringSorting(
        elements: () => Cypress.Chainable<JQuery<HTMLElement>>,
        header: () => Cypress.Chainable<JQuery<HTMLElement>>,
    ) {
        const headerButton = () => header().find('button');
        header().invoke("attr", "aria-sort").then(value => {
            if (value === "descending" || value === "none" || !value) {
                headerButton().click();
            }

            header().should("have.attr", "aria-sort", "ascending");
            const actualAscElements: { value: string, sortValue: string; }[] = [];
            elements().each($elements => {
                actualAscElements.push({
                    value: $elements.text().trim(),
                    sortValue: $elements.attr("data-sort-value") ?? ""
                });
            }).then(() => {
                const ascendingValues = actualAscElements.toSorted((a, b) => a.sortValue.localeCompare(b.sortValue));
                expect(actualAscElements, "Values are sorted").to.deep.equal(ascendingValues);
            });

            headerButton().click();
            header().should("have.attr", "aria-sort", "descending");
            const actualDscElements: { value: string, sortValue: string; }[] = [];
            elements().each($elements => {
                actualDscElements.push({
                    value: $elements.text().trim(),
                    sortValue: $elements.attr("data-sort-value") ?? ""
                });
            }).then(() => {
                const descendingValues = actualDscElements.toSorted((a, b) => b.sortValue.localeCompare(a.sortValue));
                expect(actualDscElements, "Values are sorted").to.deep.equal(descendingValues);
            });
        });
    }

    public static checkNumericSorting(
        elements: () => Cypress.Chainable<JQuery<HTMLElement>>,
        header: () => Cypress.Chainable<JQuery<HTMLElement>>,
    ) {
        const headerButton = () => header().find('button');
        header().invoke("attr", "aria-sort").then(value => {
            if (value === "descending" || value === "none" || !value) {
                headerButton().click();
            }

            header().should("have.attr", "aria-sort", "ascending");
            const actualAscElements: { value: string, sortValue: number; }[] = [];
            elements().each($elements => {
                actualAscElements.push({
                    value: $elements.text().trim(),
                    sortValue: Number($elements.attr("data-sort-value") ?? "0")
                });
            }).then(() => {
                const ascendingValues = actualAscElements.toSorted((a, b) => a.sortValue - b.sortValue);
                expect(actualAscElements, "Values are sorted").to.deep.equal(ascendingValues);
            });

            headerButton().click();
            header().should("have.attr", "aria-sort", "descending");
            const actualDscElements: { value: string, sortValue: number; }[] = [];
            elements().each($elements => {
                actualDscElements.push({
                    value: $elements.text().trim(),
                    sortValue: Number($elements.attr("data-sort-value") ?? "0")
                });
            }).then(() => {
                const descendingValues = actualDscElements.toSorted((a, b) => b.sortValue - a.sortValue);
                expect(actualDscElements, "Values are sorted").to.deep.equal(descendingValues);
            });
        });
    }
}
