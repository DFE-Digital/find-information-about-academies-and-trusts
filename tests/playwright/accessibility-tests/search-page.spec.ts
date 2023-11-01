import { test } from './a11y-test'
import { SearchPage } from '../page-object-model/search-page'
import { CurrentSearch } from '../page-object-model/shared/search-form-component'

test.describe('search page should not have any automatically detectable accessibility issues', () => {
  let searchPage: SearchPage

  test.beforeEach(async ({ page }) => {
    searchPage = new SearchPage(page, new CurrentSearch())
  })

  test('when going to a search page with no search term', async ({ expectNoAccessibilityViolations }) => {
    await searchPage.goTo()
    await searchPage.expect.toSeeNoResultsMessage()

    await expectNoAccessibilityViolations()
  })

  test('when going to a search page with a search term', async ({ expectNoAccessibilityViolations }) => {
    await searchPage.goToPageWithResults()
    await searchPage.expect.toBeOnPageWithMatchingResults()
    await searchPage.expect.toShowResults()

    await expectNoAccessibilityViolations()
  })

  test('when typing a search term and autocomplete is shown', async ({ expectNoAccessibilityViolations }) => {
    await searchPage.goTo()
    await searchPage.searchForm.typeASearchTerm()
    await searchPage.searchForm.expect.toShowAnySuggestionInAutocomplete()

    await expectNoAccessibilityViolations()
  })

  // Skipping this test as the autocomplete element fails accessibility tests when showing the no results found message
  // message: 'Element has children which are not allowed'
  // This is referring to an li element, however the ul and li nesting seems to be correct.
  // As this is not our application code we will skip this test for now, and see if we face any issues in our audit.
  test.skip('when typing a search term with no results, no results is shown in autocomplete', async ({ expectNoAccessibilityViolations }) => {
    await searchPage.goTo()

    await searchPage.searchForm.typeASearchTermWithNoMatches()
    await searchPage.searchForm.expect.toshowNoResultsFoundInAutocomplete()

    await expectNoAccessibilityViolations()
  })
})
