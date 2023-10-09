import { test } from '@playwright/test'
import { HealthPage } from '../page-object-model/health-page'

test.describe('Health status endpoint', () => {
  let healthPage: HealthPage
  test.beforeEach(({ page }) => {
    healthPage = new HealthPage(page)
  })

  test('User can navigate to the health endpoint', async () => {
    await healthPage.goTo()
    await healthPage.expect.toBeHealthy()
  })
})
