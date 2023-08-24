import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'
import { SearchPage } from '../page-object-model/search-page'

test.describe('homepage', () => {
  let homePage: HomePage
  let searchPage: SearchPage

  test.beforeEach(async ({ page }) => {
    homePage = new HomePage(page)
    searchPage = new SearchPage(page)
    await homePage.goTo()
  })

  const searchTerms = ['trust', 'education']

  for (const searchTerm of searchTerms) {
    test(`Searching for a trust with "${searchTerm}" navigates to search page with results for`, async () => {
      await homePage.searchFor(searchTerm)

      await searchPage.expect.toBeOnTheRightPage()
      await searchPage.expect.toBeOnPageWithResultsFor(searchTerm)
    })
  }
})
