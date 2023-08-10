import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'
import { SearchPage } from '../page-object-model/search-page'

test('is deployed', async ({ page }) => {
  const homePage = new HomePage(page)
  const searchPage = new SearchPage(page)
  await homePage.goTo()
  await homePage.expect.toBeOnTheRightPage()

  await homePage.searchFor('education')
  await searchPage.expect.toShowResults()
})
