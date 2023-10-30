import { Locator, Page, expect } from '@playwright/test'
import { TrustHeaderComponent } from '../shared/trust-header-component'
import { TrustNavigationComponent } from '../shared/trust-navigation-component'
import { FakeTestData } from '../../fake-data/fake-test-data'

export class ContactsPage {
  readonly expect: ContactsPageAssertions
  readonly trustHeading: TrustHeaderComponent
  readonly trustNavigation: TrustNavigationComponent
  readonly pageHeadingLocator: Locator

  fakeTestData: FakeTestData

  constructor (readonly page: Page, fakeTestData: FakeTestData) {
    this.fakeTestData = fakeTestData
    this.expect = new ContactsPageAssertions(this)
    this.trustHeading = new TrustHeaderComponent(page)
    this.trustNavigation = new TrustNavigationComponent(page)
    this.pageHeadingLocator = page.locator('h1')
  }

  async goTo (): Promise<void> {
    const uid = this.fakeTestData.getFirstTrust().uid
    await this.page.goto(`/trusts/contacts/${uid}`)
  }

  async goToPageWithoutUid (): Promise<void> {
    await this.page.goto('/trusts/contacts')
  }

  async goToPageWithUidThatDoesNotExist (): Promise<void> {
    await this.page.goto('/trusts/contacts/0000')
  }
}

class ContactsPageAssertions {
  constructor (readonly contactsPage: ContactsPage) {}

  async toBeOnTheRightPage (): Promise<void> {
    const { name } = this.contactsPage.fakeTestData.getFirstTrust()
    await this.contactsPage.trustHeading.expect.toContain(name)
    await expect(this.contactsPage.pageHeadingLocator).toHaveText('Contacts')
  }

  async toSeeCorrectTrustNameAndTypeInHeader (): Promise<void> {
    const { name, type } = this.contactsPage.fakeTestData.getFirstTrust()
    await this.contactsPage.trustHeading.expect.toSeeCorrectTrustNameAndType(name, type)
  }
}
