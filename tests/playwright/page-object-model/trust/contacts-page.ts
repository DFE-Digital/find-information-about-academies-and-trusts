import { Locator, Page, expect } from '@playwright/test'
import { TrustHeaderComponent } from '../shared/trust-header-component'
import { TrustNavigationComponent } from '../shared/trust-navigation-component'
import { FakeTestData, FakeTrust } from '../../fake-data/fake-test-data'

export class ContactsPage {
  readonly expect: ContactsPageAssertions
  readonly trustHeading: TrustHeaderComponent
  readonly trustNavigation: TrustNavigationComponent
  readonly pageHeadingLocator: Locator
  readonly dfeContactsCard: Locator
  readonly trustContactsCard: Locator

  fakeTestData: FakeTestData
  currentTrust: FakeTrust

  constructor (readonly page: Page, fakeTestData: FakeTestData) {
    this.fakeTestData = fakeTestData
    this.expect = new ContactsPageAssertions(this)
    this.trustHeading = new TrustHeaderComponent(page)
    this.trustNavigation = new TrustNavigationComponent(page)
    this.pageHeadingLocator = page.locator('h1')
    this.dfeContactsCard = page.locator('[data-testid="dfe-contacts"]')
    this.trustContactsCard = page.locator('[data-testid="trust-contacts"]')
  }

  async goTo (): Promise<void> {
    this.currentTrust = this.fakeTestData.getFirstTrust()
    await this.page.goto(`/trusts/contacts/${this.currentTrust.uid}`)
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
    await expect(this.contactsPage.pageHeadingLocator).toHaveText('Contacts')
  }

  async toSeeCorrectTrustNameAndTypeInHeader (): Promise<void> {
    const { name, type } = this.contactsPage.fakeTestData.getFirstTrust()
    await this.contactsPage.trustHeading.expect.toSeeCorrectTrustNameAndType(name, type)
  }

  async toSeeCorrectDfeContacts (): Promise<void> {
    await expect(this.contactsPage.dfeContactsCard).toContainText('Keyshawn Hermiston')
    await expect(this.contactsPage.dfeContactsCard).toContainText('Keyshawn.Hermiston@education.gov.uk')
    await expect(this.contactsPage.dfeContactsCard).toContainText('Ayana Lueilwitz')
    await expect(this.contactsPage.dfeContactsCard).toContainText('Ayana.Lueilwitz@education.gov.uk')
  }

  async toSeeCorrectTrustContacts(): Promise<void> {
    await expect(this.contactsPage.trustContactsCard).toContainText('Tyler Welch')
    await expect(this.contactsPage.trustContactsCard).toContainText('Tyler.Welch@abbeylaneacademiestrust.co.uk')
    await expect(this.contactsPage.trustContactsCard).toContainText('Telephone: 027 5395 0525')
    await expect(this.contactsPage.trustContactsCard).toContainText('Courtney Pacocha')
    await expect(this.contactsPage.trustContactsCard).toContainText('Courtney.Pacocha@abbeylaneacademiestrust.co.uk')
    await expect(this.contactsPage.trustContactsCard).toContainText('Telephone: 0500 544079')
    await expect(this.contactsPage.trustContactsCard).toContainText('Lowell Hoppe')
    await expect(this.contactsPage.trustContactsCard).toContainText('Lowell.Hoppe@abbeylaneacademiestrust.co.uk')
    await expect(this.contactsPage.trustContactsCard).toContainText('Telephone: 01289 72558')
  }
}
