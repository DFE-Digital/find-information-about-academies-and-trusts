import { Locator, Page, expect } from '@playwright/test'
export class PrivacyPage {
    readonly expect: PrivacyPageAssertions
    readonly body: Locator

    constructor (readonly page: Page) {
        this.expect = new PrivacyPageAssertions(this)
        this.body = page.locator('body')
    }

    async goTo (): Promise<void> {
        await this.page.goto('/privacy')
    }
}

class PrivacyPageAssertions {
    constructor (readonly privacyPage: PrivacyPage) { }
    async toBeOnTheRightPage(): Promise<void> {
        
        await expect(this.privacyPage.body).toContainText('Privacy notice for Find information about academies and trusts')
    }
}
