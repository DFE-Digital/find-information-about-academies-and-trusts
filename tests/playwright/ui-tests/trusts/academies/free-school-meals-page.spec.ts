import { test } from '@playwright/test'
import { FakeTestData } from '../../../fake-data/fake-test-data'
import { AcademiesFreeSchoolMealsPage } from '../../../page-object-model/trust/academies/free-school-meals-page'

test.describe('Academies in trust details page', () => {
  test('user should see the right information about a trust', async ({ page }) => {
    const freeSchoolMealsPage = new AcademiesFreeSchoolMealsPage(page, new FakeTestData())

    await freeSchoolMealsPage.goTo()
    await freeSchoolMealsPage.expect.toBeOnTheRightPage()
    await freeSchoolMealsPage.expect.toDisplayInformationForAllAcademiesInThatTrust()
    await freeSchoolMealsPage.expect.toDisplayCorrectInformationAboutAcademiesInThatTrust()
  })
})
