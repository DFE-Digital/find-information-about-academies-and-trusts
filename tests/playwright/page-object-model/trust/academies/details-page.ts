import { Locator, Page, expect } from '@playwright/test'
import { BaseTrustPage } from '../base-trust-page'
import { FakeTestData } from '../../../fake-data/fake-test-data'

export class AcademiesDetailsPage extends BaseTrustPage {
  readonly expect: AcademiesDetailsPageAssertions
  readonly academiesTableLocator: Locator
  readonly academiesRowLocator: Locator

  constructor (readonly page: Page, fakeTestData: FakeTestData) {
    super(page, fakeTestData, '/trusts/academies/details')
    this.expect = new AcademiesDetailsPageAssertions(this)
    this.academiesTableLocator = this.page.getByRole('table')
    this.academiesRowLocator = this.academiesTableLocator.getByTestId('academy-row')
  }
}

class AcademiesDetailsPageAssertions {
  constructor (readonly detailsPage: AcademiesDetailsPage) {}

  async toBeOnTheRightPage (): Promise<void> {
    await expect(this.detailsPage.pageHeadingLocator).toHaveText('Academies in this trust')
  }

  async toDisplayInformationForAllAcademiesInThatTrust (): Promise<void> {
    await expect(this.detailsPage.academiesRowLocator).toHaveCount(3)
  }

  async toDisplayCorrectInformationAboutAcademiesInThatTrust (): Promise<void> {
    await expect(this.detailsPage.academiesTableLocator).toContainText('Barr and Community R.C. SchoolURN: 109174')
    await expect(this.detailsPage.academiesTableLocator).toContainText('URN: 109174')
    await expect(this.detailsPage.academiesTableLocator).toContainText('Kingston upon Hull, City of')
    await expect(this.detailsPage.academiesTableLocator).toContainText('Academy converter')
    await expect(this.detailsPage.academiesTableLocator).toContainText('Urban minor conurbation')
  }
}
