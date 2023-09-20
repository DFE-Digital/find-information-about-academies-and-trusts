import { Locator, Page, expect } from '@playwright/test'
import { TrustHeaderComponent } from '../shared/trust-header-component'
import { TrustNavigationComponent } from '../shared/trust-navigation-component'
import { MockTrustsProvider } from '../../mocks/mock-trusts-provider'

export class DetailsPage {
  readonly expect: DetailsPageAssertions
  readonly trustHeading: TrustHeaderComponent
  readonly trustNavigation: TrustNavigationComponent
  readonly pageHeadingLocator: Locator

  constructor (readonly page: Page) {
    this.expect = new DetailsPageAssertions(this)
    this.trustHeading = new TrustHeaderComponent(page)
    this.trustNavigation = new TrustNavigationComponent(page)
    this.pageHeadingLocator = page.locator('h1')
  }

  async goTo (
    ukprn: string = MockTrustsProvider.expectedFormattedTrustResult.ukprn
  ): Promise<void> {
    await this.page.goto(`/trusts/details/${ukprn}`)
  }
}

class DetailsPageAssertions {
  constructor (readonly detailsPage: DetailsPage) {}

  async toBeOnTheRightPageFor (trust: string): Promise<void> {
    await this.detailsPage.trustHeading.expect.toContain(trust)
    await expect(this.detailsPage.pageHeadingLocator).toHaveText('Details')
  }

  async toBeOnTheRightPage (): Promise<void> {
    const { name } = MockTrustsProvider.expectedFormattedTrustResult
    await this.toBeOnTheRightPageFor(name)
  }

  async toSeeCorrectTrustNameAndTypeInHeader (): Promise<void> {
    const { name, type } = MockTrustsProvider.expectedFormattedTrustResult
    await this.detailsPage.trustHeading.expect.toSeeCorrectTrustNameAndType(name, type)
  }
}
