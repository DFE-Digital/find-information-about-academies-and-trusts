# Run tests locally

Use this documentation to run tests locally.

- [Unit tests](#unit-tests)
- [UI tests](#ui-tests)
- [Security testing with zap](#security-testing-with-zap)

## Unit tests

All .NET code is [unit tested](./test-approach.md) where possible.

You can run unit tests using your preferred IDE, or open a terminal in the project directory and run:

```bash
dotnet test
```

## UI tests

### Installation
1. Open a terminal in your repository and run:

    ```bash
    cd tests/DFE.FindInformationAcademiesTrusts.CypressTests

    # install dependencies
    npm install
    ```

2. DFE machines may have trouble running Cypress for the first time or installing dependancies due to internal proxies and auth. If in the instance this looks like its occuring on setup you will need to run the following command to bypass this for the installation.

    ```
    set NODE_TLS_REJECT_UNAUTHORIZED=0'
    ```

3. Open a terminal in your repository and run:

    ```bash
    cd tests/DFE.FindInformationAcademiesTrusts.CypressTests

    # run tests 
    npx cypress open --> #This will run Cypress for the first time and notify you of such, it should then setup Cypress locally for you. 
    ```

4. Once the above step is ran you should have the Cypress UI open showing your test specs - to run these you click on the test spec and it should then run all the tests within said spec.

>- If the test passes you should see a green tick next to said test with confirmation that all tests within the spec are passing if multiple.
>- If the test fails it should show you within the runner what the failed test step is with a screenshot of where it failed to help you in debugging the fail. (N.b Cypress will rerun the open spec everytime you save one of your open files)



### Security testing with ZAP - Currently deprecated and will be reintroduced down the line
