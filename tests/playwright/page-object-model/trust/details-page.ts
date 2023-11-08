import { Locator, Page, expect } from '@playwright/test'
import { TrustHeaderComponent } from '../shared/trust-header-component'
import { TrustNavigationComponent } from '../shared/trust-navigation-component'
import { CurrentSearch } from '../shared/search-form-component'
import { FakeTestData, FakeTrust } from '../../fake-data/fake-test-data'
import { formatDateAsExpected } from '../../helpers'

export class DetailsPage {
  readonly expect: DetailsPageAssertions
  readonly trustHeading: TrustHeaderComponent
  readonly trustNavigation: TrustNavigationComponent
  readonly pageHeadingLocator: Locator
  readonly trustDetailsCardLocator: Locator
  readonly referenceNumbersCardLocator: Locator

  currentSearch: CurrentSearch
  fakeTestData: FakeTestData
  currentTrust: FakeTrust

  constructor (readonly page: Page, fakeTestData: FakeTestData) {
    this.fakeTestData = fakeTestData
    this.expect = new DetailsPageAssertions(this)
    this.trustHeading = new TrustHeaderComponent(page)
    this.trustNavigation = new TrustNavigationComponent(page)
    this.pageHeadingLocator = page.locator('h1')
    this.trustDetailsCardLocator = this.page.getByText('Trust details Address')
    this.referenceNumbersCardLocator = this.page.getByText('Reference numbers UID')
  }

  async goTo (): Promise<void> {
    this.currentTrust = this.fakeTestData.getFirstTrust()
    await this.goToWith(this.currentTrust.uid)
  }

  async goToPageWithoutUid (): Promise<void> {
    await this.page.goto('/trusts/details')
  }

  async goToPageWithUidThatDoesNotExist (): Promise<void> {
    await this.goToWith('0000')
  }

  async goToMultiAcademyTrust (): Promise<void> {
    const uid = this.fakeTestData.getMultiAcademyTrust().uid
    await this.goToWith(uid)
  }

  async goToSingleAcademyTrust (): Promise<void> {
    const uid = this.fakeTestData.getSingleAcademyTrust().uid
    await this.goToWith(uid)
  }

  async goToWith (uid: string): Promise<void> {
    await this.page.goto(`/trusts/details?uid=${uid}`)
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
