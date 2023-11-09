import { Locator, Page, expect } from '@playwright/test'
import { FakeTestData } from '../../../fake-data/fake-test-data'
import { BaseTrustPage } from '../base-trust-page'

export class BaseAcademiesPage extends BaseTrustPage {
  readonly expect: BaseAcademiesPageAssertions
  readonly academiesTableLocator: Locator
  readonly academiesRowLocator: Locator

  constructor (readonly page: Page, fakeTestData: FakeTestData, pageUrl: string) {
    super(page, fakeTestData, '/trusts/academies/details')
    this.expect = new BaseAcademiesPageAssertions(this)
    this.academiesTableLocator = this.page.getByRole('table')
    this.academiesRowLocator = this.academiesTableLocator.getByTestId('academy-row')
  }
}

export class BaseAcademiesPageAssertions {
  constructor (readonly academiesPage: BaseAcademiesPage) {}

  async toBeOnTheRightPage (): Promise<void> {
    await expect(this.academiesPage.pageHeadingLocator).toHaveText('Academies in this trust')
  }

  async toDisplayInformationForAllAcademiesInThatTrust (): Promise<void> {
    await expect(this.academiesPage.academiesRowLocator).toHaveCount(3)
  }
}
