# Archive branches

## API implementation and mocking

### What is the branch?

The branch `archive/api-mock-implementation-with-wiremock` is an archive of main at the point when FIAT was using the Academies API (before moving to directly connect to the Academies db).

### Why did we archive this branch?

We removed the connection to the Academies API to temporarily directly connect to the Academies database but as we intend to reconnect FIAT to other APIs in the future we didn't want to lose all the good work we put into mocking APIs for the UI tests.

### What is interesting about this branch?

- [Docker compose for ci](
https://github.com/DFE-Digital/find-information-about-academies-and-trusts/blob/archive/api-mock-implementation-with-wiremock/docker/docker-compose.ci.yml) - This stands up the wiremock container and enables the application under test to communicate with it
- [wiremock-captain in package.json](https://github.com/DFE-Digital/find-information-about-academies-and-trusts/blob/archive/api-mock-implementation-with-wiremock/tests/playwright/package.json) - used to allow the wiremock container to be configured via TypeScript
- [Mock setup coordinator](https://github.com/DFE-Digital/find-information-about-academies-and-trusts/blob/archive/api-mock-implementation-with-wiremock/tests/playwright/mocks.setup.ts) and [actual mock setup](https://github.com/DFE-Digital/find-information-about-academies-and-trusts/blob/archive/api-mock-implementation-with-wiremock/tests/playwright/mocks/mock-trusts-provider.ts) code in Playwright
- [Code docs](https://github.com/DFE-Digital/find-information-about-academies-and-trusts/blob/archive/api-mock-implementation-with-wiremock/docs/getting-started.md#accessibility-and-ui-tests) - describes how to get the UI and a11y tests working
