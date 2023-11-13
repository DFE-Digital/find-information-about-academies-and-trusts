import { Locator, Page, expect } from '@playwright/test'
import { FakeTestData } from '../../fake-data/fake-test-data'
import { BaseTrustPage } from './base-trust-page'

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
}

class OverviewPageAssertions {
  constructor (readonly overviewPage: OverviewPage) {}

  async toBeOnTheRightPage (): Promise<void> {
    await expect(this.overviewPage.pageHeadingLocator).toHaveText('Overview')
  }

  async toSeeCorrectTrustNameAndTypeInHeader (): Promise<void> {
    const { name, type } = this.overviewPage.fakeTestData.getFirstTrust()
    await this.overviewPage.trustHeading.expect.toSeeCorrectTrustNameAndType(name, type)
  }

  async toSeeCorrectTrustSummary (): Promise<void> {
    await expect(this.overviewPage.trustSummaryCard).toContainText('Total academies  4')
    await expect(this.overviewPage.trustSummaryCard).toContainText('Academies in each local authority  3 in North Lincolnshire  1 in North East Lincolnshire')
    await expect(this.overviewPage.trustSummaryCard).toContainText('Pupil numbers  4,896')
    await expect(this.overviewPage.trustSummaryCard).toContainText('Pupil capacity (% full)  6,463(76%)')
  }

  async toSeeCorrectOfstedRatings (): Promise<void> {
    await expect(this.overviewPage.trustOfstedTable).toContainText('Outstanding  3')
    await expect(this.overviewPage.trustOfstedTable).toContainText('Good  5')
    await expect(this.overviewPage.trustOfstedTable).toContainText('Requires improvement  2')
    await expect(this.overviewPage.trustOfstedTable).toContainText('Inadequate  3')
    await expect(this.overviewPage.trustOfstedTable).toContainText('Not yet inspected  4')
  }
}
