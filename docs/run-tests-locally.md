# Run tests locally

Use this documentation to run tests locally.

- [Unit tests](#unit-tests)
- [Accessibility and UI tests](#accessibility-and-ui-tests)
- [Security testing with zap](#security-testing-with-zap)

## Unit tests

All .NET code is [unit tested](./test-approach.md) where possible.

You can run unit tests using your preferred IDE, or open a terminal in the project directory and run:

```bash
dotnet test
```

## Accessibility and UI tests

Accessibility and UI tests are written using [Cypress](https://www.cypress.io/), with external dependencies mocked (e.g. the database) or bypassed (e.g. the auth). You can run the tests against a docker instance, or run the application locally.

1. Ensure the Docker engine is running.
1. Create the file `tests/DFE.FindInformationAcademiesTrusts.CypressTests/cypress.env.json`

```json
{
    "url": "http://localhost/",
    "authKey": "TestSuperSecret"
}
```

1. Open a terminal in your repository and run:

    ```bash
    cd tests/DFE.FindInformationAcademiesTrusts.CypressTests

    # install dependencies
    npm install

    # run tests 
    npm run cy:run # run tests in headless mode
    npm run cy:open # run tests in headed mode

    ```

If you need to update the mocked test data for UI and Accessibility tests see [update test data](./update-test-data.md)

### Security testing with ZAP

The Cypress tests can also be run, proxied via [OWASP ZAP](https://zaproxy.org) for passive security scanning of the application.

These can be run using the configured `docker-compose.yml`, which will spin up containers for the ZAP daemon and the Cypress tests, including all networking required. You will need to update any config in the file before running

Create a `.env` file for docker, this file needs to include

- all of your required cypress configuration
- HTTP_PROXY e.g. `http://zap:8080`
- ZAP_API_KEY, can be any random guid

Example env:

```env
URL=<Enter URL>
USERNAME=<Enter username>
API=<Enter API>
API_KEY=<Enter API key>
AUTH_KEY=<Enter auth key>
HTTP_PROXY=http://zap:8080
ZAP_API_KEY=<Enter random guid>

```

**Note**: You might have trouble running this locally because of docker thinking localhost is the container and not your machine

To run docker compose use:

`docker-compose -f docker-compose.yml --exit-code-from cypress`

**Note**: `--exit-code-from cypress` tells the container to quit when cypress finishes

You can also exclude URLs from being intercepted by using the NO_PROXY setting

e.g. NO_PROXY=google.com,yahoo.co.uk

Alternatively, you can run the Cypress tests against an existing ZAP proxy by setting the environment configuration

```env
HTTP_PROXY="<zap-daemon-url>"
NO_PROXY="<list-of-urls-to-ignore>"
```

and setting the runtime variables

`zapReport=true,zapApiKey=<zap-api-key>,zapUrl="<zap-daemon-url>"`
