import { FakeTestData } from '../../fake-data/fake-test-data'
import { AcademiesDetailsPage } from '../../page-object-model/trust/academies/details-page'
import { AcademiesOfstedRatingsPage } from '../../page-object-model/trust/academies/ofsted-ratings-page'
import { test } from '../a11y-test'

test.describe('Academies pages', () => {
  test('Each tab should not have any automatically detectable accessibility issues', async ({ expectNoAccessibilityViolations, page }) => {
    const fakeTestData = new FakeTestData()
    const academiesDetailsPage = new AcademiesDetailsPage(page, fakeTestData)
    const academiesOfstedRatingsPage = new AcademiesOfstedRatingsPage(page, fakeTestData)
    await academiesDetailsPage.goTo()
    await academiesDetailsPage.expect.toBeOnTheRightPage()
    await expectNoAccessibilityViolations()

    await academiesDetailsPage.subNavigation.clickOn('Ofsted ratings')
    await academiesOfstedRatingsPage.expect.toBeOnTheRightPage()
    await expectNoAccessibilityViolations()
  })
})
