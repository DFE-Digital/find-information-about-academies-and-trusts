import { test } from '../a11y-test'
import { OverviewPage } from '../../page-object-model/trust/overview-page'
import { FakeTestData } from '../../fake-data/fake-test-data'

test.describe('Overview page', () => {
  test('should not have any automatically detectable accessibility issues', async ({ expectNoAccessibilityViolations, page }) => {
    const overviewPage = new OverviewPage(page, new FakeTestData())
    await overviewPage.goTo()
    await overviewPage.expect.toBeOnTheRightPage()
    await overviewPage.expect.toSeeCorrectTrustNameAndTypeInHeader()
    await overviewPage.trustNavigation.expect.toBeVisible()
    await overviewPage.expect.toSeeCorrectTrustSummary()
    await overviewPage.expect.toSeeCorrectOfstedRatings()
    await expectNoAccessibilityViolations()
  })
})
