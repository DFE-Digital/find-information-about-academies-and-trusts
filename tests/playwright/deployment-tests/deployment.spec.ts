import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'

test('is deployed', async ({ page }) => {
  const homePage = new HomePage(page)
  await homePage.goTo()
  await homePage.expect.toBeOnTheRightPage()
})
