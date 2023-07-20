import { defineConfig, devices } from '@playwright/test'
import * as dotenv from 'dotenv'

dotenv.config()

/**
 * See https://playwright.dev/docs/test-configuration.
 */
export default defineConfig({
  testDir: '.',
  /* Run tests in files in parallel */
  fullyParallel: true,
  /* Fail the build on CI if you accidentally left test.only in the source code. */
  forbidOnly: !!process.env.CI,
  /* Retry on CI only */
  retries: process.env.CI ? 1 : 0,
  /* Reporter to use. See https://playwright.dev/docs/test-reporters */
  reporter: 'html',

  /* Shared settings for all the projects below. See https://playwright.dev/docs/api/class-testoptions. */
  use: {
    /* Base URL to use in actions like `await page.goto('/')`. */
    baseURL: process.env.PLAYWRIGHT_BASEURL,

    /* Collect trace when retrying the failed test. See https://playwright.dev/docs/trace-viewer */
    trace: 'on-first-retry'
  },

  /* Configure projects for major browsers */
  projects: [
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] }
    },

    {
      name: 'firefox',
      use: { ...devices['Desktop Firefox'] }
    },

    {
      name: 'webkit',
      use: { ...devices['Desktop Safari'] }
    },
    {
      name: 'Microsoft Edge',
      use: { ...devices['Desktop Edge'], channel: 'msedge' },
    },
    {
      name: 'cross-browser',
      dependencies: ['chromium', 'firefox', 'webkit', 'Microsoft Edge'],
      testIgnore:'./**'
    },
    {
      name: 'deployment-tests',
      use: {
        ...devices['Desktop Chrome'],
        channel: 'chrome',
      },
      testDir: './deployment-tests'
    },
    {
      name: 'zap-tests',
      use: {
        ...devices['Desktop Chrome'],
        channel: 'chrome',
        ignoreHTTPSErrors: true,
        launchOptions: {
          /* Browser proxy option is required for Chromium on Windows. */
          proxy: { server: 'per-context' }
        },
        proxy: {
          server: process.env.HTTP_PROXY!,
          bypass: process.env.NO_PROXY
        }
      },
      teardown: 'generate ZAP report',
      /* use accessibility tests because they go through each screen of the application */
      testDir: './accessibility-tests',
    },
    {
      name: 'generate ZAP report',
      testMatch: /global.teardown\.ts/,
    }
  ]
})
