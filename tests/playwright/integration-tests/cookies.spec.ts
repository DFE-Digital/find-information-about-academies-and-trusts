import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'
import { CookiesPage } from '../page-object-model/cookies-page'
import { CurrentSearch } from '../page-object-model/shared/search-form-component'

test.describe('cookies', () => {
  // reset cookies to default
  test.use({ storageState: { cookies: [], origins: [] } })

  // Cookies integration tests are failing more often than not, we don't really understand why and we can't replicate the issue manually
  // Marking as fixme until there is time to address the problem
  test.fixme('app insights cookies are not present by default, are added when user accepts cookies and are removed when user rejects cookies', async ({ page }) => {
    const homePage = new HomePage(page, new CurrentSearch())
    const cookiesPage = new CookiesPage(page)

    // Defaults to no cookies
    await homePage.goTo()
    await homePage.expect.toBeOnTheRightPage()
    await homePage.expect.notToHaveAppInsightsCookies()

    // Can accept cookies
    await cookiesPage.goTo()
    await cookiesPage.acceptCookies()
    await cookiesPage.expect.toHaveAppInsightCookies()
    await homePage.goTo()
    await homePage.expect.toHaveAppInsightCookies() // ensure that cookie settings persist across pages

    // Can reject cookies
    await cookiesPage.goTo()
    await cookiesPage.rejectCookies()
    await cookiesPage.expect.notToHaveAppInsightsCookies()
    await homePage.goTo()
    await homePage.expect.notToHaveAppInsightsCookies() // ensure that cookie settings persist across pages
  })
})
