import { Locator, Page, expect } from '@playwright/test'

export class PaginationComponent {
  readonly expect: PaginationComponentAssertions
  readonly previousButtonLocator: Locator
  readonly nextButtonLocator: Locator

  constructor (readonly page: Page) {
    this.expect = new PaginationComponentAssertions(this)
    this.previousButtonLocator = page.getByRole('link', { name: 'Previous' })
    this.nextButtonLocator = page.getByRole('link', { name: 'Next' })
  }

  async selectNextPage (): Promise<void> {
    await this.nextButtonLocator.click()
  }

  async selectPreviousPage (): Promise<void> {
    await this.previousButtonLocator.click()
  }

  async selectPage (pageNumber: number): Promise<void> {
    await this.locatePageNumber(pageNumber).click()
  }

  locatePageNumber (pageNumber: number): Locator {
    return this.page.getByRole('link', { name: `${pageNumber}` })
  }
}

class PaginationComponentAssertions {
  constructor (readonly pagination: PaginationComponent) {}

  async toShowNextPageLink (): Promise<void> {
    await expect(this.pagination.nextButtonLocator).toBeVisible()
  }

  async toNotShowNextPageLink (): Promise<void> {
    await expect(this.pagination.nextButtonLocator).not.toBeVisible()
  }

  async toShowPreviousPageLink (): Promise<void> {
    await expect(this.pagination.previousButtonLocator).toBeVisible()
  }

  async toNotShowPreviousPageLink (): Promise<void> {
    await expect(this.pagination.previousButtonLocator).not.toBeVisible()
  }

  async toShowPageNumber (pageNumber: number): Promise<void> {
    await expect(this.pagination.locatePageNumber(pageNumber)).toBeVisible()
  }

  async toBeOnSpecificPage (pageNumber: number): Promise<void> {
    await expect(this.pagination.locatePageNumber(pageNumber)).toHaveAttribute('aria-current', 'page')
  }
}
