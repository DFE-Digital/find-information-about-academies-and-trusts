import { test, expect } from '@playwright/test';

test('is deployed', async ({ page }) => {
  await page.goto('/');

  await expect(page).toHaveTitle('Home page - DfE.FindInformationAcademiesTrusts');
});

test('this test fails', async ({ page }) => {
  await page.goto('/');

  await expect(page).toHaveTitle('This is not the page title');
});
