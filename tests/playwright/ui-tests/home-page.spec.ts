import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'
import { SearchPage } from '../page-object-model/search-page'
import { DetailsPage } from '../page-object-model/trust/details-page'
import { javaScriptContexts } from '../helpers'
import { CurrentSearch } from '../page-object-model/shared/search-form-component'
import { FakeTestData } from '../fake-data/fake-test-data'

test.describe('homepage', () => {
  let homePage: HomePage
  let searchPage: SearchPage
  let detailsPage: DetailsPage
  let currentSearch: CurrentSearch

  test.beforeEach(async ({ page }) => {
    currentSearch = new CurrentSearch()
    const fakeTestData = new FakeTestData()
    homePage = new HomePage(page, currentSearch)
    searchPage = new SearchPage(page, currentSearch, fakeTestData)
    detailsPage = new DetailsPage(page, fakeTestData)
    await homePage.goTo()
  })

  for (const javaScriptContext of javaScriptContexts) {
    test.describe(`With JavaScript ${javaScriptContext.name}`, () => {
      test.use({ javaScriptEnabled: javaScriptContext.isEnabled })

      test('Searching for different terms navigates to search page with different results', async ({ browserName }) => {
        test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')
        await homePage.searchForm.searchForATrust()
        await searchPage.expect.toBeOnTheRightPage()
        await searchPage.expect.toBeOnPageWithMatchingResults()

        await homePage.goTo()
        await homePage.searchForm.searchForADifferentTrust()
        await searchPage.expect.toBeOnTheRightPage()
        await searchPage.expect.toBeOnPageWithMatchingResults()
      })
    })
  }

  test.describe('Only with JavaScript enabled', () => {
    test.describe('Given a user is typing a search term', () => {
      test('then they should see a list of options and should be able to select one directly', async ({ browserName }) => {
        test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')
        await homePage.searchForm.typeASearchTerm()
        await homePage.searchForm.chooseItemFromAutocomplete()
        await homePage.searchForm.submitSearch()
        await detailsPage.expect.toBeOnTheRightPageFor(currentSearch.selectedTrustName)
      })

      test('then they should be able to change their search term to a free text search after selecting a result', async ({ browserName }) => {
        test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')
        await homePage.searchForm.typeASearchTerm()
        await homePage.searchForm.chooseItemFromAutocomplete()
        await homePage.searchForm.typeADifferentSearchTerm()
        await homePage.searchForm.submitSearch()
        await searchPage.expect.toBeOnPageWithMatchingResults()
      })
    })
  })
})
