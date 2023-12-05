import { Page } from '@playwright/test'
import { FakeTestData, FakeTrust } from '../../fake-data/fake-test-data'
import { TrustHeaderComponent } from '../shared/trust-header-component'
import { NavigationComponent } from '../shared/navigation-component'
import { BasePage, BasePageAssertions } from '../base-page'

export class BaseTrustPage extends BasePage {
  readonly expect: BaseTrustPageAssertions
  readonly trustHeading: TrustHeaderComponent
  readonly trustNavigation: NavigationComponent

  fakeTestData: FakeTestData
  currentTrust: FakeTrust
  pageUrl: string

  constructor (page: Page, fakeTestData: FakeTestData, pageUrl: string) {
    super(page, pageUrl, '', '')

    this.expect = new BaseTrustPageAssertions(this)
    this.pageUrl = pageUrl
    this.fakeTestData = fakeTestData
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

export class BaseTrustPageAssertions extends BasePageAssertions {
  constructor (readonly trustPage: BaseTrustPage) {
    super(trustPage)
  }

  async toSeeCorrectTrustNameAndTypeInHeader (): Promise<void> {
    const { name, type } = this.trustPage.currentTrust
    await this.trustPage.trustHeading.expect.toSeeCorrectTrustNameAndType(name, type)
  }
}
