import { Locator, Page, expect } from "@playwright/test";

export class SourcePanelComponent{
    readonly expect: SourcePanelComponentAssertions
    panel: Locator
    panelTitle: Locator

    constructor (readonly page: Page){
        this.expect = new SourcePanelComponentAssertions(this)
        this.panel = page.getByLabel("Source and updates")
        this.panelTitle = page.getByText("Source and updates")
    }

    async openPanel(): Promise<void>{
        if(!(await this.isPanelOpen())){
            await this.togglePanel()
        }
    }

    async closePanel(): Promise<void>{
        if((await this.isPanelOpen())){
            await this.togglePanel()
        }
    }

    async togglePanel(): Promise<void>{
        await this.panelTitle.click()
    }

    async getSourceItem(label: string): Promise<Locator>{
        return this.page.getByLabel(label)
    }

    async getSourceItemNextUpdate(label: string): Promise<Locator>{
        return (await this.getSourceItem(label)).getByText("Next scheduled update:")
    }

    async isPanelOpen(): Promise<boolean> {
        return await this.panel.getAttribute("open") !== null
    }

    
}

export class SourcePanelComponentAssertions{
    constructor (readonly sourcePanel: SourcePanelComponent) {}

    async toHaveDailyUpdates (label: string): Promise<void> {
        await this.sourcePanel.openPanel()
        expect(await this.sourcePanel.getSourceItemNextUpdate(label)).toContainText("Daily")
    }

    async toHaveWeeklyUpdates (label: string): Promise<void> {
        await this.sourcePanel.openPanel()
        expect(await this.sourcePanel.getSourceItemNextUpdate(label)).toContainText("Weekly")
    }

    async toHaveMonthlyUpdates (label: string): Promise<void> {
        await this.sourcePanel.openPanel()
        expect(await this.sourcePanel.getSourceItemNextUpdate(label)).toContainText("Monthly")
    }
}