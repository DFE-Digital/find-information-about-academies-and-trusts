import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'
import { CookiesPage } from '../page-object-model/cookies-page'
import { CurrentSearch } from '../page-object-model/shared/search-form-component'

test.describe('cookies', () => {
  let homePage: HomePage
  let cookiesPage: CookiesPage
  let currentSearch: CurrentSearch

  test.beforeEach(async ({ page, context }) => {
    currentSearch = new CurrentSearch()
    homePage = new HomePage(page, currentSearch)
    cookiesPage = new CookiesPage(page)
    await homePage.goTo()
    await homePage.expect.toBeOnTheRightPage()
  })

  test('app insights cookies are removed when user rejects cookies', async ({ browserName }) => {
    test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')
    await cookiesPage.expect.appInsightCookiesDoNotExist()
    await homePage.cookieBannerNavigation.clickAcceptCookies()
    await homePage.goTo()
    await cookiesPage.expect.appInsightCookiesExist()
    await homePage.footerNavigation.clickCookies()
    await cookiesPage.rejectCookies()
    await cookiesPage.clickBackToHomePage()
    await cookiesPage.expect.appInsightCookiesDoNotExist()
  })
})