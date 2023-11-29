import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'
import { CookiesPage } from '../page-object-model/cookies-page'
import { CurrentSearch } from '../page-object-model/shared/search-form-component'

test.describe('cookiespage', () => {
  let homePage: HomePage
  let cookiesPage: CookiesPage
  let currentSearch: CurrentSearch

  test.beforeEach(async ({ page, context }) => {
    currentSearch = new CurrentSearch()
    homePage = new HomePage(page, currentSearch)
    cookiesPage = new CookiesPage(page)
    await homePage.goTo()
  })

  test('user should be able to accept cookies at cookies page', async ({ browserName }) => {
    test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')
    await homePage.footerNavigation.clickCookies()
    await cookiesPage.expect.returnToLinkPageToNotBeVisible()
    await cookiesPage.expect.acceptCookiesRadioButtonIsNotChecked()
    await cookiesPage.expect.rejectCookiesRadioButtonIsNotChecked()
    await cookiesPage.acceptCookies()
    await cookiesPage.expect.returnToLinkPageToBeVisible()
    await homePage.cookieBannerNavigation.expect.isAccepted()
    await cookiesPage.clickBackToHomePage()
    await homePage.expect.toBeOnTheRightPage()
    await homePage.footerNavigation.clickCookies()
    await cookiesPage.expect.acceptCookiesRadioButtonIsChecked()
    await cookiesPage.expect.rejectCookiesRadioButtonIsNotChecked()
  })

  test('user should be able to reject cookies at cookies page', async ({ browserName }) => {
    test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')
    await homePage.footerNavigation.clickCookies()
    await cookiesPage.expect.returnToLinkPageToNotBeVisible()
    await cookiesPage.expect.acceptCookiesRadioButtonIsNotChecked()
    await cookiesPage.expect.rejectCookiesRadioButtonIsNotChecked()
    await cookiesPage.rejectCookies()
    await cookiesPage.expect.returnToLinkPageToBeVisible()
    await homePage.cookieBannerNavigation.expect.isRejected()
    await cookiesPage.clickBackToHomePage()
    await homePage.expect.toBeOnTheRightPage()
    await homePage.footerNavigation.clickCookies()
    await cookiesPage.expect.rejectCookiesRadioButtonIsChecked()
    await cookiesPage.expect.acceptCookiesRadioButtonIsNotChecked()
  })

  test('user should be able to accept cookies at cookies banner', async ({ browserName }) => {
    test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')
    await homePage.cookieBannerNavigation.expect.isVisible()
    await homePage.cookieBannerNavigation.clickAcceptCookies()
    await homePage.cookieBannerNavigation.expect.isNotVisible()
    await homePage.cookieBannerNavigation.expect.isAccepted()
    await homePage.footerNavigation.clickCookies()
    await cookiesPage.expect.acceptCookiesRadioButtonIsChecked()
  })

  test('user should be able to reject cookies at cookies banner', async ({ browserName }) => {
    test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')
    await homePage.cookieBannerNavigation.expect.isVisible()
    await homePage.cookieBannerNavigation.clickRejectCookies()
    await homePage.cookieBannerNavigation.expect.isNotVisible()
    await homePage.cookieBannerNavigation.expect.isRejected()
    await homePage.footerNavigation.clickCookies()
    await cookiesPage.expect.rejectCookiesRadioButtonIsChecked()
  })

  test('user should be able to accept cookies at cookies banner then change their preferences on the cookies page', async ({ browserName }) => {
    test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')
    await homePage.cookieBannerNavigation.expect.isVisible()
    await homePage.cookieBannerNavigation.clickAcceptCookies()
    await homePage.cookieBannerNavigation.expect.isNotVisible()
    await homePage.cookieBannerNavigation.expect.isAccepted()
    await homePage.footerNavigation.clickCookies()
    await cookiesPage.expect.acceptCookiesRadioButtonIsChecked()
    await cookiesPage.rejectCookies()
    await cookiesPage.expect.returnToLinkPageToBeVisible()
    await homePage.cookieBannerNavigation.expect.isRejected()
    await cookiesPage.clickBackToHomePage()
    await homePage.expect.toBeOnTheRightPage()
    await homePage.footerNavigation.clickCookies()
    await cookiesPage.expect.rejectCookiesRadioButtonIsChecked()
    await cookiesPage.expect.acceptCookiesRadioButtonIsNotChecked()
  })

  test('user should be able to reject cookies at cookies banner then change their preferences on the cookies page', async ({ browserName }) => {
    test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')
    await homePage.cookieBannerNavigation.expect.isVisible()
    await homePage.cookieBannerNavigation.clickRejectCookies()
    await homePage.cookieBannerNavigation.expect.isNotVisible()
    await homePage.cookieBannerNavigation.expect.isRejected()
    await homePage.footerNavigation.clickCookies()
    await cookiesPage.expect.rejectCookiesRadioButtonIsChecked()
    await cookiesPage.acceptCookies()
    await cookiesPage.expect.returnToLinkPageToBeVisible()
    await homePage.cookieBannerNavigation.expect.isAccepted()
    await cookiesPage.clickBackToHomePage()
    await homePage.expect.toBeOnTheRightPage()
    await homePage.footerNavigation.clickCookies()
    await cookiesPage.expect.acceptCookiesRadioButtonIsChecked()
    await cookiesPage.expect.rejectCookiesRadioButtonIsNotChecked()
  })
})
