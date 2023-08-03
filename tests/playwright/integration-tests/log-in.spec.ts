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

  test.describe('When the user is authenticated', () => {
    test('the app homepage is displayed', async () => {
      await homePage.goTo()
      await homePage.expect.toBeOnTheRightPage()
    })
  })

  test.describe('When the user is not authenticated', () => {
    test.use({ storageState: '.auth/unauthenticated-user.json' })

    test('the user is directed to a sign in form', async ({ page }) => {
      await homePage.goTo()
      await logInPage.expect.toBeDirectedToSignIn()
    })
  })
})
