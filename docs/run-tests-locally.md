# Run tests locally

Use this documentation to run tests locally.

- [Run tests locally](#run-tests-locally)
- [Unit tests](#unit-tests)
- [Accessibility and UI tests](#accessibility-and-ui-tests)
- [Integration and Deployment tests](#integration-and-deployment-tests)
- [Owasp Zap security tests](#integration-and-deployment-tests)

### Unit tests

All .NET code is [unit tested](./test-approach.md) where possible.

You can run unit tests using your preferred IDE, or open a terminal in the project directory and run:

```bash
dotnet test
```

### Accessibility and UI tests

Accessibility and UI tests are written using [Playwright](https://playwright.dev/), with external dependencies (e.g. APIs) mocked using a [fake database](#run-the-fake-database-for-local-development). You can run the tests against a docker instance, or run the application locally.

1. Create or update the file `tests/playwright/.env`

**If running tests against the docker instance of the app:**
```dotenv
PLAYWRIGHT_BASEURL="http://localhost/"
```

**If running tests in your local environment**
```dotenv
PLAYWRIGHT_BASEURL="http://localhost:{port} # e.g. http://localhost:5163
```

For your local environment you will also need to change your database connection string (dotnet user secrets) to point to the docker instance:

```
dotnet user-secrets set "ConnectionStrings:AcademiesDb" "Server=localhost;User Id=sa;Password=mySuperStrong_pa55word!!!;TrustServerCertificate=true"
```

2. Open a terminal in your repository and run:

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

If you are running on Apple M1 chip the SQL Server image may not work in Docker, see [Get it working with Docker](#get-it-working-with-docker) for how to fix this.

For more information on running and debugging Playwright tests it is worth familiarising yourself with the Playwright docs on [debugging](https://playwright.dev/docs/debug) and [command line flags](https://playwright.dev/docs/test-cli).

### Integration and Deployment tests

[Integration and Deployment tests](./test-approach.md) are also run in [Playwright](https://playwright.dev/), but do not have dependencies mocked. If you wish to run these locally you will need to run the app locally before running your tests. Alternatively you can run them against deployed dev or test environments.

1. Run the .NET application using your preferred IDE or by running `dotnet run`
2. Configure/create `tests/playwright/.env` and add the following:

```dotenv
PLAYWRIGHT_BASEURL="http://localhost:5163" # This should be the localhost port the application is running on, or the deployed application url you wish to test against
TEST_USER_ACCOUNT_NAME="<insert here>" # Include the domain name
TEST_USER_ACCOUNT_PASSWORD="<insert here>" # 
```

3. Open a terminal to run your tests

```shell
 cd DfE.FindInformationAcademiesTrusts.UnitTests/playwright
 npm install
 npm run test:integration
```

### Owasp Zap security tests

To run the owasp zap security scanner locally on the test or dev environments.

1. Run the owasp zap api in docker, please note the post scan test report will be sent to wherever you run this command from. Always stop and restart the owasp zap api in docker before evey test run to ensure you get a clean test report. 

```bash
docker run --rm -v ${PWD}:/zap/wrk/:rw -u zap -p 8083:8083 -i owasp/zap2docker-stable zap.sh -daemon -host 0.0.0.0 -port 8083 -config api.disablekey=true -config api.addrs.addr.name=.* -config api.addrs.addr.regex=true
```

2. Add these environment variables to the .env file in the playwright folder.

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
3. run the tests

```bash
cd tests/playwright

npx playwright test --project=zap-tests --trace=on
```