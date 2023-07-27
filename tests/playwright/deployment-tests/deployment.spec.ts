import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'
import { SearchResultsPage } from '../page-object-model/search-results-page'

test('is deployed', async ({ page }) => {
  const homePage = new HomePage(page)
  const searchResultsPage = new SearchResultsPage(page)
  await homePage.goTo()
  await homePage.expect.toBeOnTheRightPage()

  await homePage.searchFor('education')
  await searchResultsPage.expect.toShowResults()
})
