import { test } from '@playwright/test'
import { CurrentSearch } from '../page-object-model/shared/search-form-component'
import { HomePage } from '../page-object-model/home-page'
import { DetailsPage } from '../page-object-model/trust/details-page'
import { ContactsPage } from '../page-object-model/trust/contacts-page'
import { FakeTestData } from '../fake-data/fake-test-data'
import { OverviewPage } from '../page-object-model/trust/overview-page'
import { PrivacyPage } from '../page-object-model/privacy-page'

test.describe('Trust navigation', () => {
  let homePage: HomePage
  let currentSearch: CurrentSearch
  let detailsPage: DetailsPage
  let contactsPage: ContactsPage
  let overviewPage: OverviewPage
  let privacyPage: PrivacyPage

  test.beforeEach(async ({ page }) => {
    const fakeTestData = new FakeTestData()
    currentSearch = new CurrentSearch()
    homePage = new HomePage(page, currentSearch)
    detailsPage = new DetailsPage(page, fakeTestData)
    contactsPage = new ContactsPage(page, fakeTestData)
    overviewPage = new OverviewPage(page, fakeTestData)
    privacyPage = new PrivacyPage(page)
  })

  test('user should be able to navigate between different sections about a trust', async () => {
    await detailsPage.goTo()
    await detailsPage.trustNavigation.expect.toBeVisible()
    // Details => Contacts
    await detailsPage.trustNavigation.clickOn('Contacts')
    await contactsPage.expect.toBeOnTheRightPage()
    // Contacts => Details
    await contactsPage.trustNavigation.clickOn('Details')
    await detailsPage.expect.toBeOnTheRightPage()
    // Details => Overview
    await detailsPage.trustNavigation.clickOn('Overview')
    await overviewPage.expect.toBeOnTheRightPage()
    // Overview => Contacts
    await overviewPage.trustNavigation.clickOn('Contacts')
    await contactsPage.expect.toBeOnTheRightPage()
    // Contacts => Overview
    await contactsPage.trustNavigation.clickOn('Overview')
    await overviewPage.expect.toBeOnTheRightPage()
    // Overview => Details
    await overviewPage.trustNavigation.clickOn('Details')
    await detailsPage.expect.toBeOnTheRightPage()
  })

  test('user should be able to navigate to the different links within the footer', async () => {
  // Footer Via HomePage => Privacy
    await homePage.goTo()
    await homePage.footerNavigation.clickPrivacyPolicy()
    await privacyPage.expect.toBeOnTheRightPage()
  })
})
