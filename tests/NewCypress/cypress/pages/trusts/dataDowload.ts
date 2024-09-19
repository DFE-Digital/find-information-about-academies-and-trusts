export class DataDownload {
    private downloadButton: string;
    private downloadFolder: string;

    constructor() {
        this.downloadButton = '[data-testid="export-academy-data"]';
        this.downloadFolder = 'cypress/downloads';
    }

    clickDownloadButton(): void {
        cy.get(this.downloadButton).click();
    }

    findLatestDownloadedFile(): Cypress.Chainable<string> {

        return cy.task('findLatestFile', this.downloadFolder).then((latestFile) => {
            if (!latestFile) {
                throw new Error('No downloaded file found');
            }
            return latestFile as string; 
        });
    }

    checkFileDownloaded(): void {
        this.findLatestDownloadedFile().then((downloadFilePath) => {
            cy.readFile(downloadFilePath, 'binary', { timeout: 10000 }).should('exist');
        });
    }

    checkFileHasContent(): void {
        this.findLatestDownloadedFile().then((downloadFilePath) => {
            cy.readFile(downloadFilePath, 'binary', { timeout: 10000 }).then((fileContent) => {
                expect(fileContent.length).to.be.greaterThan(0);
            });
        });
    }

    deleteDownloadedFile(): void {
        this.findLatestDownloadedFile().then((downloadFilePath) => {
            cy.task<{ success: boolean; message?: string }>('deleteFile', downloadFilePath).then((result) => {
                if (!result.success) {
                    cy.log(result.message || 'Failed to delete file');
                }
            });
        });
    }
}

const dataDownload = new DataDownload();
export default dataDownload;
