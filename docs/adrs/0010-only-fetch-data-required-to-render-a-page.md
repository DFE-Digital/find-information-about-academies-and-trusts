# 10. Only fetch data required to render a page

Date: 2024-08-05

## Status

Accepted

## Context

The application is suffering noticeable performance issues due to the application performing db queries involving multiple joins on unindexed (and often keyless) tables to retrieve all the data about a trust, its academies, governors and DfE contacts for every trust page, whether or not it is needed.

## Decision

Re-architect the application to only retrieve the data required to render each page.

We will also move from using `Provider` classes in the data layer being used by pages to retrieve all possible information to using `Repository` classes in the data layer and `Service` classes in the UI layer to collate them. This ensures that the page models do not need to know where the data is coming from.

## Consequences

The pages will load significantly faster however new pages will need bespoke backend work to retrieve the data that they need. Each existing page also needs to have its bespoke data route created, this is being tackled one page at a time.
