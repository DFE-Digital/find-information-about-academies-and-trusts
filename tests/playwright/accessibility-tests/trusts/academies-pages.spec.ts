import { FakeTestData } from '../../fake-data/fake-test-data'
import { AcademiesDetailsPage } from '../../page-object-model/trust/academies/details-page'
import { AcademiesFreeSchoolMealsPage } from '../../page-object-model/trust/academies/free-school-meals-page'
import { AcademiesOfstedRatingsPage } from '../../page-object-model/trust/academies/ofsted-ratings-page'
import { AcademiesPupilNumbersPage } from '../../page-object-model/trust/academies/pupil-numbers-page '
import { test } from '../a11y-test'

test.describe('Academies pages', () => {
  test('Each tab should not have any automatically detectable accessibility issues', async ({ expectNoAccessibilityViolations, page }) => {
    const fakeTestData = new FakeTestData()
    const academiesDetailsPage = new AcademiesDetailsPage(page, fakeTestData)
    const academiesOfstedRatingsPage = new AcademiesOfstedRatingsPage(page, fakeTestData)
    const academiesPupilNumbersPage = new AcademiesPupilNumbersPage(page, fakeTestData)
    const academiesFreeSchoolMealsPage = new AcademiesFreeSchoolMealsPage(page, fakeTestData)

    await academiesDetailsPage.goTo()
    await academiesDetailsPage.expect.toBeOnTheRightPage()
    await expectNoAccessibilityViolations()

    await academiesDetailsPage.subNavigation.clickOn('Ofsted ratings')
    await academiesOfstedRatingsPage.expect.toBeOnTheRightPage()
    await expectNoAccessibilityViolations()

    await academiesOfstedRatingsPage.subNavigation.clickOn('Pupil numbers')
    await academiesPupilNumbersPage.expect.toBeOnTheRightPage()
    await expectNoAccessibilityViolations()

    await academiesPupilNumbersPage.subNavigation.clickOn('Free school meals')
    await academiesFreeSchoolMealsPage.expect.toBeOnTheRightPage()
    await expectNoAccessibilityViolations()
  })
})
