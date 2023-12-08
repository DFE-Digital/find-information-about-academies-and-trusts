import { Locator, Page } from '@playwright/test'
import { CurrentSearch, SearchFormComponent } from './shared/search-form-component'
import { BasePage } from './base-page'

export class HomePage extends BasePage {
  readonly searchForm: SearchFormComponent
  readonly _searchBoxLocator: Locator
  readonly _searchButtonLocator: Locator

  constructor (page: Page, currentSearch: CurrentSearch) {
    super(page, '/', 'Home page - Find information about academies and trusts', 'Find information about academies and trusts')

    this.searchForm = new SearchFormComponent(
      page,
      'Find information about academies and trusts',
      currentSearch
    )
  }
}
