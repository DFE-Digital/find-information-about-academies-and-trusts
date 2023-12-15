import { Locator, Page, expect } from '@playwright/test'

export class SourcePanelComponent {
  readonly expect: SourcePanelComponentAssertions
  panel: Locator
  panelTitle: Locator

  constructor (readonly page: Page) {
    this.expect = new SourcePanelComponentAssertions(this)
    this.panel = page.getByRole('group').filter({ hasText: 'Source and updates' })
    this.panelTitle = page.getByText('Source and updates')
  }

  async openPanel (): Promise<void> {
    if (!(await this.isPanelOpen())) {
      await this.togglePanel()
    }
  }

  async closePanel (): Promise<void> {
    if ((await this.isPanelOpen())) {
      await this.togglePanel()
    }
  }

  async togglePanel (): Promise<void> {
    await this.panelTitle.click()
  }

  async getSourceItem (item: DataSourcePanelItem): Promise<Locator> {
    return this.panel.getByRole('list').filter({ hasText: item.fields })
  }

  async getSourceItemDataSource (item: DataSourcePanelItem): Promise<Locator> {
    const sourceItem = await this.getSourceItem(item)
    return sourceItem.getByRole('listitem').filter({ hasText: item.fields })
  }

  async getSourceItemLastUpdate (item: DataSourcePanelItem): Promise<Locator> {
    const sourceItem = await this.getSourceItem(item)
    return sourceItem.getByText('Last updated:')
  }

  async getSourceItemNextUpdate (item: DataSourcePanelItem): Promise<Locator> {
    const sourceItem = await this.getSourceItem(item)
    return sourceItem.getByText('Next scheduled update:')
  }

  async isPanelOpen (): Promise<boolean> {
    return await this.panel.getAttribute('open') !== null
  }
}

export interface DataSourcePanelItem {
  fields: string
  dataSource: string
  update: 'Daily' | 'Monthly'
}

export class SourcePanelComponentAssertions {
  constructor (readonly sourcePanel: SourcePanelComponent) {}

  async toHaveCorrectUpdates (item: DataSourcePanelItem): Promise<void> {
    await this.sourcePanel.openPanel()
    await expect(await this.sourcePanel.getSourceItemNextUpdate(item)).toContainText(item.update)
  }

  async toUseCorrectDataSource (item: DataSourcePanelItem): Promise<void> {
    await this.sourcePanel.openPanel()
    await expect(await this.sourcePanel.getSourceItemDataSource(item)).toContainText(item.dataSource, { ignoreCase: true })
  }
}
