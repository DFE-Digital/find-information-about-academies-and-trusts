import { Locator, Page, expect } from '@playwright/test'
import { CurrentSearch, SearchFormComponent } from './shared/search-form-component'

export class HomePage {
  readonly expect: HomePageAssertions
  readonly searchForm: SearchFormComponent
  readonly _searchBoxLocator: Locator
  readonly _searchButtonLocator: Locator

  constructor (readonly page: Page, currentSearch: CurrentSearch) {
    this.expect = new HomePageAssertions(this)
    this.searchForm = new SearchFormComponent(
      page,
      'Find information about academies and trusts',
      currentSearch
    )
  }

  async goTo (): Promise<void> {
    await this.page.goto('/')
  }
}

class HomePageAssertions {
  constructor (readonly homePage: HomePage) {}

  async toBeOnTheRightPage (): Promise<void> {
    await expect(this.homePage.page).toHaveTitle('Home page - Find information about academies and trusts')
  }

  async notToBeOnThePage (): Promise<void> {
    await expect(this.homePage.page).not.toHaveTitle('Home page - Find information about academies and trusts')
  }
}
