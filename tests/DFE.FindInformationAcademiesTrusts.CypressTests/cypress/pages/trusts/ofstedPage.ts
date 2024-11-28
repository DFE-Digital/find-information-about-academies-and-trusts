import { TableUtility } from "../tableUtility";

class OfstedPage {
    elements = {
        currentRatings: {
            Section: () => cy.get('[aria-describedby="ofsted-ratings-link"]'),
            SchoolName: () => this.elements.currentRatings.Section().find('[data-testid="school-name"]'),
            SchoolNameHeader: () => this.elements.currentRatings.Section().find("th:contains('School name')"),
            DateJoined: () => this.elements.currentRatings.Section().find('[data-testid="date-joined"]'),
            DateJoinedHeader: () => this.elements.currentRatings.Section().find("th:contains('Date joined')"),
            PreviousOfstedRating: () => this.elements.currentRatings.Section().find('[data-testid="previous-ofsted-rating"]'),
            PreviousOfstedRatingHeader: () => this.elements.currentRatings.Section().find("th:contains('Previous Ofsted rating')"),
            CurrentOfstedRating: () => this.elements.currentRatings.Section().find('[data-testid="current-ofsted-rating"]'),
            CurrentOfstedRatingHeader: () => this.elements.currentRatings.Section().find("th:contains('Current Ofsted rating')"),
            NoDataMessage: () => this.elements.currentRatings.Section().contains('No data available')
        }
    };

    public checkOfstedHeadersPresent(): this {
        this.elements.currentRatings.SchoolNameHeader().should('be.visible');
        this.elements.currentRatings.DateJoinedHeader().should('be.visible');
        this.elements.currentRatings.PreviousOfstedRatingHeader().should('be.visible');
        this.elements.currentRatings.CurrentOfstedRatingHeader().should('be.visible');
        return this;
    }

    public checkOfstedSorting(): this {
        TableUtility.checkStringSorting(
            this.elements.currentRatings.SchoolName,
            this.elements.currentRatings.SchoolNameHeader
        );
        TableUtility.checkStringSorting(
            this.elements.currentRatings.DateJoined,
            this.elements.currentRatings.DateJoinedHeader
        );
        TableUtility.checkStringSorting(
            this.elements.currentRatings.PreviousOfstedRating,
            this.elements.currentRatings.PreviousOfstedRatingHeader
        );
        TableUtility.checkStringSorting(
            this.elements.currentRatings.CurrentOfstedRating,
            this.elements.currentRatings.CurrentOfstedRatingHeader
        );
        return this;
    }

    public checkPreviousOfstedTypesOnOfstedTable(): this {
        this.elements.currentRatings.PreviousOfstedRating().each((element) => {
            const text = element.text().trim();
            expect(text).to.match(/Good|Inadequate|Not yet inspected|Outstanding|Requires improvement/);
        });
        return this;
    }

    public checkCurrentOfstedTypesOnOfstedTable(): this {
        this.elements.currentRatings.CurrentOfstedRating().each((element) => {
            const text = element.text().trim();
            expect(text).to.match(/Good|Inadequate|Not yet inspected|Outstanding|Requires improvement/);
        });
        return this;
    }

    public checkNoDataMessageIsVisible(): this {
        this.elements.currentRatings.NoDataMessage().should('be.visible');
        return this;
    }
}

const ofstedPage = new OfstedPage();
export default ofstedPage;

