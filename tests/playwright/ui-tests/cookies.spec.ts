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

  test('user shoud be able to accept cookies at cookies page', async () => {
    await homePage.footerNavigation.clickCookies()
    await cookiesPage.expect.returnToLinkPageToNotBeVisble()
    await cookiesPage.acceptCookies()
    await cookiesPage.expect.returnToLinkPageToBeVisble()
    await cookiesPage.clickBackToHomePage()
    await homePage.expect.toBeOnTheRightPage()
    await homePage.footerNavigation.clickCookies()
    await cookiesPage.expect.acceptCookiesRadioButtonIsChecked()
  })

  test('user shoud be able to reject cookies at cookies page', async () => {
    await homePage.footerNavigation.clickCookies()
    await cookiesPage.expect.returnToLinkPageToNotBeVisble()
    await cookiesPage.rejectCookies()
    await cookiesPage.expect.returnToLinkPageToBeVisble()
    await cookiesPage.clickBackToHomePage()
    await homePage.expect.toBeOnTheRightPage()
    await homePage.footerNavigation.clickCookies()
    await cookiesPage.expect.rejectCookiesRadioButtonIsChecked()
  })

  test('user shoud be able to accept cookies at cookies banner', async () => {
    await homePage.cookieBannerNavigation.expect.isVisible()
    await homePage.cookieBannerNavigation.clickAcceptCookies()
    await homePage.cookieBannerNavigation.expect.isNotVisible()
    await homePage.footerNavigation.clickCookies()
    await cookiesPage.expect.acceptCookiesRadioButtonIsChecked()
  })

  test('user shoud be able to reject cookies at cookies banner', async () => {
    await homePage.cookieBannerNavigation.expect.isVisible()
    await homePage.cookieBannerNavigation.clickRejectCookies()
    await homePage.cookieBannerNavigation.expect.isNotVisible()
    await homePage.footerNavigation.clickCookies()
    await cookiesPage.expect.rejectCookiesRadioButtonIsChecked()
  })
})
