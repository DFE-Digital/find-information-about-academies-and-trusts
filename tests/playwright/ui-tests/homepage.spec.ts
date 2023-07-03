import { test, expect } from '@playwright/test'
import { IWireMockRequest, IWireMockResponse, WireMock } from 'wiremock-captain';

test.describe('homepage', () => {

  const mock = new WireMock(process.env.WIREMOCK_BASEURL!);

  test.beforeAll(async () => {
    const request: IWireMockRequest = {
      method: 'GET',
      endpoint: '/v2/trusts'
    };

    const mockedResponse: IWireMockResponse = {
      status: 200,
      body: {
        "Data": [
          {"GroupName": "trust 1"}, 
          {"GroupName": "trust 2"}, 
          {"GroupName": "trust 3"}
        ]
      },
    };

    await mock.register(request, mockedResponse);

  })

  const searchTerms = ['1', 'trust']

  for (const searchTerm of searchTerms) {
    test(`Searching for a trust with "${searchTerm}" navigates to search results page`, async ({ page }) => {
    await page.goto('/')
    await page.getByLabel('Find information about academies and trusts').fill(searchTerm)
    await page.getByRole('button', { name: 'Search'}).click()

    await expect(page.locator('h1')).toHaveText(`Search results for "${searchTerm}"`)
  })
}
})
