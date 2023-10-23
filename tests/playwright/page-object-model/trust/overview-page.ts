import { Locator, Page, expect } from '@playwright/test'
import { TrustHeaderComponent } from '../shared/trust-header-component'
import { TrustNavigationComponent } from '../shared/trust-navigation-component'
import { MockTrustsProvider } from '../../mocks/mock-trusts-provider'

export class OverviewPage {
  readonly expect: OverviewPageAssertions
  readonly trustHeading: TrustHeaderComponent
  readonly trustNavigation: TrustNavigationComponent
  readonly pageHeadingLocator: Locator
  readonly trustSummaryCard: Locator
  readonly trustOfstedTable: Locator

  constructor (readonly page: Page) {
    this.expect = new OverviewPageAssertions(this)
    this.trustHeading = new TrustHeaderComponent(page)
    this.trustNavigation = new TrustNavigationComponent(page)
    this.pageHeadingLocator = page.locator('h1')
    this.trustSummaryCard = page.locator('[aria-labelledby="trust-information"]')
    this.trustOfstedTable = page.locator('[aria-labelledby="ofsted-ratings"]')
  }

  async goTo (
    ukprn: string = MockTrustsProvider.expectedFormattedTrustResult.ukprn
  ): Promise<void> {
    await this.page.goto(`/trusts/overview/${ukprn}`)
  }
}

class OverviewPageAssertions {
  constructor (readonly overviewPage: OverviewPage) {}

  async toBeOnTheRightPageFor (trust: string): Promise<void> {
    await this.overviewPage.trustHeading.expect.toContain(trust)
    await expect(this.overviewPage.pageHeadingLocator).toHaveText('Overview')
  }

  async toBeOnTheRightPage (): Promise<void> {
    const { name } = MockTrustsProvider.expectedFormattedTrustResult
    await this.toBeOnTheRightPageFor(name)
  }

  async toSeeCorrectTrustNameAndTypeInHeader (): Promise<void> {
    const { name, type } = MockTrustsProvider.expectedFormattedTrustResult
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
