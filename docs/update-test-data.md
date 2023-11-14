# Update test data

Accessibility and UI tests are written using Playwright, with external dependencies (e.g. APIs) mocked using a fake database.

We use a console application to generate test data, which is added to a fake version of the Academies database before [running the tests](run-tests-locally.md#accessibility-and-ui-tests).

When developing you may need to update and replace this test data—particularly if you are making changes to the `DfE.FindInformationAcademiesTrusts.Data.AcademiesDb` project.

1. Open the Faker project: `tests/DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker`
2. Add or update the relevant Faker (matching the model you have updated) and add any new Fakers to `Program.cs`
3. Navigate to the project's run location  (e.g. `bin/Debug/net7.0`) and copy all json files in the new `data/` folder into `~/tests/playwright/fake-data`
4. Commit both the update to the Faker project and the new fake data in the playwright directory
5. If you are already running your tests locally you will need to stop and start the docker container to recreate the test database:

```bash
cd tests/playwright
npm run docker:stop
npm run docker:start
```
