import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'
import { LogInPage } from '../page-object-model/log-in-page'
import { SearchPage } from '../page-object-model/search-page'

test.describe('Log in to application', () => {
  let homePage: HomePage
  let logInPage: LogInPage
  let searchPage: SearchPage

  test.beforeEach(({ page }) => {
    homePage = new HomePage(page)
    logInPage = new LogInPage(page)
    searchPage = new SearchPage(page)
  })

  test.describe('Given the user is authenticated', () => {
    test('when they navigate to the home page then the app homepage is displayed', async () => {
      await homePage.goTo()
      await homePage.expect.toBeOnTheRightPage()
    })
  })

  test.describe('Given the user is not authenticated', () => {
    test.use({ storageState: '.auth/unauthenticated-user.json' })

    test('when they navigate to the home page then the user is directed to a sign in form', async () => {
      await homePage.goTo()
      await logInPage.expect.toBeDirectedToSignIn()
    })

    test('when they navigate to the search page then the user is directed to a sign in form', async () => {
      await searchPage.goTo()
      await logInPage.expect.toBeDirectedToSignIn()
    })

    test('when a user fails to log in then they should not be redirected to home page', async () => {
      if (process.env.TEST_USER_ACCOUNT_NAME === null || process.env.TEST_USER_ACCOUNT_NAME === undefined) {
        throw new Error('Test user name must be defined')
      }
      const userName: string = process.env.TEST_USER_ACCOUNT_NAME

      await homePage.goTo()
      await logInPage.logIn(userName, 'wrongPassword')

      await homePage.expect.notToBeOnThePage()
    })
  })
})
