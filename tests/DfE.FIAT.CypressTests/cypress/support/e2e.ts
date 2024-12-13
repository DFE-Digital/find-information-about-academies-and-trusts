/* eslint-disable @typescript-eslint/no-namespace */ //required to enable custom Cypress commands typescript support
import { AuthenticationInterceptorParams } from '../auth/authenticationInterceptor';
import './commands'

declare global {
    namespace Cypress {
        interface Chainable {
            login(params?: AuthenticationInterceptorParams): Chainable<Element>;
        }
    }
}
