# 21. Add new table for School Contacts and leave Contacts unchanged

Date: 2025-06-06

## Status

Accepted

## Context

We need to add a new page to the UI to show the DfE contacts for a school.

There is an existing concept of DfE contacts for a trust and these are stored in the FIAT database in a temporal table called `Contacts` (see ADR [14 - Use SQL Server temporal tables for tracking changes to entities in FIAT database](docs\adrs\0014-use-sql-server-temporal-tables.md)). Since the FIAT `Contacts` table was introduced, a central team has created a pipeline to replicate its data in a mirrored table in Academies DB for other teams and reporting purposes. It's unclear which other teams and individuals use this downstream table (and thus what the impact of changing that table would be) but it seems that changing anything about the FIAT db `Contacts` table (including the name of the table) will not be straightforward for the central team to reflect in their pipeline.

The current `Contacts` table is indexed on `uid` which is a GIAS group specific identifier (trusts are a type of GIAS group). Schools do not have a uid, instead FIAT uses the `urn` to identify and retrieve information about a school which means that we cannot use the existing `Contacts` table as it is to store information about DfE contacts for schools. Other than this, the only difference between a school contact and a trust contact is the types of role a contact may have, which is currently stored in the database as a simple string.

## Decision

- We will not change the existing `Contacts` table in the FIAT database
- We will refer to the `Contacts` table in Entity Framework as `TrustContacts`
- We will create a new table for the school contacts which will follow the same patterns as the existing `Contacts` table
- The new table `SchoolContacts` will store contacts for a school and academy using the `urn` to lookup the contacts.

## Consequences

- This will enable us to store school contacts in a seperate table without altering the `TrustContacts` table.
- The `Contacts` table will be purly for contacts for a trust. As the name wont directly be called `TrustContacts` there is a risk this could be confusing.
- As the table name for `Contacts` will stay the same no other work by other teams will be required.