import GOVUKFrontend from 'govuk-frontend/govuk/all'
import { Autocomplete } from './javascripts/autocomplete'
import SideNavigation from '@scottish-government/pattern-library/src/components/side-navigation/side-navigation'
import $ from 'jquery'
import { SortableTable, nodeListForEach } from '@ministryofjustice/frontend'

window.$ = $

GOVUKFrontend.initAll()

// Sortable table initialisation from the MOJ Frontend library
const sortableTables = document.querySelectorAll('[data-module="moj-sortable-table"]')
nodeListForEach(sortableTables, table =>
  new SortableTable({
    table
  })
)

const sideNavs = [].slice.call(document.querySelectorAll('[data-module="ds-side-navigation"]'))
sideNavs.forEach(accordion =>
  new SideNavigation(accordion).init()
)

// Add Autocomplete namespace so that exposed methods can be invoked from Razor Partial
window.Autocomplete = new Autocomplete()
