# Update test data

Accessibility and UI tests are written using Playwright, with external dependencies (e.g. APIs) mocked using a fake database.

We use a console application to generate test data, which is added to a fake version of the Academies database before [running the tests](run-tests-locally.md#accessibility-and-ui-tests). Playwright tests are run against a saved version of this fake data.

Random data is generated using a seed, so will not change unless changes are made to the code.

When developing you may need to update and replace this test data if:

- A change is made to models used in the `DfE.FindInformationAcademiesTrusts.Data`, or to the `DfE.FindInformationAcademiesTrusts.Data.AcademiesDb` project.
- A new data source is added, or we need to generate new test data using the Faker

## When a change is made to production code

If you have made a change to a data model, or to the `AcademiesDb` project, then the data will need updating in our playwright tests directory:

1. Run the Faker project in your IDE
2. Navigate to the project's run location  (e.g. `bin/Debug/net7.0`) and copy all json files in the new `data/` folder into `~/tests/playwright/fake-data`
3. Commit the new fake data in the playwright directory
4. If you are already running your tests locally you will need to stop and start the docker container to recreate the test database: `npm run docker:restart`
5. Check that the data matches by [re-running the UI tests](run-tests-locally.md#accessibility-and-ui-tests) : `npm run test:ui`

## Updating the Faker with new data

1. Open the Faker project: `tests/DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker`
2. Add or update the relevant Faker (matching the model you have updated)
3. Add any new Fakers to `AcademiesDbFaker` and the `AcademiesDbData` classes
4. If adding a new Faker, add your new dataset to the `SqlGenerator.cs` > `GenerateSqlInsertScript` method
5. We recommend testing the generated scripts locally before committing them:
   - run the Faker project
   - open the `data/` directory in the project's run location
   - run the `insertScript.sql` and then `createScript.sql` on a sql server instance
   - check the tables contain the generated data
6. Update the data in the `tests/playwright/fake-data` directory using [the steps above](#when-a-change-is-made-to-production-code)
