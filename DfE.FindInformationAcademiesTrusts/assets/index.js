import GOVUKFrontend from 'govuk-frontend/govuk/all'
import { Autocomplete } from './javascripts/autocomplete'
import SideNavigation from '@scottish-government/pattern-library/src/components/side-navigation/side-navigation'

GOVUKFrontend.initAll()

const sideNavs = [].slice.call(document.querySelectorAll('[data-module="ds-side-navigation"]'))
sideNavs.forEach(accordion =>
  new SideNavigation(accordion).init()
)

// Add Autocomplete namespace so that exposed methods can be invoked from Razor Partial
window.Autocomplete = new Autocomplete()
