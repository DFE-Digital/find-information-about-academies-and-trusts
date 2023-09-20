import { Locator, Page, expect } from '@playwright/test'
import { MockTrustsProvider } from '../../mocks/mock-trusts-provider'
import { TrustHeaderComponent } from '../shared/trust-header-component'
import { TrustNavigationComponent } from '../shared/trust-navigation-component'

export class ContactsPage {
  readonly expect: ContactsPageAssertions
  readonly trustHeading: TrustHeaderComponent
  readonly trustNavigation: TrustNavigationComponent
  readonly pageHeadingLocator: Locator

  constructor (readonly page: Page) {
    this.expect = new ContactsPageAssertions(this)
    this.trustHeading = new TrustHeaderComponent(page)
    this.trustNavigation = new TrustNavigationComponent(page)
    this.pageHeadingLocator = page.locator('h1')
  }

  async goTo (
    ukprn: string = MockTrustsProvider.expectedFormattedTrustResult.ukprn
  ): Promise<void> {
    await this.page.goto(`/trusts/contacts/${ukprn}`)
  }
}

class ContactsPageAssertions {
  constructor (readonly contactsPage: ContactsPage) {}

  async toBeOnTheRightPage (): Promise<void> {
    const { name } = MockTrustsProvider.expectedFormattedTrustResult
    await this.contactsPage.trustHeading.expect.toContain(name)
    await expect(this.contactsPage.pageHeadingLocator).toHaveText('Contacts')
  }

  async toSeeCorrectTrustNameAndTypeInHeader (): Promise<void> {
    const { name, type } = MockTrustsProvider.expectedFormattedTrustResult
    await this.contactsPage.trustHeading.expect.toSeeCorrectTrustNameAndType(name, type)
  }
}
