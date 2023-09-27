import { test } from './a11y-test'
import { HomePage } from '../page-object-model/home-page'

test.describe('home page', () => {
  let homePage: HomePage

  test.beforeEach(async ({ page }) => {
    homePage = new HomePage(page)
    await homePage.goTo()
  })

  test('should not have any automatically detectable accessibility issues', async ({ expectNoAccessibilityViolations }) => {
    await homePage.expect.toBeOnTheRightPage()

    await expectNoAccessibilityViolations()
  })

  test('when typing a search term and autocomplete is shown', async ({ expectNoAccessibilityViolations }) => {
    await homePage.searchForm.typeSearchTerm('trust')
    await homePage.searchForm.expect.toShowAllResultsInAutocomplete()

    await expectNoAccessibilityViolations()
  })
})
