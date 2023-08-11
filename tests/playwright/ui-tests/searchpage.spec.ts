import { test } from '@playwright/test'
import { EndpointFeature, IWireMockFeatures, IWireMockRequest, IWireMockResponse, WireMock } from 'wiremock-captain'
import { HomePage } from '../page-object-model/home-page'
import { SearchPage } from '../page-object-model/search-page'
import { DetailsPage } from '../page-object-model/trust/details-page'

test.describe('homepage', () => {
  let homePage: HomePage
  let searchPage: SearchPage

  const mock = new WireMock(process.env.WIREMOCK_BASEURL ?? 'http://localhost:8080')

  const trustsResponseData = [
    { GroupName: 'trust 1', TrustAddress: { Street: '12 Paddle Road', Locality: 'Bushy Park', AdditionalLine: 'Letworth', Town: 'Manchester', Postcode: 'MX12 P34' }, Ukprn: '123', Establishments: [{ urn: '123' }] },
    { GroupName: 'trust 2', TrustAddress: { Street: '12 Paddle Road', Locality: '', AdditionalLine: '', Town: 'Manchester', Postcode: 'MX12 P34' }, Ukprn: '124', Establishments: [{ rn: '456' }, { urn: '789' }] },
    { GroupName: 'trust 3', TrustAddress: { Street: '', Locality: 'Bushy Park', AdditionalLine: null, Town: 'Manchester', Postcode: '' }, Ukprn: '125', Establishments: [] }
  ]

  test.beforeAll(async () => {
    const request: IWireMockRequest = {
      method: 'GET',
      endpoint: '/v3/trusts'
    }

    const mockedResponse: IWireMockResponse = {
      status: 200,
      body: {
        Data: trustsResponseData
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

  test('Searching for a trust from the homepage displays formatted information about each trust', async () => {
    await searchPage.goToSearchFor('trust')
    await searchPage.expect.toBeOnPageWithResultsFor('trust')

    await searchPage.expect.toShowResultsWithCount(3)

    await searchPage.expect.toShowResultWithText('trust 1', '12 Paddle Road, Bushy Park, Letworth, Manchester, MX12 P34')
    await searchPage.expect.toShowResultWithText('trust 2', '12 Paddle Road, Manchester, MX12 P34')
    await searchPage.expect.toShowResultWithText('trust 3', 'Bushy Park, Manchester')

    await searchPage.expect.toShowResultWithAcademiesinTrustCount('trust 1', 1)
    await searchPage.expect.toShowResultWithAcademiesinTrustCount('trust 1', 2)
    await searchPage.expect.toShowResultWithAcademiesinTrustCount('trust 1', 0)
  })

  for (const trustResponse of trustsResponseData) {
    const trustName = trustResponse.GroupName

    test(`Clicking on a result navigates to the details page for ${trustName}`, async ({ page }) => {
      const detailsPage = new DetailsPage(page)
      const trustRequest: IWireMockRequest = {
        method: 'GET',
        endpoint: `/v3/trust/${trustResponse.Ukprn}`
      }

      const mockedTrustResponse = {
        status: 200,
        body: {
          Data: {
            GiasData: {
              GroupName: trustName,
              GroupContactAddress: {
                Street: '12 Abbey Road',
                Locality: 'Dorthy Inlet',
                AdditionalLine: 'East Park',
                Town: 'Kingston upon Hull',
                County: 'East Riding of Yorkshire',
                Postcode: 'JY36 9VC'
              }
            },
            ukprn: trustResponse.Ukprn,
            Establishments: []
          }
        }
      }

      await mock.register(trustRequest, mockedTrustResponse)

      await searchPage.goToSearchFor('trust')
      await searchPage.expect.toBeOnPageWithResultsFor('trust')

      await searchPage.clickOnSearchResultLinkWithText(trustName)
      await detailsPage.expect.toBeOnTheRightPageFor(trustName)
    })
  }
})
