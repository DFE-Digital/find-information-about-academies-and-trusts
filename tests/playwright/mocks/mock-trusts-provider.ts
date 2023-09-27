import { EndpointFeature, IWireMockFeatures, IWireMockRequest, IWireMockResponse, WireMock } from 'wiremock-captain'

export class MockTrustsProvider {
  _mock: WireMock
  static readonly fakeTrustsResponseData = {
    trust: [
      { GroupName: 'trust 1', TrustAddress: { Street: '12 Paddle Road', Locality: 'Bushy Park', AdditionalLine: 'Letworth', Town: 'Manchester', Postcode: 'MX12 P34' }, Ukprn: '123', Establishments: [{ urn: '123' }] },
      { GroupName: 'trust 2', TrustAddress: { Street: '12 Paddle Road', Locality: '', AdditionalLine: '', Town: 'Manchester', Postcode: 'MX12 P34' }, Ukprn: '124', Establishments: [{ urn: '456' }, { urn: '789' }] },
      { GroupName: 'trust 3', TrustAddress: { Street: '', Locality: 'Bushy Park', AdditionalLine: null, Town: 'Manchester', Postcode: '' }, Ukprn: '125', Establishments: [] }
    ],
    education: [
      { GroupName: 'Abbey Education', TrustAddress: { Street: '13 Paddle Road', Locality: 'Bushy Park', AdditionalLine: 'Letworth', Town: 'Liverpool', Postcode: 'MX12 P34' }, Ukprn: '175', Establishments: [{ urn: '123' }] }
    ],
    non: []
  }

  static expectedFormattedTrustResult = {
    name: 'Abbey Education',
    ukprn: '175',
    type: 'Multi-academy trust'
  }

  static nonExistingTrustUkprn = '1111'

  constructor () {
    this._mock = new WireMock(process.env.WIREMOCK_BASEURL ?? 'http://localhost:8080')
  }

  registerGetTrustsBy = async (keywords: string): Promise<void> => {
    const request: IWireMockRequest = {
      method: 'GET',
      endpoint: '/v3/trusts',
      queryParameters: {
        groupName: keywords
      }
    }

    const mockedResponse: IWireMockResponse = {
      status: 200,
      body: {
        Data: MockTrustsProvider.fakeTrustsResponseData[keywords]
      }
    }

    const features: IWireMockFeatures = { requestEndpointFeature: EndpointFeature.UrlPath }

    await this._mock.register(request, mockedResponse, features)
  }

  registerGetTrustByUkprn = async (groupName: string, ukprn: string): Promise<void> => {
    const trustRequest: IWireMockRequest = {
      method: 'GET',
      endpoint: `/v3/trust/${ukprn}`
    }

    const mockedTrustResponse = {
      status: 200,
      body: {
        Data: {
          GiasData: {
            groupName,
            GroupContactAddress: {
              Street: '12 Abbey Road',
              Locality: 'Dorthy Inlet',
              AdditionalLine: 'East Park',
              Town: 'Kingston upon Hull',
              County: 'East Riding of Yorkshire',
              Postcode: 'JY36 9VC'
            },
            groupType: 'Multi-academy trust',
            ukprn
          },
          Establishments: []
        }
      }
    }

    await this._mock.register(trustRequest, mockedTrustResponse)
  }

  registerGetTrustNotFoundResponse = async (): Promise<void> => {
    const trustRequest: IWireMockRequest = {
      method: 'GET',
      endpoint: `/v3/trust/${MockTrustsProvider.nonExistingTrustUkprn}`
    }

    const mockedResponse: IWireMockResponse = {
      status: 404
    }

    await this._mock.register(trustRequest, mockedResponse)
  }
}
