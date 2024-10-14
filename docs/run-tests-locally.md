# Run tests locally

Use this documentation to run tests locally.

- [Unit tests](#unit-tests)
- [UI tests](#ui-tests)

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

2. Create a file in tests/DFE.FindInformationAcademiesTrusts.CypressTests called cypress.env.json which contains the following:

    ```bash
    {
    "URL": "<url of the application under test>",
    "AUTH_KEY": "<auth bypass secret for application>"
    }
    ```

3. Open a terminal in your repository and run:

    ```bash
    cd tests/DFE.FindInformationAcademiesTrusts.CypressTests

    # run tests 
    npx cypress open --> #This will run Cypress for the first time and notify you of such, it should then setup Cypress locally for you. 
    ```

4. Once the above step is ran you should have the Cypress UI open showing the e3e test option which we should now click as well as what browser we want to test in. You should now see your test specs - to run these you click on the test spec and it should then run all the tests within said spec.

    - If the test passes you should see a green tick next to said test with confirmation that all tests within the spec are passing if multiple.
    - If the test fails it should show you within the runner what the failed test step is with a screenshot of where it failed to help you in debugging the fail. (N.b Cypress will rerun the open spec everytime you save one of your open files)
