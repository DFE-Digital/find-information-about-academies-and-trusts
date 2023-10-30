import { test } from '../a11y-test'
import { DetailsPage } from '../../page-object-model/trust/details-page'
import { CurrentSearch } from '../../page-object-model/shared/search-form-component'
import { FakeTestData } from '../../fake-data/fake-test-data'

test.describe('Details page', () => {
  test('should not have any automatically detectable accessibility issues', async ({ expectNoAccessibilityViolations, page }) => {
    const detailsPage = new DetailsPage(page, new CurrentSearch(), new FakeTestData())
    await detailsPage.goTo()
    await detailsPage.expect.toSeeCorrectTrustNameAndTypeInHeader()
    await detailsPage.trustNavigation.expect.toBeVisible()
    await detailsPage.expect.toSeeCorrectTrustDetails()
    await detailsPage.expect.toSeeCorrectTrustReferenceNumbers()
    await expectNoAccessibilityViolations()
  })
})
