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
    this.dfeContactsCard = page.getByText('DfE contacts Trust relationship manager')
    this.trustContactsCard = page.getByText('Trust Contacts Accounting officer')
  }

  async goToTrustWithAllContactDetailsPopulated (): Promise<void> {
    this.currentTrust = this.fakeTestData.getTrustWithAllTrustContactDetailsPopulated()
    await this.goToWith(this.currentTrust.uid)
  }

  async goToTrustWithDfeContactDetailsMissing (): Promise<void> {
    this.currentTrust = this.fakeTestData.getTrustWithDfeContactDetailsMissing()
    await this.goToWith(this.currentTrust.uid)
  }

  async goToTrustWithTrustContactEmailMissing (): Promise<void> {
    this.currentTrust = this.fakeTestData.getTrustWithTrustChiefFinancialOfficerContactEmailMissing()
    await this.goToWith(this.currentTrust.uid)
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
    await expect(this.contactsPage.dfeContactsCard).toContainText(`Trust relationship manager ${this.contactsPage.currentTrust.trustRelationshipManager.fullName} ${this.contactsPage.currentTrust.trustRelationshipManager.email}`)
    await expect(this.contactsPage.dfeContactsCard).toContainText(`SFSO (Schools financial support and oversight) lead ${this.contactsPage.currentTrust.sfsoLead.fullName} ${this.contactsPage.currentTrust.sfsoLead.email}`)
  }

  async toSeeCorrectTrustContacts (): Promise<void> {
    const requiredTrustContacts = ['Accounting Officer', 'Chair of Trustees', 'Chief Financial Officer']
    const trustContacts = this.contactsPage.currentTrust.governors.filter(x => requiredTrustContacts.some(n => n === x.role))
    await expect(this.contactsPage.trustContactsCard).toContainText(`Chair of trustees ${trustContacts[0].fullName} ${trustContacts[0].email}`)
    await expect(this.contactsPage.trustContactsCard).toContainText(`Chief financial officer ${trustContacts[1].fullName} ${trustContacts[1].email}`)
    await expect(this.contactsPage.trustContactsCard).toContainText(`Accounting officer ${trustContacts[2].fullName} ${trustContacts[2].email}`)
  }

  async toSeeCorrectDfeContactsMissingInformationMessage (): Promise<void> {
    await expect(this.contactsPage.dfeContactsCard).toContainText('SFSO (Schools financial support and oversight) lead No contact information available')
  }

  async toSeeCorrectTrustContactMissingEmailMessage (): Promise<void> {
    const requiredTrustContacts = ['Chief Financial Officer']
    const trustContacts = this.contactsPage.currentTrust.governors.filter(x => requiredTrustContacts.some(n => n === x.role))
    await expect(this.contactsPage.trustContactsCard).toContainText(`Chief financial officer ${trustContacts[0].fullName} No contact email available`)
  }
}
