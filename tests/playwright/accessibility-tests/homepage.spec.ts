import { test, expect } from '@playwright/test'
import AxeBuilder from '@axe-core/playwright'
import { HomePage } from '../page-object-model/home-page'

test.describe('home page', () => {
  test('should not have any automatically detectable accessibility issues', async ({ page }) => {
    const homePage = new HomePage(page)
    await homePage.goTo()

    const accessibilityScanResults = await new AxeBuilder({ page })
      .withTags(['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa'])
      .analyze()

    expect(accessibilityScanResults.violations).toEqual([])
  })
})
