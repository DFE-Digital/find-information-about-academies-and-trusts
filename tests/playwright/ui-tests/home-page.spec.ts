import { test } from '@playwright/test'
import { HomePage } from '../page-object-model/home-page'
import { SearchPage } from '../page-object-model/search-page'
import { DetailsPage } from '../page-object-model/trust/details-page'
import { javaScriptContexts } from '../helpers'

test.describe('homepage', () => {
  let homePage: HomePage
  let searchPage: SearchPage
  let detailsPage: DetailsPage

  test.beforeEach(async ({ page }) => {
    homePage = new HomePage(page)
    searchPage = new SearchPage(page)
    detailsPage = new DetailsPage(page)
    await homePage.goTo()
  })

  for (const javaScriptContext of javaScriptContexts) {
    test.describe(`With JavaScript ${javaScriptContext.name}`, () => {
      test.use({ javaScriptEnabled: javaScriptContext.isEnabled })

      const searchTerms = ['trust', 'education']

      for (const searchTerm of searchTerms) {
        test(`Searching for a trust with "${searchTerm}" navigates to search page with results for`, async () => {
          await homePage.searchForm.searchFor(searchTerm)

          await searchPage.expect.toBeOnTheRightPage()
          await searchPage.expect.toBeOnPageWithResultsFor(searchTerm)
        })
      }
    })
  }

  test.describe('Only with JavaScript enabled', () => {
    test.describe('Given a user is typing a search term', () => {
      test('then they should see a list of options and should be able to select one directly', async () => {
        await homePage.searchForm.typeSearchTerm('trust')
        await homePage.searchForm.expect.toShowAllResultsInAutocomplete()
        await homePage.searchForm.chooseItemFromAutocompleteWithText('trust 1')
        await homePage.searchForm.submitSearch()
        await detailsPage.expect.toBeOnTheRightPageFor('trust 1')
      })

      test('then they should be able to change their search term to a free text search after selecting a result', async () => {
        await homePage.searchForm.typeSearchTerm('trust')
        await homePage.searchForm.chooseItemFromAutocompleteWithText('trust 1')
        await homePage.searchForm.typeSearchTerm('education')
        await homePage.searchForm.submitSearch()
        await searchPage.expect.toBeOnPageWithResultsFor('education')
      })
    })
  })
})
