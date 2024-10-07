# 16. Run FIAT database migrations on Docker container start

**Date**: 2024-10-04

## Status

Accepted

## Context

We needed to develop and release the Internal DfE Contacts feature quickly to ensure that this essential feature was available to users upon Private Beta release. As part of this work we created a new FIAT database with a schema controlled by Entity Framework migrations and needed a way to apply those migrations to databases in different environments.

Current [Entity Framework guidance](https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=dotnet-core-cli) states that:

> The recommended way to deploy migrations to a production database is by generating SQL scripts.
>
>...
>
> SQL scripts can be reviewed for accuracy; this is important since applying schema changes to production databases is a potentially dangerous operation that could involve data loss.

It also warns against running migrations at runtime for production environments, noting that
:
>If multiple instances of your application are running, both applications could attempt to apply the migration concurrently and fail (or worse, cause data corruption).
Similarly, if an application is accessing the database while another application migrates it, this can cause severe issues.

As we were on a tight time constraint we wanted to ensure whatever methodology we picked for running migrations would work with minimal effort on our estate so we looked to other applications in the department. We found that each application in department currently builds and runs migrations slightly differently but every application ultimately has those migrations controlled by Docker. Some applications [build a SQL script](https://github.com/DFE-Digital/academies-academisation-api/blob/711adef80838f186e9232057262ebe6877c5a478/Dockerfile) and [run it on start](https://github.com/DFE-Digital/academies-academisation-api/blob/711adef80838f186e9232057262ebe6877c5a478/script/webapi-docker-entrypoint.sh#L18), some [run a migration tool directly on start](https://github.com/DFE-Digital/dfe-complete-conversions-transfers-and-changes/blob/0a0271359cce47abdb99fa01321ca14adc94df31/docker-entrypoint.sh#L10).

Very recently a new technique of [running a single stage of the docker image as a separate container](https://github.com/DFE-Digital/record-concerns-support-trusts/blob/f18a7d04ff1f7f4953f843309aa8e46128e1a97d/.github/workflows/build-and-push-image.yml#L84) in the pipeline has been trialled which seperates the migration from the main application start. This removes the danger of running multiple migrations simulataneously at runtime but it was new enough and complex enough that we missed it during our intial investigation. There are also some parts of this approach that we would like to change but we did not have the time to experiment.

## Decision

We wanted the benefit of being able to review and source control the SQL script so we moved this to be a manual step run at the same time as creating a new migration.

We copied the most prevalent methodology in the department of running EF migrations on startup of the production app Docker container because we knew that this method would work on our infrastructure and that it was the most longstanding way of doing things in the department.

This is a stop-gap decision and has been recorded as technical debt that needs addressing. We're planning to find a better way of running migrations and to share that with other teams.

## Consequences

- Creating the migration SQL script is a manual step ([documented here](..\databases.md#migrations-for-fiat-db)).
- Migrations are run on Docker container start, this could potentially cause unexpected behaviour if scaling causes multiple Docker containers to launch at the same time on the same environment.
- The dockerfile may not build on some DfE devices without additional proxy setup due to the installation of SQL tools.
- We will soon be revisiting how we apply and rollback migrations in the pipelines.
