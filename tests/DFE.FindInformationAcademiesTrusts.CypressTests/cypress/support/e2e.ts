import { AutomationUserProperties } from '../auth/authenticationInterceptor';
import './commands'

declare global {
    namespace Cypress {
        interface Chainable {
            login(automationUserProperties?: AutomationUserProperties): Chainable<Element>;
        }
    }
}
