# Run tests locally

Use this documentation to run tests locally.

- [Unit tests](#unit-tests)
- [UI tests](#ui-tests)
  - [Installation](#installation)
  - [Running UI tests](#running-ui-tests)
    - [To run against a deployed environment](#to-run-against-a-deployed-environment)
    - [To run against your local branch](#to-run-against-your-local-branch)

## Unit tests

All .NET code is unit tested where possible (as per the [test approach][test-approach]).

You can run unit tests using your preferred IDE, or open a terminal in the project directory and run:

```bash
dotnet test
```

> [!NOTE]
> The FIAT db tests will fail unless the Docker engine is running - see [ADR 15. Use Test Containers to unit test FIAT database configuration, usages and change tracking][adr-15] on why and how this works.

## UI tests

### Installation

1. Open a terminal in your repository and run:

    ```bash
    # go to the cypress tests folder
    cd tests/DFE.FindInformationAcademiesTrusts.CypressTests

    # install dependencies
    npm install    
    
    # set up Cypress
    npx cypress open    #This will run Cypress for the first time and notify you of such, it should then setup Cypress locally for you.
    ```

2. Create a file in tests/DFE.FindInformationAcademiesTrusts.CypressTests called `cypress.env.json` which contains the following:

    ```json
    {
      "URL": "<url of the application under test>",
      "AUTH_KEY": "<auth bypass secret for application>"
    }
    ```

### Running UI tests

#### To run against a deployed environment

1. Make sure your `cypress.env.json` file contains the correct information for the environment you want to use.

2. Open a terminal in your repository and run:

    ```bash
    # go to the cypress tests folder
    cd tests/DFE.FindInformationAcademiesTrusts.CypressTests

    # run tests 
    npx cypress open
    ```

3. You should now see the Cypress UI open

    - click the e2e test option
    - click the browser you want to test in

4. You should now see your test specs - to run these you click on the test spec and it should then run all the tests within said spec.

    - If the test passes you should see a green tick next to said test with confirmation that all tests within the spec are passing if multiple.
    - If the test fails it should show you within the runner what the failed test step is with a screenshot of where it failed to help you in debugging the fail. (N.b Cypress will rerun the open spec everytime you save one of your open files)

#### To run against your local branch

1. [Start the application in Docker][docker-run]
2. Set the `TestOverride__CypressTestSecret` value in `docker/.env` to any value that you like
3. Update your `cypress.env.json` to:

    ```json
    {
      "URL": "localhost",
      "AUTH_KEY": "<the TestOverride__CypressTestSecret value you set in step 2>"
    }
    ```

4. Open a terminal in your repository and run:

    ```bash
    # go to the cypress tests folder
    cd tests/DFE.FindInformationAcademiesTrusts.CypressTests

    # run tests 
    npx cypress open
    ```

[adr-15]: adrs/0015-use-test-containers-to-unit-test-fiat-database.md
[docker-run]: ./docker.md#how
[test-approach]: ./test-approach.md
