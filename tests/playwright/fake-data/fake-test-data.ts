import * as testDataJson from '../fake-data/trusts.json'

export interface FakeTrust {
  name: string
  address: string
  uid: string
  groupId: string
  type: string
}

export class FakeTestData {
  _fakeTrusts: FakeTrust[]
  constructor () {
    this._fakeTrusts = JSON.parse(JSON.stringify(testDataJson)).default
  }

  getTrustByUid (uid: string): FakeTrust {
    const fakeTrust = this._fakeTrusts.find(result => result.uid === uid)
    if (fakeTrust === undefined) {
      throw new Error(`Expected to find trust in the test data containing UID: ${uid}, but it was not found`)
    }
    return fakeTrust
  }

  getNumberOfTrustsWithNameMatching (searchTerm: string): number {
    return this._fakeTrusts.filter(trust => trust.name?.toLowerCase().includes(searchTerm.toLowerCase())).length
  }

  getFirstTrust (): FakeTrust {
    return this._fakeTrusts[0]
  }

  getMultiAcademyTrust (): FakeTrust {
    return this.getTrustWithType('Multi-academy trust')
  }

  getSingleAcademyTrust (): FakeTrust {
    return this.getTrustWithType('Single-academy trust')
  }

  private getTrustWithType (type: string): FakeTrust {
    const trust = this._fakeTrusts.find(trust => trust.type === type)
    if (trust === undefined) {
      throw new Error(`No trusts with type ${type} found in test data`)
    }
    return trust
  }
}
