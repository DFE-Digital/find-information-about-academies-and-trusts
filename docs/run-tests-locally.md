# Run tests locally

Use this documentation to run tests locally.

- [Unit tests](#unit-tests)
- [Accessibility and UI tests](#accessibility-and-ui-tests)
- [Integration and Deployment tests](#integration-and-deployment-tests)
- [Owasp Zap security tests](#integration-and-deployment-tests)

## Unit tests

All .NET code is [unit tested](./test-approach.md) where possible.

You can run unit tests using your preferred IDE, or open a terminal in the project directory and run:

```bash
dotnet test
```

## Accessibility and UI tests

Accessibility and UI tests are written using [Playwright](https://playwright.dev/), with external dependencies mocked (e.g. the database) or bypassed (e.g. the auth). You can run the tests against a docker instance, or run the application locally.

1. Ensure the Docker engine is running.
1. Create or update the file `tests/playwright/.env`

    - **If running tests against the docker instance of the app:**

        ```dotenv
        PLAYWRIGHT_BASEURL="http://localhost/"
        AUTH_BYPASS_SECRET="TestSuperSecret"
        ```

    - **If running tests in your local environment**

        ```dotenv
        PLAYWRIGHT_BASEURL="http://localhost:{port} # e.g. http://localhost:5163
        AUTH_BYPASS_SECRET="TestSuperSecret"
        ```

        For your local environment you will also need to update your dotnet user secrets with the following:

        ```bash
        # add testdb connection string
        dotnet user-secrets set "ConnectionStrings:AcademiesDb" "Server=localhost;User Id=sa;Password=mySuperStrong_pa55word!!!;TrustServerCertificate=true"

        # add authorization header for tests - which matches the auth bypass secret set in the playwright .env file
        dotnet user-secrets set "TestOverride:PlaywrightTestSecret" "TestSuperSecret"
        ```

        Make sure you start the application before running the tests!

1. Open a terminal in your repository and run:

    ```bash
    cd tests/playwright

    # install dependencies
    npm install
    npx playwright install

    # run docker image with an application rebuild
    # you will need to run this even if you are running tests locally - to create the fake database for tests
    npm run docker:start 

    # run tests 
    npm run test:ci

    # OR
    npm run test:ui # run only ui tests
    npm run test:ui:trace # run only ui tests in chromium, with trace mode
    npm run test:a11y # run only accessibility tests

    npx playwright test {folder-name}/* # run a particular set of tests
    npx playright test --headed # run in headed mode
    npx playwright test --trace=on # get a time machine attached to each test result in the report

    # remove docker image when done
    npm run docker:stop
    ```

    > **Warning**
    > If you have trouble getting the docker compose files to work, see [known issues with Docker](./docker-issues.md).

For more information on running and debugging Playwright tests it is worth familiarising yourself with the Playwright docs on [debugging](https://playwright.dev/docs/debug) and [command line flags](https://playwright.dev/docs/test-cli).

If you need to update the mocked test data for UI and Accessibility tests see [update test data](./update-test-data.md)

## Integration and Deployment tests

[Integration and Deployment tests](./test-approach.md) are also run in [Playwright](https://playwright.dev/), but do not have dependencies mocked. In the pipeline these will be run against deployed environments, but if you wish to run these locally you will need to run the app locally, as you will not be able to run these tests against the docker containers.

1. Update the dotnet user-secrets for the application to bypass authentication:

    ```bash
    cd DfE.FindInformationAcademiesTrusts
    dotnet user-secrets set "TestOverride:PlaywrightTestSecret" "TestSuperSecret"
    ```

1. Run the .NET application. See [getting started](getting-started.md) for info on how to do this
1. Configure/create `tests/playwright/.env` and add the following:

    ```dotenv
    PLAYWRIGHT_BASEURL="http://localhost:5163" # This should be the localhost port the application is running on, or the deployed application url you wish to test against
    AUTH_BYPASS_SECRET="TestSuperSecret"
    ```

1. Open a terminal to run your tests

    ```shell
    cd DfE.FindInformationAcademiesTrusts.UnitTests/playwright
    npm install
    npx playwright install
    npm run test:integration
    npm run test:deployment
    ```

## Owasp Zap security tests

To run the owasp zap security scanner locally on the test or dev environments.

1. Run the owasp zap api in docker, please note the post scan test report will be sent to wherever you run this command from. Always stop and restart the owasp zap api in docker before every test run to ensure you get a clean test report.

    ```bash
    docker run --rm -v ${PWD}:/zap/wrk/:rw -u zap -p 8083:8083 -i owasp/zap2docker-stable zap.sh -daemon -host 0.0.0.0 -port 8083 -config api.disablekey=true -config api.addrs.addr.name=.* -config api.addrs.addr.regex=true
    ```

1. Add these environment variables to the .env file in the playwright folder.

    ```bash
    cd tests/playwright/.env

    # add these environmental variable to .env file

    PLAYWRIGHT_BASEURL="https://dev.find-information-academies-trusts.education.gov.uk/"
    TEST_USER_ACCOUNT_NAME="<test user>"
    TEST_USER_ACCOUNT_PASSWORD="<test users password>"
    HTTP_PROXY="http://localhost:8083/"
    ZAP=true
    ZAP_PORT=8083
    ```

1. Run the tests

    ```bash
    cd tests/playwright

    npx playwright test --project=zap-tests --trace=on
    ```
