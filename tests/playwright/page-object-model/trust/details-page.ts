import { Page, expect } from '@playwright/test'
import { TrustHeaderComponent } from '../shared/trust-header-component'

export class DetailsPage {
  readonly expect: DetailsPageAssertions
  readonly _trustHeading: TrustHeaderComponent

  constructor (readonly page: Page) {
    this.expect = new DetailsPageAssertions(this)
    this._trustHeading = new TrustHeaderComponent(page)
  }
}

class DetailsPageAssertions {
  constructor (readonly detailsPage: DetailsPage) {}

  async toBeOnTheRightPageFor (trust: string): Promise<void> {
    await expect(this.detailsPage._trustHeading.locator).toHaveText(trust)
  }
}
