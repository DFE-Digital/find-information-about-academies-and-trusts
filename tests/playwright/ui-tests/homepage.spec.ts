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
        "data": [{
          "ukprn": null, "urn": null, "groupName": "This is the name of a trust",
          "companiesHouseNumber": null, "trustType": null, "trustAddress": { "street": null, "locality": null, "additionalLine": null, "town": null, "county": null, "postcode": null }, "establishments": []
        }]
      },
    };

    await mock.register(request, mockedResponse);

  })

  test('PLACEHOLDER TEST - prove ui tests talk to wiremock', async ({ page }) => {
    await page.goto('/');

    await expect(page.getByTestId('academies-api-response'))
    .toHaveText('{"data":[{"ukprn":null,"urn":null,"groupName":"This is the name of a trust","companiesHouseNumber":n');
  })
})
