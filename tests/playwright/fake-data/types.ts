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
  sfsoLead: FakePerson
  trustRelationshipManager: FakePerson
  governors: FakeGovernor[]
  academies: FakeAcademy[]
}

export interface FakeAcademy {
  urn: number
  establishmentName: string
  typeOfEstablishment: string
  localAuthority: string
  urbanRural: string
  phaseOfEducation: string
  numberOfPupils: number
  schoolCapacity: number
  ageRange: {
    minimum: number
    maximum: number
  }
  percentageFull: number
  percentageFreeSchoolMeals: number
  dateAcademyJoinedTrust: Date
  currentOfstedRating: FakeOfstedRating
  previousOfstedRating: FakeOfstedRating
}

export interface FakePerson {
  fullName: string
  email: string
}

export interface FakeGovernor {
  fullName: string
  email: string
  role: string
}

export interface FakeOfstedRating {
  ofstedRatingScore: number
  inspectionEndDate: Date | null
}
