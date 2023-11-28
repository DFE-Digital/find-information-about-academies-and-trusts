import { test } from '@playwright/test'
import { DetailsPage } from '../../page-object-model/trust/details-page'
import { NotFoundPage } from '../../page-object-model/not-found-page'
import { FakeTestData } from '../../fake-data/fake-test-data'

test.describe('Details page', () => {
  let detailsPage: DetailsPage
  let notFoundPage: NotFoundPage

  test.beforeEach(async ({ page }) => {
    detailsPage = new DetailsPage(page, new FakeTestData())
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
      await detailsPage.goToPageWithoutUid()
      await notFoundPage.expect.toBeShownNotFoundMessage()

      await detailsPage.goToPageWithUidThatDoesNotExist()
      await notFoundPage.expect.toBeShownNotFoundMessage()
    })
  })

  test.describe('Links to other services', () => {
    test('given a user visits a page for an open mult-academy trust', async () => {
      await detailsPage.goToOpenMultiAcademyTrust()
      await detailsPage.expect.toSeeCorrectLinksForOpenTrust()
    })

    test('given a user visits a page for an open single-academy trust with academies', async () => {
      await detailsPage.goToOpenSingleAcademyTrustWithAcademies()
      await detailsPage.expect.toSeeCorrectLinksForOpenTrust()
    })

    test('given a user tries to visit a page for an open single-academy trust with no academies', async () => {
      await detailsPage.goToOpenSingleAcademyTrustWithNoAcademies()
      await detailsPage.expect.toSeeCorrectLinksForSingleAcademyTrustWithNoAcademies()
    })

    test('given a user tries to visit a page for a closed trust', async () => {
      await detailsPage.goToClosedTrust()
      await detailsPage.expect.toSeeCorrectLinksForClosedTrust()
    })
  })
})
