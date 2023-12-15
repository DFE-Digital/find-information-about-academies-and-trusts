import { test } from '@playwright/test'
import { FakeTestData } from '../../../fake-data/fake-test-data'
import { AcademiesOfstedRatingsPage } from '../../../page-object-model/trust/academies/ofsted-ratings-page'
import { DataSourcePanelItem } from '../../../page-object-model/trust/sources-and-updates-component'

test.describe('Academies in trust ofsted ratings page', () => {
  test('user should see the right information about a trust', async ({ page }) => {
    const ofstedRatingsPage = new AcademiesOfstedRatingsPage(page, new FakeTestData())
    await ofstedRatingsPage.goTo()
    await ofstedRatingsPage.expect.toBeOnTheRightPage()
    await ofstedRatingsPage.expect.toDisplayInformationForAllAcademiesInThatTrust()
    await ofstedRatingsPage.expect.toDisplayCorrectInformationAboutAcademiesInThatTrust()
    await ofstedRatingsPage.sourcePanel.openPanel()
    const sources: DataSourcePanelItem[] = [
      {
        fields: 'Date joined trust',
        dataSource: 'Get information about schools',
        update: 'Daily'
      },
      {
        fields: 'Current Ofsted rating, Date of last inspection, Previous Ofsted rating, Date of previous inspection:',
        dataSource: 'State-funded school inspections and outcomes: management information',
        update: 'Monthly'
      }
    ]
    for (const source of sources) {
      await ofstedRatingsPage.expect.toSeeCorrectSourceAndUpdates(source)
    }
  })
})
