import { test } from '@playwright/test'
import { FakeTestData } from '../../../fake-data/fake-test-data'
import { AcademiesDetailsPage } from '../../../page-object-model/trust/academies/details-page'

test.describe('Academies in trust details page', () => {
  let detailsPage: AcademiesDetailsPage

  test.beforeEach(async ({ page }) => {
    detailsPage = new AcademiesDetailsPage(page, new FakeTestData())
    await detailsPage.goTo()
  })

  test('user should see the right information about a trust', async () => {
    await detailsPage.expect.toBeOnTheRightPage()
    await detailsPage.expect.toDisplayInformationForAllAcademiesInThatTrust()
    await detailsPage.expect.toDisplayCorrectInformationAboutAcademiesInThatTrust()
  })
})
