import { test } from '@playwright/test'
import { DetailsPage } from '../../page-object-model/trust/details-page'
import { NotFoundPage } from '../../page-object-model/not-found-page'
import { MockTrustsProvider } from '../../mocks/mock-trusts-provider'

test.describe('Details page', () => {
  let detailsPage: DetailsPage
  let notFoundPage: NotFoundPage

  test.beforeEach(async ({ page }) => {
    detailsPage = new DetailsPage(page)
    await detailsPage.goTo()
  })

  test('user should see trust name and type', async () => {
    await detailsPage.expect.toSeeCorrectTrustNameAndTypeInHeader()
  })

  test('user should see the correct trust information', async () => {
    await detailsPage.expect.toSeeCorrectTrustDetails()
    await detailsPage.expect.toSeeCorrectTrustReferenceNumbers()
  })

  test.describe('given a user tries to visit the url without an existing trust', () => {
    test.beforeEach(({ page }) => {
      notFoundPage = new NotFoundPage(page)
    })

    test('then they should see a not found message', async () => {
      await detailsPage.goTo('')
      await notFoundPage.expect.toBeShownNotFoundMessage()

      await detailsPage.goTo(MockTrustsProvider.nonExistingTrustUkprn)
      await notFoundPage.expect.toBeShownNotFoundMessage()
    })
  })
})
