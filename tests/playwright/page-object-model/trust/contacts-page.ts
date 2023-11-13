import { Locator, Page, expect } from '@playwright/test'
import { FakeTestData } from '../../fake-data/fake-test-data'
import { BaseTrustPage, BaseTrustPageAssertions } from './base-trust-page'

export class ContactsPage extends BaseTrustPage {
  readonly expect: ContactsPageAssertions
  readonly dfeContactsCard: Locator
  readonly trustContactsCard: Locator

  constructor (readonly page: Page, fakeTestData: FakeTestData) {
    super(page, fakeTestData, '/trusts/contacts')
    this.expect = new ContactsPageAssertions(this)
    this.dfeContactsCard = page.locator('[data-testid="dfe-contacts"]')
    this.trustContactsCard = page.locator('[data-testid="trust-contacts"]')
  }
}

class ContactsPageAssertions extends BaseTrustPageAssertions {
  constructor (readonly contactsPage: ContactsPage) {
    super(contactsPage)
  }

  async toBeOnTheRightPage (): Promise<void> {
    await expect(this.contactsPage.pageHeadingLocator).toHaveText('Contacts')
  }

  async toSeeCorrectDfeContacts (): Promise<void> {
    await expect(this.contactsPage.dfeContactsCard).toContainText('Keyshawn Hermiston')
    await expect(this.contactsPage.dfeContactsCard).toContainText('Keyshawn.Hermiston@education.gov.uk')
    await expect(this.contactsPage.dfeContactsCard).toContainText('Ayana Lueilwitz')
    await expect(this.contactsPage.dfeContactsCard).toContainText('Ayana.Lueilwitz@education.gov.uk')
  }

  async toSeeCorrectTrustContacts (): Promise<void> {
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
