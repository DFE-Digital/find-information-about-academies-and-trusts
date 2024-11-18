# 15. Use Test Containers to unit test FIAT database configuration, usages and change tracking

**Date**: 2024-10-04

## Status

Accepted

## Context

We recently added new functionality which relys heavily on SQL Server temporal tables and Entity Framework interceptors (see [ADR 14 - Use SQL Server temporal tables for tracking changes to entities in FIAT database](0014-use-sql-server-temporal-tables.md)). This functionality is difficult to test but a core part of our application.

It cannot be tested through Cypress at an end-to-end level because the Cypress tests are run against the Dev environment and do not have access to the Dev database. It cannot be tested through mocking (like the repositories using `AcademiesDbContext`) because we would be mocking out the components doing the work - the interceptor attached to the `FiatDbContext` and the temporal tables held and managed by SQL Server. The SQL Server temporal tables and Entity Framework interceptors have up to this point been tested manually but this is time intensive and requires detailed understanding of the expected behaviour.

We are at risk of introducing bugs into the application whenever the FIAT database schema changes, especially if `SYSTEM_VERSIONING` has to be temporarily disabled during a migration. It would be very easy to accidentally forget to manually regression test the change tracking functionality of the entities and as we add more entities to the system manual testing will get increasingly expensive.

## Decision

We will introduce a new technology to enable us to run low level tests backed by a real `FiatDbContext` and a real SQL Server instance - this will be [dotnet Test Containers](https://dotnet.testcontainers.org/).

[Test Containers](https://testcontainers.com/) are a widely used tool which describes itself as:

>**Unit tests with real dependencies**
>
>Testcontainers is an open source library for providing throwaway, lightweight instances of databases, message brokers, web browsers, or just about anything that can run in a Docker container.
>
>No more need for mocks or complicated environment configurations. Define your test dependencies as code, then simply run your tests and containers will be created and then deleted.
>
>With support for many languages and testing frameworks, all you need is Docker.

We will use [dotnet Test Containers](https://dotnet.testcontainers.org/) with the [SQL Server module](https://dotnet.testcontainers.org/modules/mssql/) to test areas of database access code that would otherwise be untestable. The configuration of these tests will be easily reusable through a base `BaseFiatDbTest` class which should **only** be used when an instance of SQL Server is required.

Spinning up a SQL Server docker container is much faster with Test Containers than without but it is still an overhead. To speed up the test run as much as possible, we will reuse the same container instance across tests by using an [XUnit Collection fixture](https://xunit.net/docs/shared-context#collection-fixture) and destroy and recreate the database before each test. This also means that the affected tests will run synchronously.

## Consequences

- The new database tests will be run in the pipeline as part of the standard unit test run - no extra config is needed here because all GitHub Actions runners have Docker configured and available.
- All code currently in `DfE.FIAT.Data.FiatDb` is now testable and can now be included in code coverage metrics.
- The database tests will be longer running than other unit tests (on BYOD Mac it adds an extra 8 seconds, DfE Windows machines vary but generally take longer than that). If running IDEs in continuous testing mode on a developers local machine, a developer may want to exclude these tests from their unit testing session.
- The database tests will be excluded from Stryker mutation testing due to the length of their execution time.
- If the docker engine is not running then the database tests will fail (but other tests will run and behave as per normal).
