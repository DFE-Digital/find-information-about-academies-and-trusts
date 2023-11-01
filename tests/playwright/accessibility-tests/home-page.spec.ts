import { test } from './a11y-test'
import { HomePage } from '../page-object-model/home-page'
import { CurrentSearch } from '../page-object-model/shared/search-form-component'

test.describe('home page', () => {
  let homePage: HomePage

  test.beforeEach(async ({ page }) => {
    homePage = new HomePage(page, new CurrentSearch())
    await homePage.goTo()
  })

  test('should not have any automatically detectable accessibility issues', async ({ expectNoAccessibilityViolations }) => {
    await homePage.expect.toBeOnTheRightPage()

    await expectNoAccessibilityViolations()
  })

  test('when typing a search term and autocomplete is shown', async ({ expectNoAccessibilityViolations }) => {
    await homePage.searchForm.typeASearchTerm()
    await homePage.searchForm.expect.toShowAnySuggestionInAutocomplete()

    await expectNoAccessibilityViolations()
  })
})
