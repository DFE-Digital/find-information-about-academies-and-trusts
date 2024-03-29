import { test } from '@playwright/test'
import { OverviewPage } from '../../page-object-model/trust/overview-page'
import { NotFoundPage } from '../../page-object-model/not-found-page'
import { FakeTestData } from '../../fake-data/fake-test-data'
import { javaScriptContexts } from '../../helpers'
import { DataSourcePanelItem } from '../../page-object-model/trust/sources-and-updates-component'

test.describe('Overview page', () => {
  let overviewPage: OverviewPage
  let notFoundPage: NotFoundPage
  const sources: DataSourcePanelItem[] = [{ fields: 'Trust summary, Ofsted ratings:', dataSource: 'Get information about schools', update: 'Daily' }]

  test.beforeEach(async ({ page }) => {
    overviewPage = new OverviewPage(page, new FakeTestData())
  })

  for (const javaScriptContext of javaScriptContexts) {
    test.describe(`With JavaScript ${javaScriptContext.name}`, () => {
      test.use({ javaScriptEnabled: javaScriptContext.isEnabled })

      test('user should see trust name and type', async () => {
        await overviewPage.goTo()
        await overviewPage.expect.toBeOnTheRightPage()
        await overviewPage.expect.toSeeCorrectTrustNameAndTypeInHeader()
      })

      test('user should see the correct trust information', async () => {
        await overviewPage.goToMultiAcademyTrust()
        await overviewPage.expect.toSeeCorrectTrustSummary()
        await overviewPage.expect.toSeePopulatedOfstedRatings()
      })

      test('user sees the correct information in the source and updates panel', async () => {
        await overviewPage.goTo()
        await overviewPage.expect.toSeeCorrectSourceAndUpdates(sources)
      })

      test('user should see the correct trust information on trust with no academies', async () => {
        await overviewPage.goToTrustWithNoAcademies()
        await overviewPage.expect.toSeeCorrectTrustSummaryWithNoAcademies()
        await overviewPage.expect.toSeeCorrectOfstedRatingsWithNoAcademies()
      })

      test.describe('given a user tries to visit the url without an existing trust', () => {
        test.beforeEach(({ page }) => {
          notFoundPage = new NotFoundPage(page)
        })

        test('then they should see a not found message', async () => {
          await overviewPage.goToPageWithoutUid()
          await notFoundPage.expect.toBeShownNotFoundMessage()

          await overviewPage.goToPageWithUidThatDoesNotExist()
          await notFoundPage.expect.toBeShownNotFoundMessage()
        })
      })
    })
  }
})
