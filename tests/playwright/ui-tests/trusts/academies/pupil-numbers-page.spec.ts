import { test } from '@playwright/test'
import { FakeTestData } from '../../../fake-data/fake-test-data'
import { AcademiesPupilNumbersPage } from '../../../page-object-model/trust/academies/pupil-numbers-page '
import { DataSourcePanelItem } from '../../../page-object-model/trust/sources-and-updates'

test.describe('Academies in trust pupil numbers page', () => {
  let pupilNumbersPage: AcademiesPupilNumbersPage

  test.beforeEach(async ({ page }) => {
    pupilNumbersPage = new AcademiesPupilNumbersPage(page, new FakeTestData())
    await pupilNumbersPage.goTo()
  })

  test('user should see the right information about a trust', async () => {
    await pupilNumbersPage.expect.toBeOnTheRightPage()
    await pupilNumbersPage.expect.toDisplayInformationForAllAcademiesInThatTrust()
    await pupilNumbersPage.expect.toDisplayCorrectInformationAboutAcademiesInThatTrust()
    const source:DataSourcePanelItem = {fields: "Pupil numbers:", dataSource: "Get information about schools", update:"Daily"}
    await pupilNumbersPage.expect.toSeeCorrectSourceAndUpdates(source)
  })
})
