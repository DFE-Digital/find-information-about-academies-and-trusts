import { test, expect } from '@playwright/test'

test.describe('When the user is authenticated', () => {
  test('the app homepage is displayed', async ({ page }) => {
    await page.goto('/')
    await expect(page).toHaveTitle('Home page - Find information about academies and trusts')
  })
})

test.describe('When the user is not authenticated', () => {
  test('the user is directed to a sign in form', async ({ page }) => {
    await page.goto('/')
    await expect(page).toHaveTitle('Sign in to your account')
  })
})
