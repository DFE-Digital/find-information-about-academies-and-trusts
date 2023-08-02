import { test as setup } from '@playwright/test'
import { HomePage } from './page-object-model/home-page'

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

  await page.goto('/')

  await page.locator('[name=loginfmt]').fill((username))
  await page.getByRole('button', { name: 'Next' }).click()

  await page.locator('[name=passwd]').fill((password))
  await page.getByRole('button', { name: 'Sign in' }).click()

  // Wait until we are on home page, we might (or might not) have to press a "Stay signed in" button
  const timeout = 3000
  const maxtime = Date.now() + timeout
  const step = 500
  const staySignedInLocator = page.getByText('Stay signed in')
  while (Date.now() < maxtime) {
    try {
      if (await staySignedInLocator.isVisible()) {
        await page.getByRole('button', { name: 'No' }).click()
        break
      } else if ((await page.title()) === 'Home page - Find information about academies and trusts') {
        break
      } else {
        await page.waitForTimeout(step)
      }
    } catch (error) {
      // if we are mid navigation then an error will be thrown when we attempt to access the page
      // we should wait and try again
      await page.waitForTimeout(step)
    }
  }
  await homePage.expect.toBeOnTheRightPage()

  await page.context().storageState({ path: authFile })
})

setup('save unauthenticated state', async ({ page }) => {
  await page.context().storageState({ path: unauthenticatedFile })
})
