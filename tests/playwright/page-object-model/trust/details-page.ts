import { Locator, Page, expect } from '@playwright/test'
import { TrustHeaderComponent } from '../shared/trust-header-component'
import { TrustNavigationComponent } from '../shared/trust-navigation-component'
import { FakeTestData } from '../../fake-data/fake-test-data'
import { formatDateAsExpected } from '../../helpers'
import { BaseTrustPage } from './base-trust-page'

export class DetailsPage extends BaseTrustPage {
  readonly expect: DetailsPageAssertions
  readonly trustHeading: TrustHeaderComponent
  readonly trustNavigation: TrustNavigationComponent
  readonly trustDetailsCardLocator: Locator
  readonly referenceNumbersCardLocator: Locator

  constructor (readonly page: Page, fakeTestData: FakeTestData) {
    super(page, fakeTestData, '/trusts/details')
    this.expect = new DetailsPageAssertions(this)
    this.trustHeading = new TrustHeaderComponent(page)
    this.trustNavigation = new TrustNavigationComponent(page)
    this.trustDetailsCardLocator = this.page.getByText('Trust details Address')
    this.referenceNumbersCardLocator = this.page.getByText('Reference numbers UID')
  }
}

class DetailsPageAssertions {
  constructor (readonly detailsPage: DetailsPage) {}

  async toBeOnTheRightPageFor (trustName: string): Promise<void> {
    await this.detailsPage.trustHeading.expect.toContain(trustName)
    await this.toBeOnTheRightPage()
  }

  async toBeOnTheRightPage (): Promise<void> {
    await expect(this.detailsPage.pageHeadingLocator).toHaveText('Details')
  }

  async toSeeCorrectTrustNameAndTypeInHeader (): Promise<void> {
    const { name, type } = this.detailsPage.fakeTestData.getFirstTrust()
    await this.detailsPage.trustHeading.expect.toSeeCorrectTrustNameAndType(name, type)
  }

  async toSeeCorrectTrustDetails (): Promise<void> {
    await expect(this.detailsPage.trustDetailsCardLocator).toContainText(`Address ${this.detailsPage.currentTrust.address}`)
    await expect(this.detailsPage.trustDetailsCardLocator).toContainText(`Opened on ${formatDateAsExpected(this.detailsPage.currentTrust.openedDate)}`)
    await expect(this.detailsPage.trustDetailsCardLocator).toContainText(`Region and territory ${this.detailsPage.currentTrust.regionAndTerritory}`)
  }

  async toSeeCorrectTrustReferenceNumbers (): Promise<void> {
    await expect(this.detailsPage.referenceNumbersCardLocator).toContainText(`UID (Unique group identifier) ${this.detailsPage.currentTrust.uid}`)
    await expect(this.detailsPage.referenceNumbersCardLocator).toContainText(`Group ID (identifier) and TRN (trust reference number) ${this.detailsPage.currentTrust.groupId}`)
    await expect(this.detailsPage.referenceNumbersCardLocator).toContainText(`UKPRN (UK provider reference number) ${this.detailsPage.currentTrust.ukprn}`)
    await expect(this.detailsPage.referenceNumbersCardLocator).toContainText(`Companies House number ${this.detailsPage.currentTrust.companiesHouseNumber}`)
  }
}
