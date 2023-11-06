import { test } from './a11y-test'
import { PrivacyPage } from '../page-object-model/privacy-page'

test.describe('Page not found', () => {
    let privacyPage: PrivacyPage

    test.beforeEach(async ({ page }) => {
        privacyPage = new PrivacyPage(page)
    })

    test('Privacy page should have no automatically detectable accessibility issues', async ({ expectNoAccessibilityViolations }) => {
        await privacyPage.goTo()
        await expectNoAccessibilityViolations()
    })
})
