import { test } from '@playwright/test'
import { ContactsPage } from '../../page-object-model/trust/contacts-page'
import { NotFoundPage } from '../../page-object-model/not-found-page'
import { FakeTestData } from '../../fake-data/fake-test-data'

test.describe('Contacts page', () => {
  let contactsPage: ContactsPage
  let notFoundPage: NotFoundPage

  test.beforeEach(async ({ page }) => {
    contactsPage = new ContactsPage(page, new FakeTestData())
    await contactsPage.goTo()
  })

  test('user should see trust name and type', async () => {
    await contactsPage.expect.toSeeCorrectTrustNameAndTypeInHeader()
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
