import { test, expect } from '@playwright/test'
import AxeBuilder from '@axe-core/playwright'
import { HomePage } from '../page-object-model/home-page'

test.describe('home page', () => {
  let homePage: HomePage

  test.beforeEach(async ({ page }) => {
    homePage = new HomePage(page)
    await homePage.goTo()
  })

  test('should not have any automatically detectable accessibility issues', async ({ page }) => {
    await homePage.expect.toBeOnTheRightPage()
    const accessibilityScanResults = await new AxeBuilder({ page })
      .withTags(['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa'])
      .analyze()

    expect(accessibilityScanResults.violations).toEqual([])
  })

  test('when typing a search term and autocomplete is shown', async ({ page }) => {
    await homePage.searchForm.typeSearchTerm('trust')
    await homePage.searchForm.expect.toShowAllResultsInAutocomplete()

    const accessibilityScanResults = await new AxeBuilder({ page })
      .withTags(['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa'])
      .analyze()

    expect(accessibilityScanResults.violations).toEqual([])
  })
})
