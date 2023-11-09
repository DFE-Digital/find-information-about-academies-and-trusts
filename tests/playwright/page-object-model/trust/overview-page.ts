import { Locator, Page, expect } from '@playwright/test'
import { TrustHeaderComponent } from '../shared/trust-header-component'
import { NavigationComponent } from '../shared/navigation-component'
import { FakeTestData, FakeTrust } from '../../fake-data/fake-test-data'

export class OverviewPage {
  readonly expect: OverviewPageAssertions
  readonly trustHeading: TrustHeaderComponent
  readonly trustNavigation: NavigationComponent
  readonly pageHeadingLocator: Locator
  readonly trustSummaryCard: Locator
  readonly trustOfstedTable: Locator

  fakeTestData: FakeTestData
  currentTrust: FakeTrust

  constructor (readonly page: Page, fakeTestData: FakeTestData) {
    this.fakeTestData = fakeTestData
    this.expect = new OverviewPageAssertions(this)
    this.trustHeading = new TrustHeaderComponent(page)
    this.trustNavigation = new NavigationComponent(page, 'Sections')
    this.pageHeadingLocator = page.locator('h1')
    this.trustSummaryCard = page.locator('[data-testid="trust-summary"]')
    this.trustOfstedTable = page.locator('[data-testid="ofsted-ratings"]')
  }

  async goTo (): Promise<void> {
    this.currentTrust = this.fakeTestData.getFirstTrust()
    await this.page.goto(`/trusts/overview/${this.currentTrust.uid}`)
  }

  async goToPageWithoutUid (): Promise<void> {
    await this.page.goto('/trusts/overview')
  }

  async goToPageWithUidThatDoesNotExist (): Promise<void> {
    await this.page.goto('/trusts/overview/0000')
  }
}

class OverviewPageAssertions {
  constructor (readonly overviewPage: OverviewPage) {}

  async toBeOnTheRightPageFor (trust: string): Promise<void> {
    await this.overviewPage.trustHeading.expect.toContain(trust)
    await this.toBeOnTheRightPage()
  }

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
