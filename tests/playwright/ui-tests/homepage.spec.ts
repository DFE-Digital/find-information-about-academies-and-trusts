import { test } from '@playwright/test'
import { IWireMockRequest, IWireMockResponse, WireMock } from 'wiremock-captain'
import { HomePage } from '../page-object-model/home-page'
import { SearchPage } from '../page-object-model/search-page'

test.describe('homepage', () => {
  const mock = new WireMock(process.env.WIREMOCK_BASEURL ?? 'http://localhost:8080')
  let homePage: HomePage
  let searchPage: SearchPage

  test.beforeAll(async () => {
    const request: IWireMockRequest = {
      method: 'GET',
      endpoint: '/v2/trusts'
    }

    const mockedResponse: IWireMockResponse = {
      status: 200,
      body: {
        Data: [
          { GroupName: 'trust 1' },
          { GroupName: 'trust 2' },
          { GroupName: 'trust 3' }
        ]
      }
    }

    await mock.register(request, mockedResponse)
  })

  test.beforeEach(async ({ page }) => {
    homePage = new HomePage(page)
    searchPage = new SearchPage(page)
    await homePage.goTo()
  })

  const searchTerms = ['1', 'trust']

  for (const searchTerm of searchTerms) {
    test(`Searching for a trust with "${searchTerm}" navigates to search page with results for`, async () => {
      await homePage.searchFor(searchTerm)

      await searchPage.expect.toBeOnPageWithResultsFor(searchTerm)
    })
  }
})
