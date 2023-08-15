import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'
import { SearchPage } from '../page-object-model/search-page'
import { DetailsPage } from '../page-object-model/trust/details-page'
import { MockTrustsProvider } from '../mocks/mock-trusts-provider'

test.describe('homepage', () => {
  let homePage: HomePage
  let searchPage: SearchPage
  let mockTrustsProvider: MockTrustsProvider

  test.beforeAll(async () => {
    mockTrustsProvider = new MockTrustsProvider()
    await mockTrustsProvider.registerGetTrusts()
  })

  test.beforeEach(async ({ page }) => {
    homePage = new HomePage(page)
    searchPage = new SearchPage(page)
    await homePage.goTo()
  })

  test('Searching for a trust from the homepage displays formatted information about each trust', async () => {
    await searchPage.goToSearchFor('trust')
    await searchPage.expect.toBeOnPageWithResultsFor('trust')

    await searchPage.expect.toShowResultsWithCount(3)

    await searchPage.expect.toShowResultWithText('trust 1', '12 Paddle Road, Bushy Park, Letworth, Manchester, MX12 P34')
    await searchPage.expect.toShowResultWithText('trust 2', '12 Paddle Road, Manchester, MX12 P34')
    await searchPage.expect.toShowResultWithText('trust 3', 'Bushy Park, Manchester')

    await searchPage.expect.toShowResultWithAcademiesinTrustCount('trust 1', 1)
    await searchPage.expect.toShowResultWithAcademiesinTrustCount('trust 1', 2)
    await searchPage.expect.toShowResultWithAcademiesinTrustCount('trust 1', 0)
  })

  for (const trustResponse of MockTrustsProvider.fakeTrustsResponseData) {
    const trustName = trustResponse.GroupName

    test(`Clicking on a result navigates to the details page for ${trustName}`, async ({ page }) => {
      await mockTrustsProvider.registerGetTrustByUkprn(trustName, trustResponse.Ukprn)
      const detailsPage = new DetailsPage(page)

      await searchPage.goToSearchFor('trust')
      await searchPage.expect.toBeOnPageWithResultsFor('trust')

      await searchPage.clickOnSearchResultLinkWithText(trustName)
      await detailsPage.expect.toBeOnTheRightPageFor(trustName)
    })
  }
})
