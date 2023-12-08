import { CookiesPage } from '../page-object-model/cookies-page'
import { HomePage } from '../page-object-model/home-page'
import { CurrentSearch } from '../page-object-model/shared/search-form-component'
import { test } from './a11y-test'

test.describe('Cookies', () => {
  let cookiesPage: CookiesPage
  let homePage: HomePage

  // reset cookies to default
  test.use({ storageState: { cookies: [], origins: [] } })

  test.beforeEach(async ({ page }) => {
    cookiesPage = new CookiesPage(page)
    homePage = new HomePage(page, new CurrentSearch())
  })

  test('page should have no automatically detectable accessibility issues', async ({ expectNoAccessibilityViolations }) => {
    await cookiesPage.goTo()
    await cookiesPage.expect.toBeOnTheRightPage()
    await expectNoAccessibilityViolations()

    await cookiesPage.acceptCookies()
    await expectNoAccessibilityViolations()
  })

  test('accepted banner should have no automatically detectable accessibility issues', async ({ expectNoAccessibilityViolations }) => {
    await homePage.goTo()
    await homePage.cookieBanner.expect.toAskForCookiePreferences()
    await expectNoAccessibilityViolations()

    await homePage.cookieBanner.acceptCookies()
    await homePage.cookieBanner.expect.notToAskForCookiePreferences()
    await homePage.cookieBanner.expect.toShowCookiesAcceptedMessage()
    await expectNoAccessibilityViolations()
  })

  test('rejected banner should have no automatically detectable accessibility issues', async ({ expectNoAccessibilityViolations }) => {
    await homePage.goTo()
    await homePage.cookieBanner.expect.toAskForCookiePreferences()
    await expectNoAccessibilityViolations()

    await homePage.cookieBanner.rejectCookies()
    await homePage.cookieBanner.expect.notToAskForCookiePreferences()
    await homePage.cookieBanner.expect.toShowCookiesRejectedMessage()
    await expectNoAccessibilityViolations()
  })
})
