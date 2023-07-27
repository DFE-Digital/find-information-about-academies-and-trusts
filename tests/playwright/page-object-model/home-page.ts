import { Page, expect } from '@playwright/test'

export class HomePage {
  readonly expect: HomePageAssertions

  constructor (readonly page: Page) {
    this.expect = new HomePageAssertions(this)
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
}
