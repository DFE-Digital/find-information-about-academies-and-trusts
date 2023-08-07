import { test as setup } from '@playwright/test'
import { HomePage } from './page-object-model/home-page'
import { LogInPage } from './page-object-model/log-in-page'

if (process.env.TEST_USER_ACCOUNT_NAME === null || process.env.TEST_USER_ACCOUNT_NAME === undefined) {
  throw new Error('Test user name must be defined')
}
if (process.env.TEST_USER_ACCOUNT_PASSWORD === null || process.env.TEST_USER_ACCOUNT_PASSWORD === undefined) {
  throw new Error('Test user password must be defined')
}

const authFile = '.auth/user.json'
const unauthenticatedFile = '.auth/unauthenticated-user.json'
const username: string = process.env.TEST_USER_ACCOUNT_NAME
const password: string = process.env.TEST_USER_ACCOUNT_PASSWORD

setup('authenticate', async ({ page }) => {
  const homePage = new HomePage(page)
  const logInPage = new LogInPage(page)

  await logInPage.goTo()
  await logInPage.logIn(username, password)

  await homePage.expect.toBeOnTheRightPage()

  await page.context().storageState({ path: authFile })
})

setup('save unauthenticated state', async ({ page }) => {
  await page.context().storageState({ path: unauthenticatedFile })
})
