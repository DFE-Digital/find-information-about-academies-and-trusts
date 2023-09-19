import { test } from '@playwright/test'
import { DetailsPage } from '../../page-object-model/trust/details-page'

test.describe('Details page', () => {
  let detailsPage: DetailsPage

  test.beforeEach(async ({ page }) => {
    detailsPage = new DetailsPage(page)
    await detailsPage.goTo()
  })

  test('user should see trust name and type', async () => {
    await detailsPage.expect.toSeeCorrectTrustNameAndTypeInHeader()
  })
})
