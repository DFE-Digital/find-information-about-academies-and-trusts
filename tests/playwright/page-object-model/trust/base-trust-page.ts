import { Locator, Page } from '@playwright/test'
import { FakeTestData, FakeTrust } from '../../fake-data/fake-test-data'

export class BaseTrustPage {
  readonly pageHeadingLocator: Locator

  fakeTestData: FakeTestData
  currentTrust: FakeTrust
  pageUrl: string

  constructor (readonly page: Page, fakeTestData: FakeTestData, pageUrl: string) {
    this.pageUrl = pageUrl
    this.fakeTestData = fakeTestData
    this.pageHeadingLocator = page.locator('h1')
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
