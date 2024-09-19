import dataDownload from "../../../pages/trusts/dataDowload";

describe('Trust export and content verification', () => {
  beforeEach(() => {
    cy.login()
    cy.visit('/trusts/academies/details?uid=5712');
  });

  it('should export a trust and verify it has downloaded and has content', () => {
    dataDownload.clickDownloadButton();
    dataDownload.checkFileDownloaded();
    dataDownload.checkFileHasContent();
    dataDownload.deleteDownloadedFile();
  });
});
