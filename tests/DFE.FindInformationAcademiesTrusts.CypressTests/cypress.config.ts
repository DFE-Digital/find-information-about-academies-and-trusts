const { defineConfig } = require("cypress");
const fs = require('fs');
const path = require('path');

module.exports = defineConfig({
  userAgent: 'FindInformationAcademiesTrusts/1.0 Cypress',
  e2e: {
    reporter: 'cypress-multi-reporters',
    reporterOptions: {
      reporterEnabled: 'mochawesome',
      mochawesomeReporterOptions: {
        reportDir: 'cypress/reports/mocha',
        quite: true,
        overwrite: false,
        html: false,
        json: true,
      },
    },
    setupNodeEvents(on, config) {
      config.baseUrl = config.env.URL;
      // Custom task to find the most recent .xlsx file in the downloads folder
      on('task', {
        findLatestFile(folderPath) {
          const files = fs.readdirSync(folderPath);
          const xlsxFiles = files.filter(file => file.endsWith('.xlsx'));

          if (xlsxFiles.length === 0) return null;

          // Sort files by modified date, latest first
          const latestFile = xlsxFiles
            .map(fileName => ({
              name: fileName,
              time: fs.statSync(path.join(folderPath, fileName)).mtime.getTime()
            }))
            .sort((a, b) => b.time - a.time)[0];

          return path.join(folderPath, latestFile.name);
        },

        // Custom task to delete a file
        deleteFile(filePath) {
          if (fs.existsSync(filePath)) {
            fs.unlinkSync(filePath);
            return { success: true };
          }
          return { success: false, message: 'File not found' };
        },

        // Custom task to check if files exist in the downloads folder
        checkForFiles(folderPath) {
          const files = fs.readdirSync(folderPath);
          return files.length > 0 ? files : null;
        },

        // Custom task to delete all files in the downloads folder
        clearDownloads(folderPath) {
          const files = fs.readdirSync(folderPath);
          files.forEach((file) => {
            fs.unlinkSync(path.join(folderPath, file));
          });
          return { success: true };
        }
      });

      return config;
    },

    downloadsFolder: 'cypress/downloads',
  },
});
