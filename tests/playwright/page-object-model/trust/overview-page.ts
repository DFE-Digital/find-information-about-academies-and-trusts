import { Locator, Page, expect } from '@playwright/test'
import { TrustHeaderComponent } from '../shared/trust-header-component'
import { TrustNavigationComponent } from '../shared/trust-navigation-component'
import { MockTrustsProvider } from '../../mocks/mock-trusts-provider'

export class OverviewPage {
  readonly expect: OverviewPageAssertions
  readonly trustHeading: TrustHeaderComponent
  readonly trustNavigation: TrustNavigationComponent
  readonly pageHeadingLocator: Locator

  constructor (readonly page: Page) {
    this.expect = new OverviewPageAssertions(this)
    this.trustHeading = new TrustHeaderComponent(page)
    this.trustNavigation = new TrustNavigationComponent(page)
    this.pageHeadingLocator = page.locator('h1')
  }

  async goTo (
    ukprn: string = MockTrustsProvider.expectedFormattedTrustResult.ukprn
  ): Promise<void> {
    await this.page.goto(`/trusts/overview/${ukprn}`)
  }
}

class OverviewPageAssertions {
  constructor (readonly OverviewPage: OverviewPage) {}

  async toBeOnTheRightPageFor (trust: string): Promise<void> {
    await this.OverviewPage.trustHeading.expect.toContain(trust)
    await expect(this.OverviewPage.pageHeadingLocator).toHaveText('Overview')
  }

  async toBeOnTheRightPage (): Promise<void> {
    const { name } = MockTrustsProvider.expectedFormattedTrustResult
    await this.toBeOnTheRightPageFor(name)
  }

  async toSeeCorrectTrustNameAndTypeInHeader (): Promise<void> {
    const { name, type } = MockTrustsProvider.expectedFormattedTrustResult
    await this.OverviewPage.trustHeading.expect.toSeeCorrectTrustNameAndType(name, type)
  }
}
