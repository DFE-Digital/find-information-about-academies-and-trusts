import GOVUKFrontend from 'govuk-frontend/govuk/all'
import { Autocomplete } from './javascripts/autocomplete'

GOVUKFrontend.initAll()

// Add Autocomplete namespace so that exposed methods can be invoked from Razor Partial
window.Autocomplete = new Autocomplete()
