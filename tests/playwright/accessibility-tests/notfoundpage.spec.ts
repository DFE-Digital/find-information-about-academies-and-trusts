import { test } from './a11y-test'
import { NotFoundPage } from '../page-object-model/not-found-page'

test.describe('Page not found', () => {
  let notFoundPage: NotFoundPage

  test.beforeEach(async ({ page }) => {
    notFoundPage = new NotFoundPage(page)
  })

  test('when a user tries to type in a url that does not exist', async ({ expectNoAccessibilityViolations }) => {
    await notFoundPage.goToNonExistingUrl()
    await notFoundPage.expect.toBeShownNotFoundMessage()

    await expectNoAccessibilityViolations()
  })
})
