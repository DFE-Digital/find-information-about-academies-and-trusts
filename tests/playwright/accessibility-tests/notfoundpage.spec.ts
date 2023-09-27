import { expect, test } from '@playwright/test'
import AxeBuilder from '@axe-core/playwright'
import { NotFoundPage } from '../page-object-model/not-found-page'

test.describe('Page not found', () => {
  let notFoundPage: NotFoundPage

  test.beforeEach(async ({ page }) => {
    notFoundPage = new NotFoundPage(page)
  })

  test('when a user tries to type in a url that does not exist', async ({ page }) => {
    await notFoundPage.goToNonExistingUrl()
    await notFoundPage.expect.toBeShownNotFoundMessage()

    const accessibilityScanResults = await new AxeBuilder({ page })
      .withTags(['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa'])
      .analyze()

    expect(accessibilityScanResults.violations).toEqual([])
  })
})
