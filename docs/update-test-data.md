# Update test data

Accessibility and UI tests are written using Cypress, with external dependencies (e.g. APIs) mocked using a fake database.

We use a console application to generate test data, which is added to a fake version of the Academies database before [running the tests](run-tests-locally.md#accessibility-and-ui-tests). Cypress tests are run against a saved version of this fake data.

When developing you may need to update and replace this data as more features are implemented

## How to update test data

- Make changes to the faker for the table you want to update in `tests/DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker/Fakers`
- Run `DfE.FindInformationAcademiesTrusts.Data.AcademiesDb.Faker` locally to regenerate the sql scripts
- Commit the updated data

## How this data is used to build a database

The docker compose files `docker-compose.ci.yml` and `docker-compose-db.yml` both create a database using the fake data

Both compose files do the following:

- Create a container with SQL running
- Another container will run the create and insert scripts against the running SQL instance
- You will then have a running SQL instance with a sip database that has the fake data loaded
- From this point you can either run the fiat application or run the automated tests against it

`docker-compose.ci.yml` will also run the fiat application in another container
