import { EndpointFeature, IWireMockFeatures, IWireMockRequest, IWireMockResponse, WireMock } from 'wiremock-captain'

export class MockTrustsProvider {
  _mock: WireMock
  static readonly fakeTrustsResponseData = [
    { GroupName: 'trust 1', TrustAddress: { Street: '12 Paddle Road', Locality: 'Bushy Park', AdditionalLine: 'Letworth', Town: 'Manchester', Postcode: 'MX12 P34' }, Ukprn: '123', Establishments: [{ urn: '123' }] },
    { GroupName: 'trust 2', TrustAddress: { Street: '12 Paddle Road', Locality: '', AdditionalLine: '', Town: 'Manchester', Postcode: 'MX12 P34' }, Ukprn: '124', Establishments: [{ urn: '456' }, { urn: '789' }] },
    { GroupName: 'trust 3', TrustAddress: { Street: '', Locality: 'Bushy Park', AdditionalLine: null, Town: 'Manchester', Postcode: '' }, Ukprn: '125', Establishments: [] }
  ]

  constructor () {
    this._mock = new WireMock(process.env.WIREMOCK_BASEURL ?? 'http://localhost:8080')
  }

  registerGetTrusts = async (): Promise<void> => {
    const request: IWireMockRequest = {
      method: 'GET',
      endpoint: '/v3/trusts'
    }

    const mockedResponse: IWireMockResponse = {
      status: 200,
      body: {
        Data: MockTrustsProvider.fakeTrustsResponseData
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
            }
          },
          ukprn,
          Establishments: []
        }
      }
    }

    await this._mock.register(trustRequest, mockedTrustResponse)
  }
}
