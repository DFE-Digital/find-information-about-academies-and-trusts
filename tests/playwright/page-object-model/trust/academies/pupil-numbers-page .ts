import { Page, expect } from '@playwright/test'
import { FakeTestData } from '../../../fake-data/fake-test-data'
import {
  BaseAcademiesPage,
  BaseAcademiesPageAssertions
} from './base-academies-page'
import { RowComponent } from '../../shared/table-component'
import { FakeAcademy } from '../../../fake-data/types'

enum ColumnHeading {
  PhaseAndAgeRange,
  PupilNumbers,
  PupilCapacity,
  PercentageFull,
}

export class AcademiesPupilNumbersPage extends BaseAcademiesPage {
  readonly expect: PupilNumbersPageAssertions

  constructor (readonly page: Page, fakeTestData: FakeTestData) {
    super(page, fakeTestData, '/trusts/academies/pupil-numbers')
    this.expect = new PupilNumbersPageAssertions(this)
  }
}

export class PupilNumbersPageAssertions extends BaseAcademiesPageAssertions {
  constructor (readonly page: AcademiesPupilNumbersPage) {
    super(page)
  }

  async toBeOnTheRightPage (): Promise<void> {
    await this.toBeOnAcademiesInTrustPages()
    await expect(this.page.page).toHaveTitle(/pupil numbers/)
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
      await expect(rowComponent.cellLocator(ColumnHeading.PhaseAndAgeRange)).toContainText(expectedFakeAcademy.phaseOfEducation)
      await expect(rowComponent.cellLocator(ColumnHeading.PhaseAndAgeRange)).toContainText(`${expectedFakeAcademy.ageRange.minimum} - ${expectedFakeAcademy.ageRange.maximum}`)
      await expect(rowComponent.cellLocator(ColumnHeading.PupilNumbers)).toContainText(expectedFakeAcademy.numberOfPupils.toLocaleString())
      await expect(rowComponent.cellLocator(ColumnHeading.PupilCapacity)).toContainText(expectedFakeAcademy.schoolCapacity.toLocaleString())
      await expect(rowComponent.cellLocator(ColumnHeading.PercentageFull)).toContainText(`${expectedFakeAcademy.percentageFull}%`)
    }
  }
}
