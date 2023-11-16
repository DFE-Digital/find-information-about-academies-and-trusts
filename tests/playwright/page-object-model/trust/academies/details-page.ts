import { Page, expect } from '@playwright/test'
import { FakeTestData } from '../../../fake-data/fake-test-data'
import { BaseAcademiesPage, BaseAcademiesPageAssertions } from './base-academies-page'

export class AcademiesDetailsPage extends BaseAcademiesPage {
  readonly expect: AcademiesDetailsPageAssertions

  constructor (readonly page: Page, fakeTestData: FakeTestData) {
    super(page, fakeTestData, '/trusts/academies/details')
    this.expect = new AcademiesDetailsPageAssertions(this)
  }
}

export class AcademiesDetailsPageAssertions extends BaseAcademiesPageAssertions {
  constructor (readonly detailsPage: AcademiesDetailsPage) {
    super(detailsPage)
  }

  async toBeOnTheRightPage (): Promise<void> {
    await this.toBeOnAcademiesInTrustPages()
    await expect(this.detailsPage.page).toHaveTitle(/details/)
  }

  async toDisplayCorrectInformationAboutAcademiesInThatTrust (): Promise<void> {
    await expect(this.detailsPage.academiesTable.locator).toContainText('Barr and Community R.C. SchoolURN: 109174')
    await expect(this.detailsPage.academiesTable.locator).toContainText('URN: 109174')
    await expect(this.detailsPage.academiesTable.locator).toContainText('Kingston upon Hull, City of')
    await expect(this.detailsPage.academiesTable.locator).toContainText('Academy converter')
    await expect(this.detailsPage.academiesTable.locator).toContainText('Urban minor conurbation')
  }
}
