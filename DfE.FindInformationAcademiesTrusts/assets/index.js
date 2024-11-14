import { initAll as govukFrontendInitAll } from 'govuk-frontend'
import { Autocomplete } from './javascripts/autocomplete'
import $ from 'jquery'
import { SortableTable, nodeListForEach } from '@ministryofjustice/frontend'

window.$ = $

govukFrontendInitAll()

// Sortable table initialisation from the MOJ Frontend library
const sortableTables = document.querySelectorAll(
  '[data-module="moj-sortable-table"]'
)
nodeListForEach(
  sortableTables,
  (table) =>
    new SortableTable({
      table
    })
)

// Add Autocomplete namespace so that exposed methods can be invoked from Razor Partial
window.Autocomplete = new Autocomplete()
