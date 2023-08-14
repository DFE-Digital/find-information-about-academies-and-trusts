import { EndpointFeature, IWireMockFeatures, IWireMockRequest, IWireMockResponse, WireMock } from 'wiremock-captain'

export class MockTrustsProvider {
  readonly trustsResponseData = [
    { GroupName: 'trust 1', TrustAddress: { Street: '12 Paddle Road', Locality: 'Bushy Park', AdditionalLine: 'Letworth', Town: 'Manchester', Postcode: 'MX12 P34' }, Ukprn: '123', Establishments: [{ urn: '123' }] },
    { GroupName: 'trust 2', TrustAddress: { Street: '12 Paddle Road', Locality: '', AdditionalLine: '', Town: 'Manchester', Postcode: 'MX12 P34' }, Ukprn: '124', Establishments: [{ rn: '456' }, { urn: '789' }] },
    { GroupName: 'trust 3', TrustAddress: { Street: '', Locality: 'Bushy Park', AdditionalLine: null, Town: 'Manchester', Postcode: '' }, Ukprn: '125', Establishments: [] }
  ]

  registerEndpoint = async (): Promise<void> => {
    const mock = new WireMock(process.env.WIREMOCK_BASEURL ?? 'http://localhost:8080')
    const request: IWireMockRequest = {
      method: 'GET',
      endpoint: '/v3/trusts'
    }

    const mockedResponse: IWireMockResponse = {
      status: 200,
      body: {
        Data: this.trustsResponseData
      }
    }

    const features: IWireMockFeatures = { requestEndpointFeature: EndpointFeature.UrlPath }

    await mock.register(request, mockedResponse, features)
  }
}
