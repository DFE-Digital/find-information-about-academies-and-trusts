import { test as base, expect } from '@playwright/test'
import AxeBuilder from '@axe-core/playwright'

interface A11yTest {
  expectNoAccessibilityViolations: () => Promise<void>
}

export const test = base.extend<A11yTest>({
  expectNoAccessibilityViolations: async ({ page }, use) => {
    const accessibilityScanner = new AxeBuilder({ page })
      .withTags(['wcag2a', 'wcag2aa', 'wcag21a', 'wcag21aa'])

    const doScan: () => Promise<void> = async () => {
      const accessibilityScanResults = await accessibilityScanner.analyze()

      expect(accessibilityScanResults.violations).toEqual([])
    }

    await use(doScan)
  }
})
