import { test } from '@playwright/test'
import { NotFoundPage } from '../page-object-model/not-found-page'

test.describe('Page not found', () => {
  let notFoundPage: NotFoundPage

  test.beforeEach(async ({ page }) => {
    notFoundPage = new NotFoundPage(page)
  })

  test('when a user tries to type in a url that does not exist', async () => {
    await notFoundPage.goToNonExistingUrl()
    await notFoundPage.expect.toBeShownNotFoundMessage()
  })
})
