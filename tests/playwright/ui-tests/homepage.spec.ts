import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'
import { SearchPage } from '../page-object-model/search-page'
import { MockTrustsProvider } from '../mocks/mock-trusts-provider'

test.describe('homepage', () => {
  let homePage: HomePage
  let searchPage: SearchPage

  test.beforeAll(async () => {
    const mockTrustsProvider = new MockTrustsProvider()
    await mockTrustsProvider.registerGetTrusts()
  })

  test.beforeEach(async ({ page }) => {
    homePage = new HomePage(page)
    searchPage = new SearchPage(page)
    await homePage.goTo()
  })

  const searchTerms = ['1', 'trust']

  for (const searchTerm of searchTerms) {
    test(`Searching for a trust with "${searchTerm}" navigates to search page with results for`, async () => {
      await homePage.searchFor(searchTerm)

      await searchPage.expect.toBeOnTheRightPage()
      await searchPage.expect.toBeOnPageWithResultsFor(searchTerm)
    })
  }
})
