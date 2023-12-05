import * as testDataJson from '../fake-data/trusts.json'

export interface FakeTrust {
  name: string
  address: string
  uid: string
  groupId: string
  type: string
  ukprn: string
  openedDate: string
  companiesHouseNumber: string
  regionAndTerritory: string
  status: string
  academies: FakeAcademy[]
}

export interface FakeAcademy {
  urn: number
  establishmentName: string
  typeOfEstablishment: string
  localAuthority: string
  urbanRural: string
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

  getOpenMultiAcademyTrust (): FakeTrust {
    return this.getTrustWithType('Multi-academy trust')
  }

  getOpenSingleAcademyTrust (): FakeTrust {
    return this.getTrustWithType('Single-academy trust')
  }

  getOpenSingleAcademyTrustWithNoAcademies (): FakeTrust {
    const trust = this._fakeTrusts.find(trust => trust.type === 'Single-academy trust' && trust.status === 'Open' && trust.academies.length === 0)
    if (trust === undefined) {
      throw new Error('No trusts with type Single-academy trust and no academies exists in test data')
    }
    return trust
  }

  getOpenSingleAcademyTrustWithAcademies (): FakeTrust {
    const trust = this._fakeTrusts.find(trust => trust.type === 'Single-academy trust' && trust.status === 'Open' && trust.academies.length > 0)
    if (trust === undefined) {
      throw new Error('No trusts with type Single-academy trust which has academies exists in test data')
    }
    return trust
  }

  getClosedTrust (): FakeTrust {
    const trust = this._fakeTrusts.find(trust => trust.status === 'Closed')
    if (trust === undefined) {
      throw new Error('No trusts with closed status found in test data')
    }
    return trust
  }

  private getTrustWithType (type: string, status = 'Open'): FakeTrust {
    const trust = this._fakeTrusts.find(trust => trust.type === type && trust.status === status)
    if (trust === undefined) {
      throw new Error(`No trusts with type ${type} found in test data`)
    }
    return trust
  }

  static getTrustAcademyByUrn (trust: FakeTrust, urn: number): FakeAcademy {
    const academy = trust.academies.find(academy => academy.urn === urn)
    if (academy === undefined) {
      throw new Error(`Expected to find academy in the test data containing URN: ${urn}, but it was not found`)
    }
    return academy
  }
}
