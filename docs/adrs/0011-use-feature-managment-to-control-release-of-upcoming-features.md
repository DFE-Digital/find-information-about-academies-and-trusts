# 11. Use feature management to control the release of upcoming features

Date: 2024-08-07

## Status

Accepted

## Context

Development of future features may have to be started before supporting work is completed. We need a system that allows us to create new features without having them released to users.

## Decision

Using the ASP.Net Feature managment nuget package we will implement feature flags. This will allow us to place upcoming features/pages behind feature gates meaning that development can be completed on future features and the release of these can be controlled using environment variables and using the app settings files. A list of feature flags will be kept in documentation so we can track which flags are available and what features they restrict. For each User story or feature that involves adding feature flag, a work task will be created to remove the flag when it is no longer needed. Flags should be removed after a appropriate amount of time after the feature is released.

## Consequences

Features can now be restricted to certain versions or environments. Feature flags available will need to be kept track of using a constants file in the code and additional documentation.
