import { AuthenticationInterceptorParams } from '../auth/authenticationInterceptor';
import './commands'

declare global {
    namespace Cypress {
        interface Chainable {
            login(params?: AuthenticationInterceptorParams): Chainable<Element>;
        }
    }
}