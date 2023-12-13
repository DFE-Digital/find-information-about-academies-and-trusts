# 7. Use MIS schema for Ofsted data

Date: 2023-12-13

## Status

Accepted

## Context

We need to show previous and current Ofsted ratings to our users. GIAS gives us some information about current ratings but nothing about previous ratings. GIAS sometimes splits the "inadequate rating" into "special measures" and "serious weaknesses" and also stores the value as a descriptive string which is less useful to us than the raw numerical value.

The only source of previous Ofsted ratings currently available to us is the MIS schema in the academies db (which reflects "State-funded school inspections and outcomes: management information" system) which contains both the information we need now and further information we may potentially require in the future.

## Decision

We've chose to use the MIS schema for both current and previous Ofsted ratings.

We've chosen to use it as our source for current Ofsted data (instead of GIAS) partially for data source consistency, partially because there is scope to extend and take other values from the tables and partially because there is less processing of the scores.

## Consequences

Find information about academies and trusts is reliant on State-funded school inspections and outcomes: management information as a data source.
