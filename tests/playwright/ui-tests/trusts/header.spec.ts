import { test } from '@playwright/test'
import { DetailsPage } from '../../page-object-model/trust/details-page'
import { FakeTestData } from '../../fake-data/fake-test-data'

test.describe('Trust header', () => {
  let detailsPage: DetailsPage

  test.beforeEach(async ({ page }) => {
    const fakeTestData = new FakeTestData()
    detailsPage = new DetailsPage(page, fakeTestData)
  })

  test.describe('user should see the trust type in the header', () => {
    test('when user navigates to multi-academy trust', async () => {
      await detailsPage.goToOpenMultiAcademyTrust()
      await detailsPage.trustHeading.expect.toHaveMultiAcademyTrustType()
    })

    test('when user navigates to single-academy trust', async () => {
      await detailsPage.goToOpenSingleAcademyTrust()
      await detailsPage.trustHeading.expect.toHaveSingleAcademyTrustType()
    })
  })
})
