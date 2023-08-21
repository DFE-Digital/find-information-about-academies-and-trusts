import { test } from '@playwright/test'
import { SearchPage } from '../page-object-model/search-page'
import { DetailsPage } from '../page-object-model/trust/details-page'
import { MockTrustsProvider } from '../mocks/mock-trusts-provider'

test.describe('Search page', () => {
  let searchPage: SearchPage
  let detailsPage: DetailsPage
  let searchTerm: string

  test.beforeEach(async ({ page }) => {
    searchPage = new SearchPage(page)
    detailsPage = new DetailsPage(page)
  })

  test.describe('Given a user searches for a term that returns results', () => {
    test.beforeEach(async () => {
      searchTerm = 'trust'
      await searchPage.goToSearchFor(searchTerm)
      await searchPage.expect.toBeOnPageWithResultsFor(searchTerm)
    })

    test('then it displays a list of results with information about each trust', async () => {
      await searchPage.expect.toDisplayNumberOfResultsFound()
      await searchPage.expect.toSeeInformationForEachResult()
    })

    test('the user can edit their search and search again', async () => {
      await searchPage.expect.toSeeSearchInputContainingSearchTerm(searchTerm)
      await searchPage.searchForTerm('education')
      await searchPage.expect.toBeOnPageWithResultsFor('education')
      await searchPage.expect.toSeeSearchInputContainingSearchTerm('education')
    })

    for (const trustResponse of MockTrustsProvider.fakeTrustsResponseData) {
      const trustName = trustResponse.GroupName

      test(`when the user clicks on "${trustName}" then it navigates to the details page`, async () => {
        await searchPage.clickOnSearchResultLinkWithText(trustName)
        await detailsPage.expect.toBeOnTheRightPageFor(trustName)
      })
    }
  })
})
