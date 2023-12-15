import { Locator, Page, expect } from '@playwright/test'
import { FakeTestData } from '../../fake-data/fake-test-data'
import { formatDateAsExpected } from '../../helpers'
import { BaseTrustPage, BaseTrustPageAssertions } from './base-trust-page'
import { DataSourcePanelItem } from './sources-and-updates'

export class DetailsPage extends BaseTrustPage {
  readonly expect: DetailsPageAssertions
  readonly trustDetailsCardLocator: Locator
  readonly referenceNumbersCardLocator: Locator
  readonly giasLinkLocator: Locator
  readonly companiesHouseLinkLocator: Locator
  readonly schoolPerformanceLinkLocator: Locator
  readonly financialBenchmarkingLinkLocator: Locator

  constructor (readonly page: Page, fakeTestData: FakeTestData) {
    super(page, fakeTestData, '/trusts/details')
    this.expect = new DetailsPageAssertions(this)
    this.trustDetailsCardLocator = this.page.getByText('Trust details Address')
    this.referenceNumbersCardLocator = this.page.getByText('Reference numbers UID')
    this.giasLinkLocator = this.page.getByRole('link', { name: 'Get information about schools' })
    this.companiesHouseLinkLocator = this.page.getByRole('link', { name: 'Companies House' })
    this.schoolPerformanceLinkLocator = this.page.getByRole('link', { name: 'Find school college and performance data in England' })
    this.financialBenchmarkingLinkLocator = this.page.getByRole('link', { name: 'Schools financial benchmarking' })
  }
}

class DetailsPageAssertions extends BaseTrustPageAssertions {
  constructor (readonly detailsPage: DetailsPage) {
    super(detailsPage)
  }

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

  async toSeeCorrectLinksForOpenTrust (): Promise<void> {
    await expect(this.detailsPage.companiesHouseLinkLocator).toBeVisible()
    await expect(this.detailsPage.giasLinkLocator).toBeVisible()
    await expect(this.detailsPage.financialBenchmarkingLinkLocator).toBeVisible()
    await expect(this.detailsPage.schoolPerformanceLinkLocator).toBeVisible()
  }

  async toSeeCorrectLinksForSingleAcademyTrustWithNoAcademies (): Promise<void> {
    await expect(this.detailsPage.companiesHouseLinkLocator).toBeVisible()
    await expect(this.detailsPage.giasLinkLocator).toBeVisible()
  }

  async toSeeCorrectLinksForClosedTrust (): Promise<void> {
    await expect(this.detailsPage.companiesHouseLinkLocator).toBeVisible()
  }
}
