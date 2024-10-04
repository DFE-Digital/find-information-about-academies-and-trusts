# 14. Use SQL Server temporal tables for tracking changes to entities in FIAT database

**Date**: 2024-10-03

## Status

Accepted

## Context

We are creating a new database to manage internal DfE contact information for Trusts, replacing TRAMS as the primary data source for this data. As data provenance is a key concern for FIAT we want to track changes made to both Contacts and any other future entity data we hold with the following requirements:

- Data provenance stored must include date of change, who made the change and what the change was.
- The history of all changes to an entity must be retained.
- The history must be retained automatically for every change to data whether made through Entity Framework or direct SQL access.
- Changes to the entity properties or structure must not result in loss of history.
- Data provenance and history must be easily accessible by Find Information about Academies and Trusts so it can be displayed to the user.
- There should be minimal maintenance overhead for developers.
- It should be simple to set up the same change tracking for new entity types.
- We should be able to manage the change tracking mechanism through Entity Framework in a code first pattern.

We investigated different options and technologies including:

- Keeping a hand-rolled single audit table for the whole database
- Entity Framework [interceptors](https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors) and [events](https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/events)
- SQL Server [DML triggers](https://learn.microsoft.com/en-us/sql/relational-databases/triggers/dml-triggers?view=sql-server-ver16)
- Using a third party library such as [Audit.NET](https://github.com/thepirat000/Audit.NET/tree/master)
- SQL Server [change data capture](https://learn.microsoft.com/en-us/sql/relational-databases/track-changes/about-change-data-capture-sql-server?view=sql-server-ver16)
- SQL Server [temporal tables](https://learn.microsoft.com/en-us/sql/relational-databases/tables/temporal-tables?view=sql-server-ver16)

## Decision

We decided to use SQL Server [temporal tables](https://learn.microsoft.com/en-us/sql/relational-databases/tables/temporal-tables?view=sql-server-ver16) in conjunction with Entity Framework [interceptors](https://learn.microsoft.com/en-us/ef/core/logging-events-diagnostics/interceptors).

The [temporal tables are well supported by Entity Framework](https://learn.microsoft.com/en-us/ef/core/providers/sql-server/temporal-tables) and will automatically store changes made to any row in a history table which is linked to the main table under the hood.

As of .NET 8, Entity Framework (EF) does not allow us to retrieve the temporal table's `PeriodStart` timestamp (the time a row became the version of truth) as a standard entity property so we have added a computed column called `LastModifiedAtTime` which mirrors the `PeriodStart` column.

The EF interceptors are used to automatically retrieve the details of the user making the edit from the signed in user session and set this information in the entity before saving it to the database. For direct SQL this information needs to be manually supplied which suits our needs as a direct SQL update may be coming from a team or process rather than an individual.

## Consequences

- A history of changes to Contact records will be kept in the database
- History will be accessible in EF by including `TemporalAll()` in the query
- History will be accessible in SQL by appending `FOR SYSTEM_TIME ALL` to the `WHERE` clause
- New entities in EF will need to inherit from the `BaseEntity` class and be configured to use temporal tables
- The `FiatDbContext` cannot be mocked and tested in the same way as the `AcademiesDbContext` due to EF interceptors and SQL Server temporal tables being difficult to mock
- When making changes to the Contacts schema, `SYSTEM_VERSIONING` may need to be temporarily disabled - see [docs on changing schema of a temporal table](https://learn.microsoft.com/en-us/sql/relational-databases/tables/changing-the-schema-of-a-system-versioned-temporal-table?view=sql-server-ver16)
- When introducing a new non-nullable column or otherwise updating existing records, a decision will need to be taken about whether or not that should be reflected in the history. History can be temporarily disabled using [`SET (SYSTEM_VERSIONING = OFF)`](https://learn.microsoft.com/en-us/sql/relational-databases/tables/stopping-system-versioning-on-a-system-versioned-temporal-table?view=sql-server-ver16)
