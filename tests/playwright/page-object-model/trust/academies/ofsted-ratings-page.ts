import { Page, expect } from '@playwright/test'
import { FakeTestData } from '../../../fake-data/fake-test-data'
import { BaseAcademiesPage, BaseAcademiesPageAssertions } from './base-academies-page'

export class AcademiesOfstedRatingsPage extends BaseAcademiesPage {
  readonly expect: AcademiesOfstedRatingsPageAssertions
  constructor (readonly page: Page, fakeTestData: FakeTestData) {
    super(page, fakeTestData, '/trusts/academies/ofsted-data')
    this.expect = new AcademiesOfstedRatingsPageAssertions(this)
  }
}

class AcademiesOfstedRatingsPageAssertions extends BaseAcademiesPageAssertions {
  constructor (readonly ofstedRatingsPage: AcademiesOfstedRatingsPage) {
    super(ofstedRatingsPage)
  }

  async toBeOnTheRightPage (): Promise<void> {
    await this.toBeOnAcademiesInTrustPages()
    await expect(this.ofstedRatingsPage.page).toHaveTitle(/Ofsted ratings/)
  }
}
