import { Page, expect } from '@playwright/test'
import { FakeTestData } from '../../../fake-data/fake-test-data'
import { BaseAcademiesPage, BaseAcademiesPageAssertions } from './base-academies-page'

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
  constructor (readonly ofstedRatingsPage: AcademiesOfstedRatingsPage) {
    super(ofstedRatingsPage)
  }

  async toBeOnTheRightPage (): Promise<void> {
    await this.toBeOnAcademiesInTrustPages()
    await expect(this.ofstedRatingsPage.page).toHaveTitle(/Ofsted ratings/)
  }

  // Once the page is updated with real data this override can be deleted
  // so that the method on the page object model is called with the real academy count.
  async toDisplayInformationForAllAcademiesInThatTrust (): Promise<void> {
    await expect(this.academiesPage.academiesRowLocator).toHaveCount(3)
  }

  async toDisplayCorrectInformationAboutAcademiesInThatTrust (): Promise<void> {
    // start at 1 (rather than 0) because the first row will be the header
    const firstRowComponent = this.ofstedRatingsPage.academiesTable.getRowComponentAt(1)
    await expect(firstRowComponent.rowHeaderLocator).toContainText('Barr and Community R.C. School')
    await expect(firstRowComponent.rowHeaderLocator).toContainText('URN: 109174')
    await expect(firstRowComponent.cellLocator(ColumnHeading.DateJoined)).toContainText('Nov 2018')
    await expect(firstRowComponent.cellLocator(ColumnHeading.PreviousOfstedRatings)).toContainText('Not yet inspected')
    await expect(firstRowComponent.cellLocator(ColumnHeading.CurrentOfstedRatings)).toContainText('Not yet inspected')

    const secondRowComponent = this.ofstedRatingsPage.academiesTable.getRowComponentAt(2)
    await expect(secondRowComponent.rowHeaderLocator).toContainText('Fay Fort Catholic Primary Academy')
    await expect(secondRowComponent.rowHeaderLocator).toContainText('URN: 109174')
    await expect(secondRowComponent.cellLocator(ColumnHeading.DateJoined)).toContainText('Nov 2017')
    await expect(secondRowComponent.cellLocator(ColumnHeading.PreviousOfstedRatings)).toContainText('Not yet inspected')
    await expect(secondRowComponent.cellLocator(ColumnHeading.CurrentOfstedRatings)).toContainText('Good')
    await expect(secondRowComponent.cellLocator(ColumnHeading.CurrentOfstedRatings)).toContainText('9 May 2021')

    const thirdRowComponent = this.ofstedRatingsPage.academiesTable.getRowComponentAt(2)
    await expect(thirdRowComponent.rowHeaderLocator).toContainText('Fay Fort Catholic Primary Academy')
    await expect(thirdRowComponent.rowHeaderLocator).toContainText('URN: 109174')
    await expect(thirdRowComponent.cellLocator(ColumnHeading.DateJoined)).toContainText('Nov 2017')
    await expect(thirdRowComponent.cellLocator(ColumnHeading.PreviousOfstedRatings)).toContainText('Not yet inspected')
    await expect(thirdRowComponent.cellLocator(ColumnHeading.CurrentOfstedRatings)).toContainText('Good')
    await expect(thirdRowComponent.cellLocator(ColumnHeading.CurrentOfstedRatings)).toContainText('9 May 2021')
  }

  async toDisplayTheCorrectTagsForEachOfstedRating (): Promise<void> {
    const secondRowComponent = this.ofstedRatingsPage.academiesTable.getRowComponentAt(2)
    const currentOfstedRatingCell = secondRowComponent.cellLocator(ColumnHeading.CurrentOfstedRatings)
    await expect(currentOfstedRatingCell).toContainText('After joining')

    const thirdRowComponent = this.ofstedRatingsPage.academiesTable.getRowComponentAt(3)
    const thirdRowPreviousOfstedRatingsCell = thirdRowComponent.cellLocator(ColumnHeading.PreviousOfstedRatings)
    const thirdRowCurrentOfstedRatingsCell = thirdRowComponent.cellLocator(ColumnHeading.CurrentOfstedRatings)

    await expect(thirdRowPreviousOfstedRatingsCell).toContainText('Before joining')
    await expect(thirdRowCurrentOfstedRatingsCell).toContainText('After joining')
  }
}
