import { test, expect } from '@playwright/test'
import { AxeBuilder } from '@axe-core/playwright'
import { SearchPage } from '../page-object-model/search-page'
import { MockTrustsProvider } from '../mocks/mock-trusts-provider'

test.describe('search page should not have any automatically detectable accessibility issues', () => {
  test('when going to a search page with no search term', async ({ page }) => {
    const searchPage = new SearchPage(page)
    await searchPage.goTo()

    const accessibilityScanResults = await new AxeBuilder({ page })
      .withTags(['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa'])
      .analyze()

    expect(accessibilityScanResults.violations).toEqual([])
  })

  test('when going to a search page with a search term', async ({ page }) => {
    const mockTrustsProvider = new MockTrustsProvider()
    await mockTrustsProvider.registerGetTrusts()

    const searchPage = new SearchPage(page)
    await searchPage.goToSearchFor('edu')
    await searchPage.expect.toBeOnPageWithResultsFor('edu')

    const accessibilityScanResults = await new AxeBuilder({ page })
      .withTags(['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa'])
      .analyze()

    expect(accessibilityScanResults.violations).toEqual([])
  })
})
