import { Locator, Page, expect } from '@playwright/test'

export class TrustHeaderComponent {
  readonly expect: TrustHeaderComponentAssertions
  readonly locator: Locator

  constructor (readonly page: Page) {
    this.expect = new TrustHeaderComponentAssertions(this)
    this.locator = page.getByTestId('app-trust-header')
  }
}

class TrustHeaderComponentAssertions {
  constructor (readonly trustHeader: TrustHeaderComponent) {}

  async toContain (name: string): Promise<void> {
    await expect(this.trustHeader.locator).toContainText(name)
  }

  async toSeeCorrectTrustNameAndType (name: string, type: string): Promise<void> {
    await expect(this.trustHeader.locator).toHaveText(`${name} ${type}`)
  }

  async toHaveMultiAcademyTrustType (): Promise<void> {
    await expect(this.trustHeader.locator).toContainText('Multi-academy trust')
  }

  async toHaveSingleAcademyTrustType (): Promise<void> {
    await expect(this.trustHeader.locator).toContainText('Single-academy trust')
  }
}
