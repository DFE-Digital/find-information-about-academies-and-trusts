import { Locator, Page, expect } from '@playwright/test'
import { FakeTestData } from '../../fake-data/fake-test-data'
import { BaseTrustPage, BaseTrustPageAssertions } from './base-trust-page'

export class OverviewPage extends BaseTrustPage {
  readonly expect: OverviewPageAssertions
  readonly trustSummaryCard: Locator
  readonly trustOfstedTable: Locator

  constructor (readonly page: Page, fakeTestData: FakeTestData) {
    super(page, fakeTestData, '/trusts/overview')
    this.expect = new OverviewPageAssertions(this)
    this.trustSummaryCard = page.locator('[data-testid="trust-summary"]')
    this.trustOfstedTable = page.locator('[data-testid="ofsted-ratings"]')
  }

  async goToMultiAcademyTrust (): Promise<void> {
    this.currentTrust = this.fakeTestData.getOpenMultiAcademyTrust()
    await this.goToWith(this.currentTrust.uid)
  }

  async goToTrustWithNoAcademies (): Promise<void> {
    this.currentTrust = this.fakeTestData.getOpenSingleAcademyTrustWithNoAcademies()
    await this.goToWith(this.currentTrust.uid)
  }
}

class OverviewPageAssertions extends BaseTrustPageAssertions {
  constructor (readonly overviewPage: OverviewPage) {
    super(overviewPage)
  }

  async toBeOnTheRightPage (): Promise<void> {
    await expect(this.overviewPage.pageHeadingLocator).toHaveText('Overview')
  }

  async toSeeCorrectTrustNameAndTypeInHeader (): Promise<void> {
    const { name, type } = this.overviewPage.fakeTestData.getFirstTrust()
    await this.overviewPage.trustHeading.expect.toSeeCorrectTrustNameAndType(name, type)
  }

  async toSeeCorrectTrustSummary (): Promise<void> {
    await expect(this.overviewPage.trustSummaryCard).toContainText(`Total academies ${this.overviewPage.currentTrust.academies.length}`)

    await expect(this.overviewPage.trustSummaryCard).toContainText('Academies in each local authority')

    const ListOfAuthorities = this.overviewPage.currentTrust.academies
      .map(x => x.localAuthority)
      .reduce((acc: { [key: string]: number }, curr) => {
        acc[curr] = (acc[curr] !== undefined && acc[curr] !== null) ? acc[curr] + 1 : 1
        return acc
      }, {})

    for (const key in ListOfAuthorities) {
      const authorityCount = `${ListOfAuthorities[key]} in ${key}`
      await expect(this.overviewPage.trustSummaryCard).toContainText(`${authorityCount}`)
    }

    const totalTrustPupilNumbers = this.overviewPage.currentTrust.academies.reduce((sum, x) => sum + x.numberOfPupils, 0)
    await expect(this.overviewPage.trustSummaryCard).toContainText(`Pupil numbers ${totalTrustPupilNumbers.toLocaleString()}`)

    const totalTrustCapacity = this.overviewPage.currentTrust.academies.reduce((sum, x) => sum + x.schoolCapacity, 0)
    await expect(this.overviewPage.trustSummaryCard).toContainText(`Pupil capacity (% full) ${totalTrustCapacity.toLocaleString()}`)

    await expect(this.overviewPage.trustSummaryCard).toContainText(/\d+%/)
  }

  async toSeeCorrectTrustSummaryWithNoAcademies (): Promise<void> {
    await expect(this.overviewPage.trustSummaryCard).toContainText('Total academies 0')
    await expect(this.overviewPage.trustSummaryCard).toContainText('Academies in each local authority')
    await expect(this.overviewPage.trustSummaryCard).toContainText('Pupil numbers 0')
    await expect(this.overviewPage.trustSummaryCard).toContainText('Pupil capacity (% full) 0')
  }

  async toSeePopulatedOfstedRatings (): Promise<void> {
    await expect(this.overviewPage.trustOfstedTable).toContainText(/Outstanding\s+\d+/)
    await expect(this.overviewPage.trustOfstedTable).toContainText(/Good\s+\d+/)
    await expect(this.overviewPage.trustOfstedTable).toContainText(/Requires improvement\s+\d+/)
    await expect(this.overviewPage.trustOfstedTable).toContainText(/Inadequate\s+\d+/)
    await expect(this.overviewPage.trustOfstedTable).toContainText(/Not yet inspected\s+\d+/)
  }

  async toSeeCorrectOfstedRatingsWithNoAcademies (): Promise<void> {
    await expect(this.overviewPage.trustOfstedTable).toContainText('Outstanding 0')
    await expect(this.overviewPage.trustOfstedTable).toContainText('Good 0')
    await expect(this.overviewPage.trustOfstedTable).toContainText('Requires improvement 0')
    await expect(this.overviewPage.trustOfstedTable).toContainText('Inadequate 0')
    await expect(this.overviewPage.trustOfstedTable).toContainText('Not yet inspected 0')
  }
}
