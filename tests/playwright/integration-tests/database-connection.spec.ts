import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'
import { SearchResultsPage } from '../page-object-model/search-results-page'

test.describe('search for trusts', () => {
  let homePage: HomePage
  let searchResultsPage: SearchResultsPage

  test.beforeEach(({ page }) => {
    homePage = new HomePage(page)
    searchResultsPage = new SearchResultsPage(page)
  })

  test('returns results', async () => {
    await homePage.goTo()
    await homePage.searchFor('education')

    await searchResultsPage.expect.toShowResults()
  })

  test('returns empty results', async () => {
    await homePage.goTo()
    await homePage.searchFor('trust that does not exist')

    await searchResultsPage.expect.toShowEmptyResultMessage()
  })
})
