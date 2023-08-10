import { Page, expect } from '@playwright/test'

export class LogInPage {
  readonly expect: LogInPageAssertions

  readonly userNameFieldLocator = this.page.locator('[name=loginfmt]')
  readonly nextButtonLocator = this.page.getByRole('button', { name: 'Next' })
  readonly passwordFieldLocator = this.page.locator('[name=passwd]')
  readonly signInButtonLocator = this.page.getByRole('button', { name: 'Sign in' })
  readonly staySignedInTextLocator = this.page.getByText('Stay signed in')
  readonly noButtonLocator = this.page.getByRole('button', { name: 'No' })

  constructor (readonly page: Page) {
    this.expect = new LogInPageAssertions(this)
  }

  async goTo (): Promise<void> {
    await this.page.goto('/')
  }

  async logIn (username: string, password: string): Promise<void> {
    await this.userNameFieldLocator.fill((username))
    await this.nextButtonLocator.click()

    await this.passwordFieldLocator.fill((password))
    await this.signInButtonLocator.click()

    // Wait until we are on home page, we might (or might not) have to press a "Stay signed in" button
    const timeout = 3000
    const maxtime = Date.now() + timeout
    const step = 500
    while (Date.now() < maxtime) {
      try {
        if (await this.staySignedInTextLocator.isVisible()) {
          await this.noButtonLocator.click()
          break
        } else if ((await this.page.title()) === 'Home page - Find information about academies and trusts') {
          break
        } else {
          await this.page.waitForTimeout(step)
        }
      } catch (error) {
        // if we are mid navigation then an error will be thrown when we attempt to access the page
        // we should wait and try again
        await this.page.waitForTimeout(step)
      }
    }
  }
}

class LogInPageAssertions {
  constructor (readonly logInPage: LogInPage) { }

  async toBeDirectedToSignIn (): Promise<void> {
    await expect(this.logInPage.page).toHaveTitle('Sign in to your account')
  }
}
