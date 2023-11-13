import { Locator, Page } from '@playwright/test'
import { FakeTestData, FakeTrust } from '../../fake-data/fake-test-data'
import { TrustHeaderComponent } from '../shared/trust-header-component'
import { NavigationComponent } from '../shared/navigation-component'

export class BaseTrustPage {
  readonly expect: BaseTrustPageAssertions
  readonly pageHeadingLocator: Locator
  readonly trustHeading: TrustHeaderComponent
  readonly trustNavigation: NavigationComponent

  fakeTestData: FakeTestData
  currentTrust: FakeTrust
  pageUrl: string

  constructor (readonly page: Page, fakeTestData: FakeTestData, pageUrl: string) {
    this.expect = new BaseTrustPageAssertions(this)
    this.pageUrl = pageUrl
    this.fakeTestData = fakeTestData
    this.pageHeadingLocator = page.locator('h1')
    this.trustHeading = new TrustHeaderComponent(page)
    this.trustNavigation = new NavigationComponent(page, 'Sections')
  }

  async goTo (): Promise<void> {
    this.currentTrust = this.fakeTestData.getFirstTrust()
    await this.goToWith(this.currentTrust.uid)
  }

  async goToPageWithoutUid (): Promise<void> {
    await this.page.goto(this.pageUrl)
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
    await this.page.goto(`${this.pageUrl}?uid=${uid}`)
  }
}

export class BaseTrustPageAssertions {
  constructor (readonly trustPage: BaseTrustPage) {}

  async toSeeCorrectTrustNameAndTypeInHeader (): Promise<void> {
    const { name, type } = this.trustPage.currentTrust
    await this.trustPage.trustHeading.expect.toSeeCorrectTrustNameAndType(name, type)
  }
}
