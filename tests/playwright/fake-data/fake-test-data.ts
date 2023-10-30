import * as testDataJson from '../fake-data/trusts.json'

export interface FakeTrust {
  name: string
  address: string
  uid: string
  groupId: string
}

export class FakeTestData {
  _fakeTrusts: FakeTrust[]
  constructor () {
    this._fakeTrusts = JSON.parse(JSON.stringify(testDataJson)).default
  }

  getTrustByUid (uid: string): FakeTrust | undefined {
    return this._fakeTrusts.find(result => result.uid === uid)
  }

  getNumberOfTrustsWithNameMatching (searchTerm: string): number {
    return this._fakeTrusts.filter(trust => trust.name?.toLowerCase().includes(searchTerm.toLowerCase())).length
  }
}
