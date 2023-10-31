import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'
import { SearchPage } from '../page-object-model/search-page'
import { CurrentSearch } from '../page-object-model/shared/search-form-component'

test('is deployed', async ({ page }) => {
  const homePage = new HomePage(page, new CurrentSearch())
  const searchPage = new SearchPage(page, new CurrentSearch())
  await homePage.goTo()
  await homePage.expect.toBeOnTheRightPage()

  await homePage.searchForm.typeASearchTerm()
  await homePage.searchForm.submitSearch()
  await searchPage.expect.toShowResults()
})
