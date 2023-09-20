import { expect, test } from '@playwright/test'
import { DetailsPage } from '../../page-object-model/trust/details-page'
import AxeBuilder from '@axe-core/playwright'

test.describe('Details page', () => {
  test('should not have any automatically detectable accessibility issues', async ({ page }) => {
    const detailsPage = new DetailsPage(page)
    await detailsPage.goTo()
    await detailsPage.expect.toSeeCorrectTrustNameAndTypeInHeader()
    await detailsPage.trustNavigation.expect.toBeVisible()

    const accessibilityScanResults = await new AxeBuilder({ page })
      .withTags(['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa'])
      .analyze()

    expect(accessibilityScanResults.violations).toEqual([])
  })
})
