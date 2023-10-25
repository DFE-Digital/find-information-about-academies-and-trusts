import { test } from '../a11y-test'
import { DetailsPage } from '../../page-object-model/trust/details-page'
import { CurrentSearch } from '../../page-object-model/shared/search-form-component'

test.describe('Details page', () => {
  test('should not have any automatically detectable accessibility issues', async ({ expectNoAccessibilityViolations, page }) => {
    const detailsPage = new DetailsPage(page, new CurrentSearch())
    await detailsPage.goTo()
    await detailsPage.expect.toSeeCorrectTrustNameAndTypeInHeader()
    await detailsPage.trustNavigation.expect.toBeVisible()
    await detailsPage.expect.toSeeCorrectTrustDetails()
    await detailsPage.expect.toSeeCorrectTrustReferenceNumbers()
    await expectNoAccessibilityViolations()
  })
})
