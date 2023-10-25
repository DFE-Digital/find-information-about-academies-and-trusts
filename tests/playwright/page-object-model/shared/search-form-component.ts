import { Locator, Page, expect } from '@playwright/test'

class SelectedTrust {
  name = ''
  address = ''
}

export class CurrentSearch {
  term = ''
  selectedTrust = new SelectedTrust()
}

export class SearchFormComponent {
  readonly expect: SearchFormComponentAssertions
  readonly searchFormLocator: Locator
  readonly searchInputLocator: Locator
  readonly searchButtonLocator: Locator

  readonly searchTerms = ['trust', 'education']
  currentSearch: CurrentSearch
  searchNumber = 0
  itemToSelect = 0

  constructor (readonly page: Page, label: string, currentSearch: CurrentSearch) {
    this.expect = new SearchFormComponentAssertions(this)
    this.searchFormLocator = page.getByTestId('app-search-form')
    this.searchInputLocator = this.searchFormLocator.getByLabel(label)
    this.searchButtonLocator = this.searchFormLocator.getByRole('button', { name: 'Search' })
    this.currentSearch = currentSearch
  }

  async typeASearchTerm (): Promise<void> {
    this.currentSearch.term = 'trust'
    await this.searchInputLocator.fill(this.currentSearch.term)
  }

  async typeADifferentSearchTerm (): Promise<void> {
    this.currentSearch.term = 'education'
    await this.searchInputLocator.fill(this.currentSearch.term)
  }

  async typeASearchTermWithNoMatches (): Promise<void> {
    this.currentSearch.term = 'non'
    await this.searchInputLocator.fill(this.currentSearch.term)
  }

  async searchForATrust (): Promise<void> {
    await this.typeASearchTerm()
    await this.submitSearch()
  }

  async searchForADifferentTrust (): Promise<void> {
    await this.typeADifferentSearchTerm()
    await this.submitSearch()
  }

  async submitSearch (): Promise<void> {
    await this.searchButtonLocator.click()
  }

  async chooseItemFromAutocomplete (): Promise<void> {
    // We always select at least the second item, due to an issue when the user focuses on the autocomplete input when it has a default value https://github.com/alphagov/accessible-autocomplete/issues/424
    // the first suggestion will be the value until the user changes the search
    const itemToSelect = this.searchFormLocator.getByRole('option').nth(1)
    const itemText = (await itemToSelect.innerText()).split('\n')

    this.currentSearch.selectedTrust = {
      name: itemText[0],
      address: itemText[1]
    }

    await itemToSelect.click()
  }
}

class SearchFormComponentAssertions {
  constructor (readonly searchForm: SearchFormComponent) {}

  async inputToContainSearchTerm (): Promise<void> {
    await expect(this.searchForm.searchInputLocator).toHaveValue(this.searchForm.currentSearch.term)
  }

  async inputToContainNoSearchTerm (): Promise<void> {
    await expect(this.searchForm.searchInputLocator).toHaveValue('')
  }

  async toshowNoResultsFoundInAutocomplete (): Promise<void> {
    await expect(this.searchForm.searchFormLocator.getByRole('listitem').first()).toHaveText('No results found')
    await expect(this.searchForm.searchFormLocator.getByRole('option')).toHaveCount(0)
  }

  async toShowAnySuggestionInAutocomplete (): Promise<void> {
    const firstResult = this.searchForm.searchFormLocator.getByRole('option').first()
    await expect(firstResult).toBeVisible()
  }
}
