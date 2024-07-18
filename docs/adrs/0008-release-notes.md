# Architectural Decision Record (ADR)

## ADR: Adoption of Keep a Changelog Format for Release Notes

### Status

To be discussed with wider team

### Context

To introduce the clarity and consistency of release notes for our project, we decided to adopt a standardised format for documenting changes. This will help both developers and stakeholders understand the modifications, additions, and fixes in each release.

### Decision

We have chosen to use the [Keep a Changelog](https://keepachangelog.com/en/1.0.0/) format for our release notes. This format is widely recognised and provides a clear structure for documenting changes. Other products in the programme follow this structure, see [Complete](https://github.com/DFE-Digital/dfe-complete-conversions-transfers-and-changes/blob/main/CHANGELOG.md). The intention is that everytime a developer or tester merges a piece of work a new check will be "Have you added release notes?".

### Consequences

- **Pros**:
  - Provides a consistent and clear format for release notes.
  - Helps developers quickly understand the changes in each release.
  - Improves communication with stakeholders by clearly documenting changes.
  - Other products in the program follow this structure, minimising training requirements.
  - Will make a more regular release cadence digestable by stakeholders.

- **Cons**:
  - Requires discipline to maintain the format consistently.
  - Will often be written by a developer/tester and as such may lean towards the more technical.

### Implementation

A CHANGELOG.md file has been added to the repository.

1. All future release notes will follow the Keep a Changelog format.
2. Check will be added to our GitHub Pull Request template to remind of the need to update notes that are relevant to the work completed.
3. Communication with the business change team will not be replaced by these, these will act as a reference point for us to link them to, ensuring they understand what is included in a given release.
