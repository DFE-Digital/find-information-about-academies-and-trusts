import { test } from '../a11y-test'
import { ContactsPage } from '../../page-object-model/trust/contacts-page'
import { FakeTestData } from '../../fake-data/fake-test-data'

test.describe('Contacts page', () => {
  test('should not have any automatically detectable accessibility issues', async ({ expectNoAccessibilityViolations, page }) => {
    const contactsPage = new ContactsPage(page, new FakeTestData())
    await contactsPage.goTo()
    await contactsPage.expect.toSeeCorrectTrustNameAndTypeInHeader()
    await contactsPage.trustNavigation.expect.toBeVisible()
    await contactsPage.expect.toSeeCorrectDfeContacts()
    await contactsPage.expect.toSeeCorrectTrustContacts()
    await expectNoAccessibilityViolations()
  })
})
