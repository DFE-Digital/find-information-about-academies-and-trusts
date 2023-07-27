import { Locator, Page, expect } from '@playwright/test'

export class HomePage {
  readonly expect: HomePageAssertions
  readonly _searchBoxLocator: Locator
  readonly _searchButtonLocator: Locator

  constructor (readonly page: Page) {
    this.expect = new HomePageAssertions(this)
    this._searchBoxLocator = this.page.getByLabel('Find information about academies and trusts')
    this._searchButtonLocator = this.page.getByRole('button', { name: 'Search' })
  }

  async goTo (): Promise<void> {
    await this.page.goto('/')
  }

  async searchFor (searchTerm: string): Promise<void> {
    await this._searchBoxLocator.fill(searchTerm)
    await this._searchButtonLocator.click()
  }
}

class HomePageAssertions {
  constructor (readonly homePage: HomePage) {}

  async toBeOnTheRightPage (): Promise<void> {
    await expect(this.homePage.page).toHaveTitle('Home page - Find information about academies and trusts')
  }
}
