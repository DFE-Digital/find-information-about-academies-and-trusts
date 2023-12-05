import { Locator, Page } from '@playwright/test'
import { CurrentSearch, SearchFormComponent } from './shared/search-form-component'
import { FooterNavigationComponent } from '../page-object-model/shared/footer-navigation-component'
import { CookieBannerComponent } from '../page-object-model/shared/cookie-banner-component'
import { BasePage } from './base-page'

export class HomePage extends BasePage {
  readonly searchForm: SearchFormComponent
  readonly footerNavigation: FooterNavigationComponent
  readonly cookieBanner: CookieBannerComponent
  readonly _searchBoxLocator: Locator
  readonly _searchButtonLocator: Locator

  constructor (page: Page, currentSearch: CurrentSearch) {
    super(page, '/', 'Home page - Find information about academies and trusts', 'Find information about academies and trusts')

    this.footerNavigation = new FooterNavigationComponent(page)
    this.cookieBanner = new CookieBannerComponent(page)
    this.searchForm = new SearchFormComponent(
      page,
      'Find information about academies and trusts',
      currentSearch
    )
  }
}
