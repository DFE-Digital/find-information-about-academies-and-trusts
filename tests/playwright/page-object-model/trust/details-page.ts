import { Locator, Page, expect } from '@playwright/test'
import { TrustHeaderComponent } from '../shared/trust-header-component'
import { TrustNavigationComponent } from '../shared/trust-navigation-component'
import { CurrentSearch } from '../shared/search-form-component'
import { FakeTestData } from '../../fake-data/fake-test-data'

export class DetailsPage {
  readonly expect: DetailsPageAssertions
  readonly trustHeading: TrustHeaderComponent
  readonly trustNavigation: TrustNavigationComponent
  readonly pageHeadingLocator: Locator
  readonly trustDetailsCardLocator: Locator
  readonly referenceNumbersCardLocator: Locator

  currentSearch: CurrentSearch
  fakeTestData: FakeTestData

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
    const uid = this.fakeTestData.getFirstTrust().uid
    await this.page.goto(`/trusts/details/${uid}`)
  }

  async goToPageWithoutUid (): Promise<void> {
    await this.page.goto('/trusts/details')
  }

  async goToPageWithUidThatDoesNotExist (): Promise<void> {
    await this.page.goto('/trusts/details/0000')
  }

  async goToMultiAcademyTrust (): Promise<void> {
    const uid = this.fakeTestData.getMultiAcademyTrust().uid
    await this.page.goto(`/trusts/details/${uid}`)
  }

  async goToSingleAcademyTrust (): Promise<void> {
    const uid = this.fakeTestData.getSingleAcademyTrust().uid
    await this.page.goto(`/trusts/details/${uid}`)
  }
}

class DetailsPageAssertions {
  constructor (readonly detailsPage: DetailsPage) {}

  async toBeOnTheRightPageFor (trustName: string): Promise<void> {
    await this.detailsPage.trustHeading.expect.toContain(trustName)
    await expect(this.detailsPage.pageHeadingLocator).toHaveText('Details')
  }

  async toBeOnTheRightPage (): Promise<void> {
    const { name } = this.detailsPage.fakeTestData.getFirstTrust()
    await this.toBeOnTheRightPageFor(name)
  }

  async toSeeCorrectTrustNameAndTypeInHeader (): Promise<void> {
    const { name, type } = this.detailsPage.fakeTestData.getFirstTrust()
    await this.detailsPage.trustHeading.expect.toSeeCorrectTrustNameAndType(name, type)
  }

  async toSeeCorrectTrustDetails (): Promise<void> {
    await expect(this.detailsPage.trustDetailsCardLocator).toContainText('Address Dorthy Inlet, Kingston upon Hull, City of, JY36 9VC')
    await expect(this.detailsPage.trustDetailsCardLocator).toContainText('Opened on 30 Dec 2013')
    await expect(this.detailsPage.trustDetailsCardLocator).toContainText('Region and territory Yorkshire and the Humber')
  }

  async toSeeCorrectTrustReferenceNumbers (): Promise<void> {
    await expect(this.detailsPage.referenceNumbersCardLocator).toContainText('UID (Unique group identifier) 2412')
    await expect(this.detailsPage.referenceNumbersCardLocator).toContainText('Group ID (identifier) and TRN (trust reference number) TR3971')
    await expect(this.detailsPage.referenceNumbersCardLocator).toContainText('UKPRN (UK provider reference number) 10013796')
    await expect(this.detailsPage.referenceNumbersCardLocator).toContainText('Companies House number 03080547')
  }
}
