import { Locator, Page, expect } from '@playwright/test'
import { FakeTestData } from '../../../fake-data/fake-test-data'
import { BaseTrustPage, BaseTrustPageAssertions } from '../base-trust-page'
import { NavigationComponent } from '../../shared/navigation-component'
import { TableComponent } from '../../shared/table-component'

export class BaseAcademiesPage extends BaseTrustPage {
  readonly expect: BaseAcademiesPageAssertions
  readonly academiesTable: TableComponent
  readonly academiesRowLocator: Locator
  readonly subNavigation: NavigationComponent

  constructor (readonly page: Page, fakeTestData: FakeTestData, pageUrl: string) {
    super(page, fakeTestData, pageUrl)
    this.expect = new BaseAcademiesPageAssertions(this)
    this.academiesTable = new TableComponent(this.page.getByRole('table'))
    this.academiesRowLocator = this.academiesTable.locator.getByTestId('academy-row')
    this.subNavigation = new NavigationComponent(page, 'Academies tabs')
  }
}

export class BaseAcademiesPageAssertions extends BaseTrustPageAssertions {
  constructor (readonly academiesPage: BaseAcademiesPage) {
    super(academiesPage)
  }

  async toBeOnAcademiesInTrustPages (): Promise<void> {
    await expect(this.academiesPage.pageHeadingLocator).toHaveText('Academies in this trust')
  }

  async toDisplayInformationForAllAcademiesInThatTrust (): Promise<void> {
    await expect(this.academiesPage.academiesRowLocator).toHaveCount(3)
  }
}
