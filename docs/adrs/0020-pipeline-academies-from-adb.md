# 20. Interim Data Integration for Pipeline Academies

**Date**: 2024-11-18  

## Status

Accepted  

## Context  

The FIAT project currently relies on the Academies Database (ADB) and the FIAT database for all its data requirements. As we plan to introduce the "Pipeline Academies" functionality, we need to integrate data from additional applications within the RSD portfolio, including:  

- **Prepare Conversions**  
- **Prepare Transfers**  
- **Manage Free School Projects**  
- **Complete**  

Long term, the optimal solution is to consume this data through the APIs provided by each respective application. However, the timelines for implementing this feature make it impractical to expect these projects to deliver their APIs in time.  

To meet our deadlines, we propose an interim approach: leveraging the existing synchronisation mechanisms where these applications sync their data back to the Academies Database daily for reporting purposes. This allows us to utilise a consolidated and up-to-date data source for our needs while maintaining acceptable performance and reliability.  

## Decision  

For the interim, we will integrate data from the Academies Database (ADB) to support the "Pipeline Academies" feature. This approach involves querying specific tables within ADB, which aggregate data synced from the relevant applications.  

In the medium term, we will transition to consuming this data via versioned APIs from the respective applications once they become available.  

## Reasons for the Decision  

1. **Access to Current Data**: Leveraging the ADB ensures we have access to data that is synced daily from the relevant applications, enabling us to work with up-to-date information.  
2. **Simplicity and Feasibility**: This approach avoids dependencies on external teams delivering APIs within our tight timelines, reducing implementation risk.  
3. **Performance**: Consolidating data into a single "super atomic" call from the ADB ensures our application remains performant, while allowing us to query the exact data we need.  
4. **Maintainability**: While interim integration requires managing changes to ADB schema, transitioning to APIs later will improve maintainability with version control and documentation inherent to API usage.  

## Consequences  

### Short-Term Benefits  

- **Rapid Development**: We can meet project deadlines without waiting for API availability.  
- **Centralised Data Access**: ADB provides a unified source of truth for data from multiple applications.  

### Drawbacks  

1. **Dependence on ADB Schema**: Changes in the data model or structure of contributing applications may impact our queries and require maintenance.  
2. **Limited Data Validation**: Relying on ADB means less control over the accuracy of the data, as it is aggregated and not fetched directly from source systems.  

### Medium-Term Considerations  

- Transitioning to APIs will provide improved control, validation, and resilience against schema changes.  

## Trade-offs  

### Simplified Implementation vs. Future Maintenance  

This decision simplifies our immediate implementation but introduces potential maintenance overhead as ADB schemas evolve with changes in upstream applications.  

### Interim vs. Long-Term Solutions  

Our interim reliance on ADB data ensures we can deliver the feature on time but will require effort to migrate to API-based integration later.  

## Requirements for API Integration (Medium-Term)  

While our analysis is ongoing, the following data fields are identified as necessary for the integration:  

### Prepare Conversions (Academisation API)  

| Column Name                     | Field Name                      | ADB Table Name         | Column Name                     |  
|---------------------------------|---------------------------------|-----------------------|---------------------------------|  
| Name                            | School Name                    | [academisation].[Project] | [SchoolName]                   |  
| URN                             | Unique Reference Number (URN)  | [academisation].[Project] | [urn]                          |  
| Project Type                    | Academy Type and Route         | [academisation].[Project] | [AcademyTypeAndRoute]          |  
| Age Range                       | Age Range                      | [academisation].[Project] | [AgeRange]                     |  
| Local Authority                 | Local Authority                | [academisation].[Project] | [LocalAuthority]               |  
| Proposed Conversion/Transfer Date | Proposed Conversion Date       | [academisation].[Project] | [ProposedAcademyOpeningDate]   |  

### Prepare Transfers (Academisation API)  

| Column Name                     | Field Name                      | ADB Table Name         | Column Name                     |  
|---------------------------------|---------------------------------|-----------------------|---------------------------------|  
| Name                            | School Name                    | [mis].[Establishment] | [EstablishmentName]            |  
| URN                             | Unique Reference Number (URN)  | [mis].[Establishment] | [URN]                          |  
| Project Type                    | Academy Type and Route         | [academisation].[TransferProject] | [TypeOfTransfer]            |  
| Age Range                       | Age Range                      | [mis].[Establishment] | [StatutoryLowAge], [StatutoryHighAge] |  
| Local Authority                 | Local Authority                | [academisation].[TransferringAcademy] | [LocalAuthority]           |  
| Proposed Transfer Date          | Proposed Transfer Date         | [academisation].[TransferProject] | [TargetDateForTransfer]    |  

### Complete (Complete API)  

#### Conversion Endpoint  

| Column Name                     | Field Name                      | ADB Table Name         | Column Name                     |  
|---------------------------------|---------------------------------|-----------------------|---------------------------------|  
| Name                            | Name                           | [mis].[Establishment] | [EstablishmentName]            |  
| URN                             | Academy URN                    | [complete].[projects] | [academy_urn]                  |  
| Project Type                    | Type                           | [complete].[projects] | [type]                         |  
| Age Range                       | Age Range                      | [mis].[Establishment] | [StatutoryLowAge], [StatutoryHighAge] |  
| Conversion Date                 | Conversion Date                | [complete].[projects] | [completed_at]                 |  
| Local Authority                 | Local Authority                | [complete].[projects] | [local_authority]              |  

### Manage Free School Projects (MFSP API)  

| Column Name                   | Field Name                          | ADB Table Name   | Column Name                                                      |
|-------------------------------|--------------------------------------|------------------|------------------------------------------------------------------|
| Name                          | Current free school name            | [mfsp].[KPI]     | [Project Status.Current free school name]                       |
| Project Type                  | Project type                        | [mfsp].[KPI]     | [Project Status.Free school application wave]                   |
| Age Range                     | Age range                           | [mfsp].[KPI]     | [School Details.Age range]                                      |
| Local Authority               | Local authority                     | [mfsp].[KPI]     | [School Details.Local authority]                                |
| URN                           | URN                                 | [mfsp].[KPI]     | [Project Status.URN (when given one)]                           |
| Provisional Opening Date      | Provisional opening date            | [mfsp].[KPI]     | [Project Status.Provisional opening date agreed with trust]     |

## Future Considerations  

1. **Transition to APIs**: Once APIs are implemented by the respective applications, we will integrate them for better data accuracy, resilience to schema changes, and improved maintainability.  
2. **Versioning and Stability**: API-based integration allows version control, reducing disruptions caused by changes in data models.  
3. **Automated Validation**: Implement automated checks to ensure data integrity during the interim period.  
