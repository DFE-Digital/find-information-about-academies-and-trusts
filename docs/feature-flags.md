# Feature flags

Feature flags can be used to programatically turn features on and off. We can use them to control the release of features in different environments or at different times.

## Adding a flag

To create a flag you need to add it as a constant to `DfE.FindInformationAcadmiesTrusts/Configuration/FeatureFlags.cs` and add a default value to the appsettings.json file. Finally add the flag name, default value (used in appsettings.json) and reason for creating the flag/what the flag controls to the list at the end of this file.

## Using a flag

Flag can be used in 3 ways to restrict parts of the code:

- Using the tag helper in a razor page
- Using a attribute on a class or function
- Using dependency injection with the IFeatureManagement class

They can then be toggled using enviroment variables in each of the different environments, or using the appsettings.json file to override them when developing locally.

## List of flags

- `TestFlag` (Default: false): Flag added to test the functionallity of feature flags. Adds a line to the layout that shows flags are working when set to true.
