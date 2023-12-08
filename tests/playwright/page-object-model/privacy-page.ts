import { Page } from '@playwright/test'
import { BasePage } from './base-page'

export class PrivacyPage extends BasePage {
  constructor (page: Page) {
    super(page, '/privacy', 'Privacy notice - Find information about academies and trusts', 'Privacy notice for Find information about academies and trusts')
  }
}
