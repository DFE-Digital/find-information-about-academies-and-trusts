import { test } from '@playwright/test'
import { SearchPage } from '../page-object-model/search-page'
import { DetailsPage } from '../page-object-model/trust/details-page'
import { javaScriptContexts } from '../helpers'
import { CurrentSearch } from '../page-object-model/shared/search-form-component'
import { FakeTestData } from '../fake-data/fake-test-data'

test.describe('Search page', () => {
  let searchPage: SearchPage
  let detailsPage: DetailsPage
  let currentSearch: CurrentSearch

  test.beforeEach(async ({ page }) => {
    currentSearch = new CurrentSearch()
    const fakeTestData = new FakeTestData()
    searchPage = new SearchPage(page, currentSearch, fakeTestData)
    detailsPage = new DetailsPage(page, fakeTestData)
  })

  for (const javaScriptContext of javaScriptContexts) {
    test.describe(`With JavaScript ${javaScriptContext.name}`, () => {
      test.use({ javaScriptEnabled: javaScriptContext.isEnabled })

      test.describe('Given a user goes to straight to the search page', () => {
        test('then they see an empty search input and can search by a new term', async () => {
          await searchPage.goTo()
          await searchPage.searchForm.expect.inputToContainNoSearchTerm()
          await searchPage.expect.toSeeNoResultsMessage()
          await searchPage.searchForm.searchForATrust()
          await searchPage.expect.toSeeInformationForEachResult()
        })
      })

      test.describe('Given a user searches for a term that returns results', () => {
        test.beforeEach(async () => {
          await searchPage.goToSearchWithResults()
          await searchPage.expect.toBeOnPageWithMatchingResults()
        })

        test('then it displays a list of results with information about each trust', async () => {
          await searchPage.expect.toDisplayTotalNumberOfResultsFound()
          await searchPage.expect.toSeeInformationForEachResult()
        })

        test('the user can edit their search and search again', async ({ browserName }) => {
          test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')
          await searchPage.searchForm.expect.inputToContainSearchTerm()
          await searchPage.searchForm.searchForATrust()
          await searchPage.expect.toBeOnPageWithMatchingResults()
          await searchPage.searchForm.searchForADifferentTrust()
          await searchPage.searchForm.expect.inputToContainSearchTerm()
          await searchPage.expect.toBeOnPageWithMatchingResults()
          await searchPage.expect.toSeeInformationForEachResult()
        })

        test('when the user clicks on different results they are taken to different trust details pages', async () => {
          await searchPage.clickOnSearchResultLink(1)
          await detailsPage.expect.toBeOnTheRightPageFor(currentSearch.selectedTrustName)

          await searchPage.goToSearchWithResults()
          await searchPage.clickOnSearchResultLink(2)
          await detailsPage.expect.toBeOnTheRightPageFor(currentSearch.selectedTrustName)
        })
      })

      test.describe('given that there are multiple pages of results', () => {
        test.beforeEach(async () => {
          await searchPage.goToSearchWithManyPagesOfResults()
          await searchPage.expect.toBeOnPageWithMatchingResults()
          await searchPage.pagination.expect.toBeOnSpecificPage(1)
        })

        test('the correct number of results are returned', async () => {
          await searchPage.expect.toDisplayTotalNumberOfResultsFound()
          await searchPage.expect.toSeeInformationForEachResult()
        })

        test('the next page link is visible when there is another page', async () => {
          await searchPage.pagination.expect.toShowNextPageLink()
          await searchPage.pagination.selectPage(4)
          await searchPage.pagination.expect.toBeOnSpecificPage(4)
          await searchPage.pagination.expect.toNotShowNextPageLink()
        })

        test('the previous page link is visible when there is another page', async () => {
          await searchPage.pagination.expect.toNotShowPreviousPageLink()
          await searchPage.pagination.selectNextPage()
          await searchPage.pagination.expect.toBeOnSpecificPage(2)
          await searchPage.pagination.expect.toShowPreviousPageLink()
        })
      })

      test.describe('given that there is only 1 page of results', () => {
        test.beforeEach(async () => {
          await searchPage.goToSearchWithOnePageOfResults()
        })

        test('the correct number of results are returned', async () => {
          await searchPage.expect.toDisplayTotalNumberOfResultsFound()
          await searchPage.expect.toSeeInformationForEachResult()
        })

        test('the next and previous page links are not visible', async () => {
          await searchPage.pagination.expect.toBeOnSpecificPage(1)
          await searchPage.pagination.expect.toNotShowNextPageLink()
          await searchPage.pagination.expect.toNotShowPreviousPageLink()
        })
      })

      test.describe('Given a user searches for a term that returns no results', () => {
        test('then they can see an input containing the search term so they can edit it', async () => {
          await searchPage.goToSearchWithNoResults()
          await searchPage.searchForm.expect.inputToContainSearchTerm()
        })

        test('they see a helpful message to help them change their search', async () => {
          await searchPage.goToSearchWithNoResults()
          await searchPage.expect.toSeeNoResultsMessage()
        })
      })
    })
  }

  test.describe('Only with JavaScript enabled', () => {
    test.describe('Given a user searches for a term that returns results', () => {
      test.beforeEach(async () => {
        await searchPage.goToSearchWithResults()
        await searchPage.expect.toBeOnPageWithMatchingResults()
      })

      test('the user can edit their search and select an item using autocomplete', async ({ browserName }) => {
        test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')
        await searchPage.searchForm.expect.inputToContainSearchTerm()
        await searchPage.searchForm.typeASearchTerm()
        await searchPage.searchForm.chooseItemFromAutocomplete()
        await searchPage.searchForm.submitSearch()
        await detailsPage.expect.toBeOnTheRightPageFor(currentSearch.selectedTrustName)
      })
    })

    test.describe('Given a user is typing a search term', () => {
      test.beforeEach(async () => {
        await searchPage.goTo()
      })

      test('then they should see a list of options and should be able to select one directly', async ({ browserName }) => {
        test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')
        await searchPage.searchForm.typeASearchTerm()
        await searchPage.searchForm.chooseItemFromAutocomplete()
        await searchPage.searchForm.submitSearch()
        await detailsPage.expect.toBeOnTheRightPageFor(currentSearch.selectedTrustName)
      })

      test('then they should be able to change their search term to a free text search after selecting a result', async ({ browserName }) => {
        test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')
        await searchPage.searchForm.typeASearchTerm()
        await searchPage.searchForm.chooseItemFromAutocomplete()
        await searchPage.searchForm.typeADifferentSearchTerm()
        await searchPage.searchForm.submitSearch()
        await searchPage.expect.toBeOnPageWithMatchingResults()
        await searchPage.expect.toSeeInformationForEachResult()
      })

      test('then they should be able to change their selection after clicking a result', async ({ browserName }) => {
        test.skip(browserName === 'webkit', 'Failing due to issues with setting cookies for POST request https://github.com/microsoft/playwright/issues/5236')
        await searchPage.searchForm.typeASearchTerm()
        await searchPage.searchForm.chooseItemFromAutocomplete()
        await searchPage.searchForm.typeADifferentSearchTerm()
        await searchPage.searchForm.chooseItemFromAutocomplete()
        await searchPage.searchForm.submitSearch()
        await detailsPage.expect.toBeOnTheRightPageFor(currentSearch.selectedTrustName)
      })

      test('then they should see no results message if there are no matching trusts', async () => {
        await searchPage.searchForm.typeASearchTermWithNoMatches()
        await searchPage.searchForm.expect.toshowNoResultsFoundInAutocomplete()
      })
    })
  })
})
