import { test } from './a11y-test'
import { AccessibilityPage } from '../page-object-model/accessibility-page'

test.describe('Accessibility page', () => {
  let accessibilityPage: AccessibilityPage

  test.beforeEach(async ({ page }) => {
    accessibilityPage = new AccessibilityPage(page)
  })

  test('Accessibility page should have no automatically detectable accessibility issues', async ({ expectNoAccessibilityViolations }) => {
    await accessibilityPage.goTo()
    await expectNoAccessibilityViolations()
  })
})
