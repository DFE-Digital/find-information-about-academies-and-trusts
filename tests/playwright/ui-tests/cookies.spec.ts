import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'
import { CookiesPage } from '../page-object-model/cookies-page'
import { CurrentSearch } from '../page-object-model/shared/search-form-component'
import { PrivacyPage } from '../page-object-model/privacy-page'
import { DetailsPage } from '../page-object-model/trust/details-page'
import { FakeTestData } from '../fake-data/fake-test-data'
import { SearchPage } from '../page-object-model/search-page'

test.describe('Cookies', () => {
  let homePage: HomePage
  let cookiesPage: CookiesPage
  let privacyPage: PrivacyPage
  let detailsPage: DetailsPage
  let searchPage: SearchPage

  test.beforeEach(async ({ page }) => {
    const currentSearch = new CurrentSearch()
    cookiesPage = new CookiesPage(page)
    homePage = new HomePage(page, currentSearch)
    searchPage = new SearchPage(page, currentSearch)
    privacyPage = new PrivacyPage(page)
    detailsPage = new DetailsPage(page, new FakeTestData())
  })

  // reset cookies to default
  test.use({ storageState: { cookies: [], origins: [] } })

  test.describe('page', () => {
    test('user should be able to accept cookies at cookies page', async ({ browserName }) => {
      test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')

      await cookiesPage.goTo()
      await cookiesPage.expect.acceptCookiesRadioButtonNotToBeChecked()
      await cookiesPage.expect.rejectCookiesRadioButtonNotToBeChecked()

      await cookiesPage.acceptCookies()

      await homePage.cookieBanner.expect.toShowCookiesAcceptedMessage()

      await cookiesPage.expect.acceptCookiesRadioButtonToBeChecked()
      await cookiesPage.expect.rejectCookiesRadioButtonNotToBeChecked()
    })

    test('user should be able to reject cookies at cookies page', async ({ browserName }) => {
      test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')

      await cookiesPage.goTo()
      await cookiesPage.expect.acceptCookiesRadioButtonNotToBeChecked()
      await cookiesPage.expect.rejectCookiesRadioButtonNotToBeChecked()

      await cookiesPage.rejectCookies()

      await homePage.cookieBanner.expect.toShowCookiesRejectedMessage()

      await cookiesPage.expect.acceptCookiesRadioButtonNotToBeChecked()
      await cookiesPage.expect.rejectCookiesRadioButtonToBeChecked()
    })

    test('user should be able to return to previous page after setting cookie preferences', async ({ browserName }) => {
      test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')

      await homePage.goTo()
      await homePage.footerNavigation.goToCookies()

      await cookiesPage.expect.returnToLinkPageToNotBeVisible()
      await cookiesPage.acceptCookies()
      await cookiesPage.expect.returnToLinkPageToBeVisible()

      await cookiesPage.returnToPreviousPageViaLink()
      await homePage.expect.toBeOnTheRightPage()

      await homePage.footerNavigation.goToPrivacyPolicy()
      await privacyPage.expect.toBeOnTheRightPage()

      await privacyPage.footerNavigation.goToCookies()
      await cookiesPage.expect.returnToLinkPageToNotBeVisible()
      await cookiesPage.rejectCookies()
      await cookiesPage.expect.returnToLinkPageToBeVisible()

      await cookiesPage.returnToPreviousPageViaLink()
      await privacyPage.expect.toBeOnTheRightPage()
    })
  })

  test.describe('banner', () => {
    test.beforeEach(async () => {
      await homePage.goTo()
    })

    test('should be visible on all page types before setting a cookies preference', async ({ browserName }) => {
      test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')

      await homePage.cookieBanner.expect.toAskForCookiePreferences()

      await privacyPage.goTo()
      await privacyPage.cookieBanner.expect.toAskForCookiePreferences()

      await searchPage.goToSearchWithResults()
      await searchPage.cookieBanner.expect.toAskForCookiePreferences()

      await detailsPage.goToMultiAcademyTrust()
      await detailsPage.cookieBanner.expect.toAskForCookiePreferences()
    })

    test('should be able to navigate to cookies page from banner', async ({ browserName }) => {
      await homePage.cookieBanner.expect.toAskForCookiePreferences()

      await homePage.cookieBanner.goToCookiesPage()

      await cookiesPage.expect.toBeOnTheRightPage()
    })

    test('user should be able to accept cookies at cookies banner', async ({ browserName }) => {
      test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')
      await homePage.cookieBanner.expect.toAskForCookiePreferences()

      await homePage.cookieBanner.acceptCookies()

      await homePage.cookieBanner.expect.notToAskForCookiePreferences()
      await homePage.cookieBanner.expect.toShowCookiesAcceptedMessage()

      await homePage.footerNavigation.goToCookies()
      await cookiesPage.expect.acceptCookiesRadioButtonToBeChecked()
    })

    test('user should be able to reject cookies at cookies banner', async ({ browserName }) => {
      test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')
      await homePage.cookieBanner.expect.toAskForCookiePreferences()

      await homePage.cookieBanner.rejectCookies()

      await homePage.cookieBanner.expect.notToAskForCookiePreferences()
      await homePage.cookieBanner.expect.toShowCookiesRejectedMessage()

      await homePage.footerNavigation.goToCookies()
      await cookiesPage.expect.rejectCookiesRadioButtonToBeChecked()
    })

    test('user should be able to set preferences at cookies banner then change their preferences on the cookies page', async ({ browserName }) => {
      test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')
      await homePage.cookieBanner.expect.toAskForCookiePreferences()
      await homePage.cookieBanner.rejectCookies()

      await homePage.footerNavigation.goToCookies()
      await cookiesPage.expect.rejectCookiesRadioButtonToBeChecked()

      await cookiesPage.acceptCookies()

      await cookiesPage.expect.acceptCookiesRadioButtonToBeChecked()
    })
  })
})
