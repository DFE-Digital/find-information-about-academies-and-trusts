import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'
import { CookiesPage } from '../page-object-model/cookies-page'
import { CurrentSearch } from '../page-object-model/shared/search-form-component'

test.describe('cookies', () => {
  // reset cookies to default
  test.use({ storageState: { cookies: [], origins: [] } })

  test('app insights cookies are not present by default, are added when user accepts cookies and are removed when user rejects cookies', async ({ page }) => {
    const homePage = new HomePage(page, new CurrentSearch())
    const cookiesPage = new CookiesPage(page)

    await homePage.goTo()
    await homePage.expect.toBeOnTheRightPage()
    await homePage.expect.notToHaveAppInsightsCookies()

    await homePage.cookieBanner.acceptCookies()
    await homePage.goTo() // Cookie settings may not apply until refresh of page due to server side order of setting/using cookie preferences cookie
    await homePage.expect.toHaveAppInsightCookies()

    await cookiesPage.goTo()
    await cookiesPage.expect.toHaveAppInsightCookies() // ensure that cookie settings persist

    await cookiesPage.rejectCookies()
    await cookiesPage.goTo() // Cookie settings may not apply until refresh of page due to server side order of setting/using cookie preferences cookie
    await cookiesPage.expect.notToHaveAppInsightsCookies()

    await homePage.goTo()
    await homePage.expect.notToHaveAppInsightsCookies() // ensure that cookie settings persist
  })
})
