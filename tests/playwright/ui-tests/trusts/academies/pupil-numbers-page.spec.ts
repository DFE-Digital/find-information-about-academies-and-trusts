import { test } from '@playwright/test'
import { FakeTestData } from '../../../fake-data/fake-test-data'
import { AcademiesPupilNumbersPage } from '../../../page-object-model/trust/academies/pupil-numbers-page '
import { DataSourcePanelItem } from '../../../page-object-model/trust/sources-and-updates-component'

test.describe('Academies in trust pupil numbers page', () => {
  let pupilNumbersPage: AcademiesPupilNumbersPage
  const sources: DataSourcePanelItem[] = [{ fields: 'Pupil numbers:', dataSource: 'Get information about schools', update: 'Daily' }]

  test.beforeEach(async ({ page }) => {
    pupilNumbersPage = new AcademiesPupilNumbersPage(page, new FakeTestData())
    await pupilNumbersPage.goTo()
    await pupilNumbersPage.expect.toBeOnTheRightPage()
  })

  test('user should see the right information about a trust', async () => {
    await pupilNumbersPage.expect.toDisplayInformationForAllAcademiesInThatTrust()
    await pupilNumbersPage.expect.toDisplayCorrectInformationAboutAcademiesInThatTrust()
  })

  test('user sees the correct information in the source and updates panel', async () => {
    await pupilNumbersPage.expect.toSeeCorrectSourceAndUpdates(sources)
  })
})
