# 6. Use hardcoded data for free school meals averages

Date: 2023-12-12

## Status

Accepted

## Context

For our MVP we have a user need to display the local and national percentages of students eligible for free school meals, alongside the figure for those eligible in a particular academy - so that the user can assess how well a trust is performing.

The Academies database does not currently contain any accurate data on these figures, and that which is displayed to users in TRAMS is not correct.

The figures are currently available via a csv download hosted on [Explore Education Statistics](https://explore-education-statistics.service.gov.uk/data-tables/permalink/25bc8d0b-c700-4000-1b8a-08dbb99e3fd8). However, there may be an api in development to make this data easier to access in the future.

## Decision

Rather than expend effort on creating a new table in the academies database—which is undergoing change—or creating a new application database for a data source which should be accessible via an open API in the not too distant future, we will hardcode the data into our application.

In order to make it easy to change to a new data source in the future we have added the data in a C# static class, accessed via a provider layer.

We will make it clear to the user that it is data that will not change, using the `Data source` component displayed at the bottom of each page. We will also display the years the data covers in the column headings, to make it clear to the user if it is out of date.

## Consequences

This approach gives us quick access to a small additional data source we need for MVP. However, adding data in this way will require manual updating. The hardcoded data should eventually be replaced with a database table, or direct access to the statistics data via API if one is developed in the future.

This data is currently available publicly, so there is no risk in exposing senstive information by hardcoding it into our application.
