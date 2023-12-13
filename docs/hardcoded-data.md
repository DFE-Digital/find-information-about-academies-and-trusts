# Hardcoded data

## Free school meals averages

### What is the data?

The data is the percent of pupils eligible for free school meals (2022/23), by region and establishment type.
It is taken from the Permanent data table
['Pupil characteristics - number of pupils by fsm eligibility'](https://explore-education-statistics.service.gov.uk/data-tables/permalink/25bc8d0b-c700-4000-1b8a-08dbb99e3fd8) from 'Schools, pupils and their characteristics' on the Explore education statistics service.

### Why is it hardcoded

See [ADR 6 - Use hardcoded free school meals statistics data](adrs/0006-use-hardcoded-free-school-meals-statistics-data.md)

### What needs updating in the future

The data should either:

- be removed, and accessed via an API request
- be replaced with the latest annual figures when they are available

### How to update it

If updating the hardcoded data with new figures, you will need to:

- Replace the contents of the method: `AddPercentagesByPhaseType()` on the static data class `FreeSchoolMealsData`
- Update references to the year 2022/23 in the UI and in the code
- **Note:** we have only added the four establishment types on the enum `ExploreEducationStatisticsPhaseType` as the other establishment types in the table are not currently relevant to our data
- **Note:** the list of local authorities added in `PopulateLocalAuthorities` is from the same data table and could also become out of date
