import { test } from '@playwright/test'
import { FakeTestData } from '../../../fake-data/fake-test-data'
import { AcademiesFreeSchoolMealsPage } from '../../../page-object-model/trust/academies/free-school-meals-page'
import { DataSourcePanelItem } from '../../../page-object-model/trust/sources-and-updates-component'

const sources: DataSourcePanelItem[] = [
  {
    fields: 'Pupils eligible for free school meals',
    dataSource: 'Get information about schools',
    update: 'Daily'
  },
  {
    fields: 'Local authority average 2022/23, National average 2022/23',
    dataSource: 'Explore education statistics',
    update: 'Annually'
  }
]

test.describe('Academies in trust details page', () => {
  let freeSchoolMealsPage: AcademiesFreeSchoolMealsPage

  test.beforeEach(async ({ page }) => {
    freeSchoolMealsPage = new AcademiesFreeSchoolMealsPage(page, new FakeTestData())
    await freeSchoolMealsPage.goTo()
    await freeSchoolMealsPage.expect.toBeOnTheRightPage()
  })

  test('user should see the right information about a trust', async ({ page }) => {
    await freeSchoolMealsPage.expect.toDisplayInformationForAllAcademiesInThatTrust()
    await freeSchoolMealsPage.expect.toDisplayCorrectInformationAboutAcademiesInThatTrust()
  })

  test('user sees the correct information in the source and updates panel', async () => {
    await freeSchoolMealsPage.expect.toSeeCorrectSourceAndUpdates(sources)
  })
})
