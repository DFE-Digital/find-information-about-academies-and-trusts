import { test } from '@playwright/test'
import { DetailsPage } from '../../page-object-model/trust/details-page'
import { ContactsPage } from '../../page-object-model/trust/contacts-page'

test.describe('Trust navigation', () => {
  let detailsPage: DetailsPage
  let contactsPage: ContactsPage

  test.beforeEach(async ({ page }) => {
    detailsPage = new DetailsPage(page)
    contactsPage = new ContactsPage(page)
  })

  test.describe('Given a user navigates to trust details', () => {
    test.beforeEach(async () => {
      await detailsPage.goTo()
    })

    test('user should be able to navigate between different sections about a trust', async () => {
      await detailsPage.trustNavigation.expect.toBeVisible()
      await detailsPage.trustNavigation.clickOn('Contacts')
      await contactsPage.expect.toBeOnTheRightPage()
      await contactsPage.trustNavigation.clickOn('Details')
      await detailsPage.expect.toBeOnTheRightPage()
    })
  })
})
