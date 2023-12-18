import { Page, expect } from '@playwright/test'
import { FakeTestData } from '../../../fake-data/fake-test-data'
import { BaseAcademiesPage, BaseAcademiesPageAssertions } from './base-academies-page'
import { RowComponent } from '../../shared/table-component'
import { FakeAcademy } from '../../../fake-data/types'

enum ColumnHeading {
  PupilsEligableForFreeSchoolMeals,
  LocalAuthorityAverage,
  NationalAverage
}

const percentageFormatToOneDecimalPlace = /\d+\.\d%/

export class AcademiesFreeSchoolMealsPage extends BaseAcademiesPage {
  readonly expect: FreeSchoolMealsPageAssertions
  constructor (readonly page: Page, fakeTestData: FakeTestData) {
    super(page, fakeTestData, '/trusts/academies/free-school-meals')
    this.expect = new FreeSchoolMealsPageAssertions(this)
  }
}

class FreeSchoolMealsPageAssertions extends BaseAcademiesPageAssertions {
  constructor (readonly page: AcademiesFreeSchoolMealsPage) {
    super(page)
  }

  async toBeOnTheRightPage (): Promise<void> {
    await this.toBeOnAcademiesInTrustPages()
    await expect(this.page.page).toHaveTitle(/free school meals/)
  }

  async toDisplayCorrectInformationAboutAcademiesInThatTrust (): Promise<void> {
    const rowCount = await this.page.academiesTable.getRowCount()
    let rowComponent: RowComponent
    let expectedFakeAcademy: FakeAcademy
    // start at 1 (rather than 0) because the first row will be the header
    for (let i = 1; i < rowCount; i++) {
      rowComponent = this.page.academiesTable.getRowComponentAt(i)
      expectedFakeAcademy = await this.page.getExpectedAcademyMatching(rowComponent.rowHeaderLocator)

      await expect(rowComponent.rowHeaderLocator).toContainText(expectedFakeAcademy.establishmentName)
      await expect(rowComponent.rowHeaderLocator).toContainText(`URN: ${expectedFakeAcademy.urn}`)
      await expect(rowComponent.cellLocator(ColumnHeading.PupilsEligableForFreeSchoolMeals)).toContainText(`${expectedFakeAcademy.percentageFreeSchoolMeals.toFixed(1)}%`)
      await expect(rowComponent.cellLocator(ColumnHeading.LocalAuthorityAverage)).toContainText(percentageFormatToOneDecimalPlace)
      await expect(rowComponent.cellLocator(ColumnHeading.NationalAverage)).toContainText(percentageFormatToOneDecimalPlace)
    }
  }
}
