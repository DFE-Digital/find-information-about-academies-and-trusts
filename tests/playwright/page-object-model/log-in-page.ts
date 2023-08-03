import { Page, expect } from '@playwright/test'

export class LogInPage {
  readonly expect: LogInPageAssertions

  constructor (readonly page: Page) {
    this.expect = new LogInPageAssertions(this)
  }
}

class LogInPageAssertions {
  constructor (readonly logInPage: LogInPage) { }

  async toBeDirectedToSignIn (): Promise<void> {
    await expect(this.logInPage.page).toHaveTitle('Sign in to your account')
  }
}
