import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'
import { SearchPage } from '../page-object-model/search-page'

test.describe('search for trusts', () => {
  let homePage: HomePage
  let searchPage: SearchPage

  test.beforeEach(({ page }) => {
    homePage = new HomePage(page)
    searchPage = new SearchPage(page)
  })

  test('returns results', async () => {
    await homePage.goTo()
    await homePage.searchFor('education')

    await searchPage.expect.toShowResults()
  })

  test('returns empty results', async () => {
    await homePage.goTo()
    await homePage.searchFor('trust that does not exist')

    await searchPage.expect.toShowEmptyResultMessage()
  })
})
