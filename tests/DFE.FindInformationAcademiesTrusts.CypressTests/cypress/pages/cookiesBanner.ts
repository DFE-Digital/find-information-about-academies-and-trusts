class CookiesBanner {
    public accept(): this {
        cy.getByTestId("banner-accept-analytics-cookies-button").click();

        return this;
    }

    public reject(): this {
        cy.getByTestId("banner-reject-analytics-cookies-button").click();

        return this;
    }

    public viewCookies(): this {

        cy.getByTestId("banner-view-cookies").click();

        return this;
    }

    public isNotVisible(): this {
        cy.getByTestId("cookies-banner").should("not.exist");

        return this;
    }
}

const cookiesBanner = new CookiesBanner();

export default cookiesBanner;