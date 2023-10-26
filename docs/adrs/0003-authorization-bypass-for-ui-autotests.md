# 3. Authorization bypass for UI auto tests

Date: 2023-10-26

## Status

Accepted

## Context

Up until now, we have been unable to run ui automation tests on dev or test enviroments.

Due to DfE's policy of only allowing certain IP ranges access to authenticate via microsoft, test users will not work in the test pipelines in the same way as a normal user so we needed to implement a special route to enable the test user to bypass authentication in non-prod environments.

## Decision

We will implement the dotnet class DenyAnonymousAuthorizationRequirement which allow bypassing of authorization functionality.

It requires a secret guid to be stored in azure which is passed in an authorization header with every test automation request.

The bypass is only allowed on non production environments.

This method as been used in other Dfe projects within the program.

## Consequences

Test reporting has to be stopped in the dev and test environments during ci pipeline testing to prevent the test secret being exposed.

If the test secret is ever exposed during test reporting it needs to be changed in azure.
