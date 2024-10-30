/* eslint-disable @typescript-eslint/no-namespace */ //required to enable custom Cypress commands typescript support
import { AutomationUserProperties } from '../auth/authenticationInterceptor';
import './commands';

declare global {
    namespace Cypress {
        interface Chainable {
            login(automationUserProperties?: AutomationUserProperties): Chainable<Element>;
        }
    }
}
