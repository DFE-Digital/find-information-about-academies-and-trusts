import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'
import { LogInPage } from '../page-object-model/log-in-page'

test.describe('Log in to application', () => {
  let homePage: HomePage
  let logInPage: LogInPage

  test.beforeEach(({ page }) => {
    homePage = new HomePage(page)
    logInPage = new LogInPage(page)
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
  })
})
