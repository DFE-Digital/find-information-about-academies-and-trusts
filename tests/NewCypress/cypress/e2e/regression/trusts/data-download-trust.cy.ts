import dataDownload from "../../../pages/trusts/dataDownload";

describe('Trust export and content verification', () => {
  beforeEach(() => {
    cy.login();
    cy.visit('/trusts/academies/details?uid=5712');

    // Clear the downloads folder before running each test
    cy.task('checkForFiles', 'cypress/downloads').then((files) => {
      if (files) {
        cy.task('clearDownloads', 'cypress/downloads');
      }
    });
  });

  it('should export a trust and verify it has downloaded and has content', () => {
    dataDownload
      .clickDownloadButton()
      .checkFileDownloaded()
      .checkFileHasContent()
      .deleteDownloadedFile();
  });
});
