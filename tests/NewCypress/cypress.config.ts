const { defineConfig } = require("cypress");
const fs = require('fs');
const path = require('path');

module.exports = defineConfig({
  e2e: {
    setupNodeEvents(on, config) {
      config.baseUrl = config.env.url;

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
        }
      });

      return config;
    },
    downloadsFolder: 'cypress/downloads',
  },
});
