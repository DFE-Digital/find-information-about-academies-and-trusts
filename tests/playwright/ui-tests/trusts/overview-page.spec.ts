import { test } from '@playwright/test'
import { OverviewPage } from '../../page-object-model/trust/overview-page'
import { NotFoundPage } from '../../page-object-model/not-found-page'
import { MockTrustsProvider } from '../../mocks/mock-trusts-provider'

test.describe('Overview page', () => {
  let overviewPage: OverviewPage
  let notFoundPage: NotFoundPage

  test.beforeEach(async ({ page }) => {
    overviewPage = new OverviewPage(page)
    await overviewPage.goTo()
  })

  test('user should see trust name and type', async () => {
    await overviewPage.expect.toBeOnTheRightPage()
    await overviewPage.expect.toSeeCorrectTrustNameAndTypeInHeader()
  })

  test('user should see the correct trust information', async () => {
    await overviewPage.expect.toSeeCorrectTrustSummary()
    await overviewPage.expect.toSeeCorrectOfstedRatings()
  })

  test.describe('given a user tries to visit the url without an existing trust', () => {
    test.beforeEach(({ page }) => {
      notFoundPage = new NotFoundPage(page)
    })

    test('then they should see a not found message', async () => {
      await overviewPage.goTo('')
      await notFoundPage.expect.toBeShownNotFoundMessage()

      await overviewPage.goTo(MockTrustsProvider.nonExistingTrustUkprn)
      await notFoundPage.expect.toBeShownNotFoundMessage()
    })
  })
})