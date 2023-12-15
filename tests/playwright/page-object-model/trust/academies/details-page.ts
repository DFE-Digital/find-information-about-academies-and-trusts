import { Page, expect } from '@playwright/test'
import { FakeTestData } from '../../../fake-data/fake-test-data'
import { BaseAcademiesPage, BaseAcademiesPageAssertions } from './base-academies-page'
import { RowComponent } from '../../shared/table-component'
import { FakeAcademy } from '../../../fake-data/types'

enum ColumnHeading {
  LocalAuthority,
  Type,
  RuralOrUrban,
  GetInformationAboutSchools
}

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
    const rowCount = await this.detailsPage.academiesTable.getRowCount()
    let rowComponent: RowComponent
    let expectedFakeAcademy: FakeAcademy
    // start at 1 (rather than 0) because the first row will be the header
    for (let i = 1; i < rowCount; i++) {
      rowComponent = this.detailsPage.academiesTable.getRowComponentAt(i)
      expectedFakeAcademy = await this.detailsPage.getExpectedAcademyMatching(rowComponent.rowHeaderLocator)

      await expect(rowComponent.rowHeaderLocator).toContainText(expectedFakeAcademy.establishmentName)
      await expect(rowComponent.rowHeaderLocator).toContainText(`URN: ${expectedFakeAcademy.urn}`)
      await expect(rowComponent.cellLocator(ColumnHeading.LocalAuthority)).toContainText(expectedFakeAcademy.localAuthority)
      await expect(rowComponent.cellLocator(ColumnHeading.Type)).toContainText(expectedFakeAcademy.typeOfEstablishment)
      await expect(rowComponent.cellLocator(ColumnHeading.RuralOrUrban)).toContainText(expectedFakeAcademy.urbanRural)
      await expect(rowComponent.cellLocator(ColumnHeading.GetInformationAboutSchools)).toContainText('More information')
    }
  }
}
