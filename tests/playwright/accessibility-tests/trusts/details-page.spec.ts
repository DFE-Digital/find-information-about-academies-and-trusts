import { test } from '../a11y-test'
import { DetailsPage } from '../../page-object-model/trust/details-page'

test.describe('Details page', () => {
  test('should not have any automatically detectable accessibility issues', async ({ expectNoAccessibilityViolations, page }) => {
    const detailsPage = new DetailsPage(page)
    await detailsPage.goTo()
    await detailsPage.expect.toSeeCorrectTrustNameAndTypeInHeader()
    await detailsPage.trustNavigation.expect.toBeVisible()

    await expectNoAccessibilityViolations()
  })
})
