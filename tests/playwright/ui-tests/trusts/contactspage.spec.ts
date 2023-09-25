import { test } from '@playwright/test'
import { ContactsPage } from '../../page-object-model/trust/contacts-page'

test.describe('Details page', () => {
  let contactsPage: ContactsPage

  test.beforeEach(async ({ page }) => {
    contactsPage = new ContactsPage(page)
    await contactsPage.goTo()
  })

  test('user should see trust name and type', async () => {
    await contactsPage.expect.toSeeCorrectTrustNameAndTypeInHeader()
  })
})
