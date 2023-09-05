import accessibleAutocomplete from 'accessible-autocomplete'
const XMLHttpRequest = require('xhr2')

export class Autocomplete {
  loadTrustSearch = (inputId, hiddenFieldId, autocompleteContainerId) => {
    let loading = false

    function suggest (query, populateResults) {
      if (query && !loading) {
        loading = true
        const http = new XMLHttpRequest()
        http.onload = function () {
          populateResults(JSON.parse(this.responseText))
          loading = false
        }

        http.open('GET', `/search?handler=populateautocomplete&keywords=${query}`, true)
        http.send()
      }
    }

    accessibleAutocomplete({
      element: document.querySelector(autocompleteContainerId),
      id: inputId,
      name: 'keywords',
      source: suggest,
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
          document.querySelector(hiddenFieldId).value = (selected.trustId)
        }
      }
    })
  }
}
