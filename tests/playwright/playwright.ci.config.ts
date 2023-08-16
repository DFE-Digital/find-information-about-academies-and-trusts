import { baseConfig } from './playwright.config'
import { defineConfig, devices } from '@playwright/test'

export default defineConfig({
  ...baseConfig,
  projects: [
    {
      name: 'mock-dependencies',
      testMatch: /.mocks.setup\.ts/
    },
    {
      name: 'chromium',
      use: { ...devices['Desktop Chrome'] },
      dependencies: ['mock-dependencies']
    },

    {
      name: 'firefox',
      use: { ...devices['Desktop Firefox'] },
      dependencies: ['mock-dependencies']
    },

    {
      name: 'webkit',
      use: { ...devices['Desktop Safari'] },
      dependencies: ['mock-dependencies']
    },
    {
      name: 'Microsoft Edge',
      use: { ...devices['Desktop Edge'], channel: 'msedge' },
      dependencies: ['mock-dependencies']
    }
  ]
})
