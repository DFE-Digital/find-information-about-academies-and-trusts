import { Page } from '@playwright/test'
import { FakeTestData } from '../../fake-data/fake-test-data'
import { TrustHeaderComponent } from '../shared/trust-header-component'
import { NavigationComponent } from '../shared/navigation-component'
import { BasePage, BasePageAssertions } from '../base-page'
import { FakeTrust } from '../../fake-data/types'
import { DataSourcePanelItem, SourcePanelComponent } from './sources-and-updates-component'

export class BaseTrustPage extends BasePage {
  readonly expect: BaseTrustPageAssertions
  readonly trustHeading: TrustHeaderComponent
  readonly trustNavigation: NavigationComponent
  readonly sourcePanel: SourcePanelComponent

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
    this.sourcePanel = new SourcePanelComponent(page)
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

  async goToOpenMultiAcademyTrust (): Promise<void> {
    this.currentTrust = this.fakeTestData.getOpenMultiAcademyTrust()
    await this.goToWith(this.currentTrust.uid)
  }

  async goToOpenSingleAcademyTrust (): Promise<void> {
    this.currentTrust = this.fakeTestData.getOpenSingleAcademyTrust()
    await this.goToWith(this.currentTrust.uid)
  }

  async goToOpenSingleAcademyTrustWithAcademies (): Promise<void> {
    this.currentTrust = this.fakeTestData.getOpenSingleAcademyTrustWithAcademies()
    await this.goToWith(this.currentTrust.uid)
  }

  async goToOpenSingleAcademyTrustWithNoAcademies (): Promise<void> {
    this.currentTrust = this.fakeTestData.getOpenSingleAcademyTrustWithNoAcademies()
    await this.goToWith(this.currentTrust.uid)
  }

  async goToClosedTrust (): Promise<void> {
    this.currentTrust = this.fakeTestData.getClosedTrust()
    await this.goToWith(this.currentTrust.uid)
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

  async toSeeCorrectSourceAndUpdates (sources: DataSourcePanelItem[]): Promise<void> {
    await this.trustPage.sourcePanel.openPanel()
    for (const source of sources) {
      await this.trustPage.sourcePanel.expect.toHaveCorrectUpdates(source)
      await this.trustPage.sourcePanel.expect.toNotHaveUnknownDate(source)
      await this.trustPage.sourcePanel.expect.toUseCorrectDataSource(source)
    }
  }
}
