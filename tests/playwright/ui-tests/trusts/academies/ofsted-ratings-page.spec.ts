import { test } from '@playwright/test'
import { FakeTestData } from '../../../fake-data/fake-test-data'
import { AcademiesOfstedRatingsPage } from '../../../page-object-model/trust/academies/ofsted-ratings-page'

test.describe('Academies in trust ofsted ratings page', () => {
  test('user should see the right information about a trust', async ({ page }) => {
    const ofstedRatingsPage = new AcademiesOfstedRatingsPage(page, new FakeTestData())
    await ofstedRatingsPage.goTo()
    await ofstedRatingsPage.expect.toBeOnTheRightPage()
    await ofstedRatingsPage.expect.toDisplayInformationForAllAcademiesInThatTrust()
    await ofstedRatingsPage.expect.toDisplayCorrectInformationAboutAcademiesInThatTrust()
  })
})
