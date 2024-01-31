class FooterLinks {
    public viewCookies(): this {
        cy.getByTestId("cookies-footer-link").click();

        return this;
    }
}

const footerLinks = new FooterLinks();

export default footerLinks;