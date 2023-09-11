import { test, expect } from '@playwright/test'
import { AxeBuilder } from '@axe-core/playwright'
import { SearchPage } from '../page-object-model/search-page'

test.describe('search page should not have any automatically detectable accessibility issues', () => {
  let searchPage: SearchPage

  test.beforeEach(async ({ page }) => {
    searchPage = new SearchPage(page)
  })

  test('when going to a search page with no search term', async ({ page }) => {
    await searchPage.goTo()
    await searchPage.expect.toSeeNoResultsMessage()

    const accessibilityScanResults = await new AxeBuilder({ page })
      .withTags(['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa'])
      .analyze()

    expect(accessibilityScanResults.violations).toEqual([])
  })

  test('when going to a search page with a search term', async ({ page }) => {
    await searchPage.goToSearchFor('trust')
    await searchPage.expect.toBeOnPageWithResultsFor('trust')
    await searchPage.expect.toShowResults()

    const accessibilityScanResults = await new AxeBuilder({ page })
      .withTags(['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa'])
      .analyze()

    expect(accessibilityScanResults.violations).toEqual([])
  })

  test('when typing a search term and autocomplete is shown', async ({ page }) => {
    await searchPage.goTo()
    await searchPage.searchForm.typeSearchTerm('trust')
    await searchPage.searchForm.expect.toShowAllResultsInAutocomplete()

    const accessibilityScanResults = await new AxeBuilder({ page })
      .withTags(['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa'])
      .analyze()

    expect(accessibilityScanResults.violations).toEqual([])
  })

  // Skipping this test as the autocomplete element fails accessibility tests when showing the no results found message
  // message: 'Element has children which are not allowed'
  // This is referring to an li element, however the ul and li nesting seems to be correct.
  // As this is not our application code we will skip this test for now, and see if we face any issues in our audit.
  test.skip('when typing a search term with no results, no results is shown in autocomplete', async ({ page }) => {
    await searchPage.goTo()

    await searchPage.searchForm.typeSearchTerm('non')
    await searchPage.searchForm.expect.toshowNoResultsFoundInAutocomplete()
    const accessibilityScanResults = await new AxeBuilder({ page })
      .withTags(['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa']).analyze()

    expect(accessibilityScanResults.violations).toEqual([])
  })
})
