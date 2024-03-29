import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'
import { LogInPage } from '../page-object-model/log-in-page'
import { SearchPage } from '../page-object-model/search-page'
import { CurrentSearch } from '../page-object-model/shared/search-form-component'

test.describe('Log in to application', () => {
  let homePage: HomePage
  let logInPage: LogInPage
  let searchPage: SearchPage

  test.beforeEach(({ page }) => {
    const currentSearch = new CurrentSearch()
    homePage = new HomePage(page, currentSearch)
    logInPage = new LogInPage(page)
    searchPage = new SearchPage(page, currentSearch)
  })

  test.describe('Given the user is authenticated', () => {
    test('when they navigate to the home page then the app homepage is displayed', async () => {
      await homePage.goTo()
      await homePage.expect.toBeOnTheRightPage()
    })
  })

  test.describe('Given the user is not authenticated', () => {
    test.use({ extraHTTPHeaders: { Authorization: 'unauthenticated user' } })

    test('when they navigate to the home page then the user is directed to a sign in form', async () => {
      await homePage.goTo()
      await logInPage.expect.toBeDirectedToSignIn()
    })

    test('when they navigate to the search page then the user is directed to a sign in form', async () => {
      await searchPage.goTo()
      await logInPage.expect.toBeDirectedToSignIn()
    })
  })
})
