import { test } from '@playwright/test'
import { DetailsPage } from '../../page-object-model/trust/details-page'
import { ContactsPage } from '../../page-object-model/trust/contacts-page'
import { FakeTestData } from '../../fake-data/fake-test-data'
import { OverviewPage } from '../../page-object-model/trust/overview-page'

test.describe('Trust navigation', () => {
  let detailsPage: DetailsPage
  let contactsPage: ContactsPage
  let overviewPage: OverviewPage

  test.beforeEach(async ({ page }) => {
    const fakeTestData = new FakeTestData()
    detailsPage = new DetailsPage(page, fakeTestData)
    contactsPage = new ContactsPage(page, fakeTestData)
    overviewPage = new OverviewPage(page, fakeTestData)
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
})
