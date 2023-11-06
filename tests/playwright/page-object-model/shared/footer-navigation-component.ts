import { Locator, Page, expect } from '@playwright/test'

export class FooterNavigationComponent {
    readonly expect: FooterNavigationComponentAssertions
    readonly locator: Locator
    readonly privacyPolicyLinkLocator: Locator
    constructor(readonly page: Page) {
        this.expect = new FooterNavigationComponentAssertions(this)
        this.privacyPolicyLinkLocator = page.getByTestId('privacy-policy-link')
    }

    async clickPrivacyPolicy () {
        await this.privacyPolicyLinkLocator.click()
    }
}
    class FooterNavigationComponentAssertions {
        constructor(readonly footerNavigation: FooterNavigationComponent) {
        }
        async isVisible(): Promise<void> {
            await expect(this.footerNavigation.locator).toContainText('All content is available under the Open Government Licence v3.0, except where otherwise stated')
        }
    }