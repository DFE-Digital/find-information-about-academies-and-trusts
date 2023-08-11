import { test } from '@playwright/test'
import { EndpointFeature, IWireMockFeatures, IWireMockRequest, IWireMockResponse, WireMock } from 'wiremock-captain'
import { HomePage } from '../page-object-model/home-page'
import { SearchPage } from '../page-object-model/search-page'

test.describe('homepage', () => {
  const mock = new WireMock(process.env.WIREMOCK_BASEURL ?? 'http://localhost:8080')
  let homePage: HomePage
  let searchPage: SearchPage

  test.beforeAll(async () => {
    const request: IWireMockRequest = {
      method: 'GET',
      endpoint: '/v3/trusts'
    }

    const mockedResponse: IWireMockResponse = {
      status: 200,
      body: {
        Data: [
          { GroupName: 'trust 1', TrustAddress: { Street: null, Locality: null, AdditionalLine: null, Town: null, County: null, Postcode: null }, Ukprn: '123', Establishments: [] },
          { GroupName: 'trust 2', TrustAddress: { Street: null, Locality: null, AdditionalLine: null, Town: null, County: null, Postcode: null }, Ukprn: '124', Establishments: [] },
          { GroupName: 'trust 3', TrustAddress: { Street: null, Locality: null, AdditionalLine: null, Town: null, County: null, Postcode: null }, Ukprn: '125', Establishments: [] }
        ]
      }
    }

    const features: IWireMockFeatures = { requestEndpointFeature: EndpointFeature.UrlPath }

    await mock.register(request, mockedResponse, features)
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

      await searchPage.expect.toBeOnTheRightPage()
      await searchPage.expect.toBeOnPageWithResultsFor(searchTerm)
    })
  }
})
