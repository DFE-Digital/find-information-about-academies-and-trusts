import { Locator } from '@playwright/test'

export class TableComponent {
  readonly locator: Locator
  readonly rowsLocator: Locator

  constructor (locator: Locator) {
    this.locator = locator
    this.rowsLocator = this.locator.locator('tr')
  }

  getRowComponentAt (rowNumber: number): RowComponent {
    return new RowComponent(this.rowsLocator.nth(rowNumber))
  }

  async getRowCount (): Promise<number> {
    return await this.rowsLocator.count()
  }
}

export class RowComponent {
  readonly locator: Locator
  readonly rowHeaderLocator: Locator

  constructor (locator: Locator) {
    this.locator = locator
    this.rowHeaderLocator = this.locator.locator('th')
  }

  cellLocator (columnNumber: number): Locator {
    return this.locator.locator('td').nth(columnNumber)
  }
}
