# 4. Connect to Academies DB directly

Date: 2023-11-01

## Status

Accepted

## Context

Until now we have been retrieving information about academies and trusts from the shared Academies API which is the standard way that products across RSD get information. Unfortunately the Academies API only provides a small proportion of the data that FIAT needs access to and it is unclear where some of the data originally comes from. Data provenance is a fundamental concern to FIAT as we need to be able to confidentally and truthfully display the source of the data to our users so they know that they don't need to cross reference it with other trusted systems.

The Academies API is a difficult thing to change, without a clear owner and a lot of different people and teams depend upon it. Its future is also uncertain - it might be retired or rewritten in the near future.

## Decision

Rather than expend a lot of effort and risk in adding the precise data we need to the Academies API, we will connect directly to the Academies DB to retrieve the information that we need for FIAT until better data sources that meet our needs are available. We will also make our system easy to change to using other data providers.

## Consequences

This approach gives us much greater flexibility in what data we use and how we use it. It also means that we don't have to go through a change process for every user story and that we can't break things for other systems.
