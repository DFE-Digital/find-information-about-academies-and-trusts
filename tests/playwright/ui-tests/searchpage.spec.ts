import { test } from '@playwright/test'
import { SearchPage } from '../page-object-model/search-page'
import { DetailsPage } from '../page-object-model/trust/details-page'
import { MockTrustsProvider } from '../mocks/mock-trusts-provider'

test.describe('Search page', () => {
  let searchPage: SearchPage
  let detailsPage: DetailsPage
  const searchTerm = 'trust'

  test.beforeEach(async ({ page }) => {
    searchPage = new SearchPage(page)
    detailsPage = new DetailsPage(page)
  })

  test.describe('Given a user goes to straight to the search page', () => {
    test('then they see an empty search input and can search by a new term', async () => {
      await searchPage.goTo()
      await searchPage.expect.toSeeSearchInputContainingNoSearchTerm()
      await searchPage.expect.toSeeNoResultsMessage()
      await searchPage.searchFor(searchTerm)
      await searchPage.expect.toSeeInformationForEachResult()
    })
  })

  test.describe('Given a user searches for a term that returns results', () => {
    test.beforeEach(async () => {
      await searchPage.goToSearchFor(searchTerm)
      await searchPage.expect.toBeOnPageWithResultsFor(searchTerm)
    })

    test('then it displays a list of results with information about each trust', async () => {
      await searchPage.expect.toDisplayNumberOfResultsFound()
      await searchPage.expect.toSeeInformationForEachResult()
    })

    test('the user can edit their search and search again', async () => {
      await searchPage.expect.toSeeSearchInputContainingSearchTerm()
      await searchPage.searchFor('education')
      await searchPage.expect.toBeOnPageWithResultsFor('education')
      await searchPage.expect.toSeeSearchInputContainingSearchTerm()
      await searchPage.expect.toSeeInformationForEachResult()
    })

    test('the user can edit their search and select an item using autocomplete', async () => {
      await searchPage.expect.toSeeSearchInputContainingSearchTerm()
      await searchPage.typeSearchTerm('education')
      await searchPage.chooseItemFromAutocompleteWithText('Abbey Education')
      await searchPage.submitSearch()
      await detailsPage.expect.toBeOnTheRightPageFor('Abbey Education')
    })

    for (const trustResponse of MockTrustsProvider.fakeTrustsResponseData[searchTerm]) {
      const trustName = trustResponse.GroupName

      test(`when the user clicks on "${trustName}" then it navigates to the details page`, async () => {
        await searchPage.clickOnSearchResultLinkWithText(trustName)
        await detailsPage.expect.toBeOnTheRightPageFor(trustName)
      })
    }
  })

  test.describe('Given a user is typing a search term', () => {
    test.beforeEach(async () => {
      await searchPage.goTo()
    })

    test('then they should see a list of options and should be able to select one directly', async () => {
      await searchPage.typeSearchTerm(searchTerm)
      await searchPage.chooseItemFromAutocompleteWithText('trust 1')
      await searchPage.submitSearch()
      await detailsPage.expect.toBeOnTheRightPageFor('trust 1')
    })

    test('then they should be able to change their search term to a free text search after selecting a result', async () => {
      await searchPage.typeSearchTerm(searchTerm)
      await searchPage.chooseItemFromAutocompleteWithText('trust 1')
      await searchPage.typeSearchTerm('education')
      await searchPage.submitSearch()
      await searchPage.expect.toBeOnPageWithResultsFor('education')
      await searchPage.expect.toSeeInformationForEachResult()
    })

    test('then they should be able to change their selection after clicking a result', async () => {
      await searchPage.typeSearchTerm(searchTerm)
      await searchPage.chooseItemFromAutocompleteWithText('trust 1')
      await searchPage.typeSearchTerm('education')
      await searchPage.chooseItemFromAutocompleteWithText('Abbey Education')
      await searchPage.submitSearch()
      await detailsPage.expect.toBeOnTheRightPageFor('Abbey Education')
    })

    test('then they should see no results message if there are no matching trusts', async () => {
      await searchPage.typeSearchTerm('non')
      await searchPage.expect.toshowNoResultsFoundInAutocomplete()
    })
  })

  test.describe('Given a user searches for a term that returns no results', () => {
    test('then they can see an input containing the search term so they can edit it', async () => {
      await searchPage.goToSearchFor('non')
      await searchPage.expect.toSeeSearchInputContainingSearchTerm()
    })

    test('they see a helpful message to help them change their search', async () => {
      await searchPage.goToSearchFor('non')
      await searchPage.expect.toSeeNoResultsMessage()
    })
  })
})
