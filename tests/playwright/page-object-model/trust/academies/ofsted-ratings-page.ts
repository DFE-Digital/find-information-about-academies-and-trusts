import { Locator, Page, expect } from '@playwright/test'
import { FakeTestData } from '../../../fake-data/fake-test-data'
import { BaseAcademiesPage, BaseAcademiesPageAssertions } from './base-academies-page'
import { RowComponent } from '../../shared/table-component'
import { formatDateAsExpected } from '../../../helpers'
import { FakeAcademy, FakeOfstedRating } from '../../../fake-data/types'

enum ColumnHeading {
  DateJoined,
  PreviousOfstedRatings,
  CurrentOfstedRatings
}

export class AcademiesOfstedRatingsPage extends BaseAcademiesPage {
  readonly expect: AcademiesOfstedRatingsPageAssertions
  constructor (readonly page: Page, fakeTestData: FakeTestData) {
    super(page, fakeTestData, '/trusts/academies/ofsted-ratings')
    this.expect = new AcademiesOfstedRatingsPageAssertions(this)
  }
}

class AcademiesOfstedRatingsPageAssertions extends BaseAcademiesPageAssertions {
  constructor (readonly page: AcademiesOfstedRatingsPage) {
    super(page)
  }

  async toBeOnTheRightPage (): Promise<void> {
    await this.toBeOnAcademiesInTrustPages()
    await expect(this.page.page).toHaveTitle(/Ofsted ratings/)
  }

  async toDisplayCorrectInformationAboutAcademiesInThatTrust (): Promise<void> {
    const rowCount = await this.page.academiesTable.getRowCount()
    let rowComponent: RowComponent
    let academy: FakeAcademy

    // start at 1 (rather than 0) because the first row will be the header
    for (let i = 1; i < rowCount; i++) {
      rowComponent = this.page.academiesTable.getRowComponentAt(i)
      academy = await this.page.getExpectedAcademyMatching(rowComponent.rowHeaderLocator)

      await expect(rowComponent.rowHeaderLocator).toContainText(academy.establishmentName)
      await expect(rowComponent.rowHeaderLocator).toContainText(`URN: ${academy.urn}`)
      await expect(rowComponent.cellLocator(ColumnHeading.DateJoined)).toContainText(formatDateAsExpected(academy.dateAcademyJoinedTrust))

      await this.expectToDisplayTheCorrectOfstedRatingDetailsFor(
        academy.previousOfstedRating,
        academy.dateAcademyJoinedTrust,
        rowComponent.cellLocator(ColumnHeading.PreviousOfstedRatings))

      await this.expectToDisplayTheCorrectOfstedRatingDetailsFor(
        academy.currentOfstedRating,
        academy.dateAcademyJoinedTrust,
        rowComponent.cellLocator(ColumnHeading.CurrentOfstedRatings))
    }
  }

  private async expectToDisplayTheCorrectOfstedRatingDetailsFor (ofstedRating: FakeOfstedRating, dateAcademyJoinedTrust: Date, cell: Locator): Promise<void> {
    await expect(cell).toContainText(getExpectedOfstedRatingDescription(ofstedRating.ofstedRatingScore))
    if (ofstedRating.inspectionEndDate !== null) {
      await expect(cell).toContainText(formatDateAsExpected(ofstedRating.inspectionEndDate))
      await expect(cell).toContainText(ofstedRating.inspectionEndDate >= dateAcademyJoinedTrust ? 'After joining' : 'Before joining')
    }
  }
}

const getExpectedOfstedRatingDescription = (key: number): string => {
  switch (key) {
    case -1:
      return 'Not yet inspected'
    case 1:
      return 'Outstanding'
    case 2:
      return 'Good'
    case 3:
      return 'Requires improvement'
    case 4:
      return 'Inadequate'
    default:
      return ''
  }
}
