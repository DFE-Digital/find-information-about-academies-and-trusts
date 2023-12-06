import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'
import { CookiesPage } from '../page-object-model/cookies-page'
import { CurrentSearch } from '../page-object-model/shared/search-form-component'

test.describe('cookies', () => {
  let homePage: HomePage
  let cookiesPage: CookiesPage
  let currentSearch: CurrentSearch

  test.beforeEach(async ({ page }) => {
    currentSearch = new CurrentSearch()
    homePage = new HomePage(page, currentSearch)
    cookiesPage = new CookiesPage(page)
    await homePage.goTo()
    await homePage.expect.toBeOnTheRightPage()
  })

  test('app insights cookies are removed when user rejects cookies', async () => {
    await cookiesPage.expect.appInsightCookiesDoNotExist()
    await homePage.cookieBanner.acceptCookies()
    await homePage.goTo()
    await cookiesPage.expect.appInsightCookiesExist()
    await homePage.footerNavigation.goToCookies()
    await cookiesPage.rejectCookies()
    await cookiesPage.returnToPreviousPageViaLink()
    await cookiesPage.expect.appInsightCookiesDoNotExist()
  })
})
