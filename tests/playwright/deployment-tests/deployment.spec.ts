import { test, expect } from '@playwright/test'

test('is deployed', async ({ page }) => {
  await page.goto('/')

  await expect(page).toHaveTitle('Home page - Find information about academies and trusts')
})
