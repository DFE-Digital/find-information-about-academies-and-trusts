import { test } from '@playwright/test'
import { ContactsPage } from '../../page-object-model/trust/contacts-page'
import { NotFoundPage } from '../../page-object-model/not-found-page'
import { FakeTestData } from '../../fake-data/fake-test-data'

test.describe('Contacts page', () => {
  let contactsPage: ContactsPage
  let notFoundPage: NotFoundPage

  test.beforeEach(async ({ page }) => {
    contactsPage = new ContactsPage(page, new FakeTestData())
  })

  test('user should see trust name and type', async () => {
    await contactsPage.goTo()
    await contactsPage.expect.toSeeCorrectTrustNameAndTypeInHeader()
  })

  test('user should see the correct contact information when contact details fully populated', async () => {
    await contactsPage.goToTrustWithAllContactDetailsPopulated()
    await contactsPage.expect.toSeeCorrectDfeContacts()
    await contactsPage.expect.toSeeCorrectTrustContacts()
  })

  test('user should see missing information messages when dfe contact details not fully populated', async () => {
    await contactsPage.goToTrustWithDfeContactDetailsMissing()
    await contactsPage.expect.toSeeCorrectDfeContactsMissingInformationMessage()
  })

  test('user should see missing information messages when trust contact email not fully populated', async () => {
    await contactsPage.goToTrustWithTrustContactEmailMissing()
    await contactsPage.expect.toSeeCorrectTrustContactMissingEmailMessage()
  })

  test.describe('given a user tries to visit the url without an existing trust', () => {
    test.beforeEach(({ page }) => {
      notFoundPage = new NotFoundPage(page)
    })

    test('then they should see a not found message', async () => {
      await contactsPage.goToPageWithoutUid()
      await notFoundPage.expect.toBeShownNotFoundMessage()

      await contactsPage.goToPageWithUidThatDoesNotExist()
      await notFoundPage.expect.toBeShownNotFoundMessage()
    })
  })
})
