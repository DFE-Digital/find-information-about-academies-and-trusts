import { test } from '@playwright/test'
import { DetailsPage } from '../../page-object-model/trust/details-page'
import { ContactsPage } from '../../page-object-model/trust/contacts-page'

test.describe('Details page', () => {
  let contactsPage: ContactsPage
  let detailsPage: DetailsPage

  test.beforeEach(async ({ page }) => {
    contactsPage = new ContactsPage(page)
    detailsPage = new DetailsPage(page)
    await contactsPage.goTo()
  })

  test('user should see trust name and type', async () => {
    await contactsPage.expect.toSeeCorrectTrustNameAndTypeInHeader()
  })

  test('user should be able to navigate between different sections about a trust', async () => {
    await contactsPage.trustNavigation.expect.toBeVisible()
    await contactsPage.trustNavigation.clickOn('Details')
    await detailsPage.expect.toBeOnTheRightPage()
  })
})
