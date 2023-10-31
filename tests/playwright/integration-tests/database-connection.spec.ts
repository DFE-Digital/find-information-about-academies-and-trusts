import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'
import { SearchPage } from '../page-object-model/search-page'
import { CurrentSearch } from '../page-object-model/shared/search-form-component'

test.describe('search for trusts', () => {
  let homePage: HomePage
  let searchPage: SearchPage

  test.beforeEach(({ page }) => {
    const currentSearch = new CurrentSearch()
    homePage = new HomePage(page, currentSearch)
    searchPage = new SearchPage(page, currentSearch)
  })

  test('returns results', async () => {
    await homePage.goTo()
    await homePage.searchForm.typeASearchTerm()
    await homePage.searchForm.submitSearch()
    await searchPage.expect.toShowResults()
  })

  test('returns empty results', async () => {
    await homePage.goTo()
    await homePage.searchForm.typeASearchTermWithNoMatches()
    await homePage.searchForm.submitSearch()
    await searchPage.expect.toSeeNoResultsMessage()
  })
})
