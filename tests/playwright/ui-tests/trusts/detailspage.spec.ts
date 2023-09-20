import { test } from '@playwright/test'
import { DetailsPage } from '../../page-object-model/trust/details-page'
import { ContactsPage } from '../../page-object-model/trust/contacts-page'

test.describe('Details page', () => {
  let detailsPage: DetailsPage
  let contactsPage: ContactsPage

  test.beforeEach(async ({ page }) => {
    detailsPage = new DetailsPage(page)
    contactsPage = new ContactsPage(page)
    await detailsPage.goTo()
  })

  test('user should see trust name and type', async () => {
    await detailsPage.expect.toSeeCorrectTrustNameAndTypeInHeader()
  })

  test('user should be able to navigate between different sections about a trust', async () => {
    await detailsPage.trustNavigation.expect.toBeVisible()
    await detailsPage.trustNavigation.clickOn('Contacts')
    await contactsPage.expect.toBeOnTheRightPage()
  })
})
