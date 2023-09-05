import accessibleAutocomplete from 'accessible-autocomplete'

export class Autocomplete {
  suggest = async (query, populateResults) => {
    if (query) {
      const response = await fetch(`/search?handler=populateautocomplete&keywords=${query}`)
      const results = await response.json()
      populateResults(results)
    }
  }

  loadTrustSearch = async (inputId) => {
    const autocompleteTemplate = document.getElementById(`${inputId}-js-autocomplete-template`)
    const autocompleteTemplateContents = autocompleteTemplate.content.cloneNode(true)
    const elementToReplace = document.getElementById(`${inputId}-no-js-search-container`)

    elementToReplace.replaceWith(autocompleteTemplateContents)
    
    accessibleAutocomplete({
      element: document.getElementById(`${inputId}-autocomplete-container`),
      id: inputId,
      name: 'keywords',
      source: this.suggest,
      autoselect: false,
      confirmOnBlur: false,
      showNoOptionsFound: false,
      displayMenu: 'overlay',
      minLength: 3,
      templates: {
        inputValue: function (r) { return r && r.name },
        suggestion: function (r) { return r && `${r.name}<span class="autocomplete__option-hint">${r.address}</span>` }
      },
      onConfirm: (selected) => {
        if (selected) {
          document.getElementById(`${inputId}-selected-trust`).value = (selected.trustId)
        }
      }
    })
  }
}
