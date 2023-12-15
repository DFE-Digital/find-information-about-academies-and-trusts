import { test } from '@playwright/test'
import { ContactsPage } from '../../page-object-model/trust/contacts-page'
import { NotFoundPage } from '../../page-object-model/not-found-page'
import { FakeTestData } from '../../fake-data/fake-test-data'
import { DataSourcePanelItem } from '../../page-object-model/trust/sources-and-updates'

test.describe('Contacts page', () => {
  let contactsPage: ContactsPage
  let notFoundPage: NotFoundPage
  const sources: DataSourcePanelItem[] = [{ fields: 'DfE Contacts:', dataSource: 'RSD service support team', update: 'Daily' },
    { fields: 'Accounting Officer Name, Chief Financial Officer Name, Chair of trustees Name:', dataSource: 'Get information about schools', update: 'Daily' },
    { fields: 'Accounting Officer Email, Chief Financial Officer Email, Chair of trustees Email:', dataSource: 'Get information about schools', update: 'Daily' }]

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
    await contactsPage.sourcePanel.openPanel()
    for (const source of sources) {
      await contactsPage.expect.toSeeCorrectSourceAndUpdates(source)
    }
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
