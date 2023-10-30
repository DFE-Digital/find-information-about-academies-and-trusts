# 3. Authorization bypass for UI auto tests

Date: 2023-10-26

## Status

Accepted

## Context

Up until now, we have been unable to run ui automation tests on dev or test enviroments.

Due to DfE's policy of only allowing certain IP ranges access to authenticate via microsoft, test users will not work in the test pipelines in the same way as a normal user so we needed to implement a special route to enable the test user to bypass authentication in non-prod environments.

## Decision

We will implement the dotnet class `AuthorizationHandler<DenyAnonymousAuthorizationRequirement>` which allows bypassing of our standard authorization route.

It requires a secret to be stored in azure which is passed in an authorization header with every test automation request.

The bypass is only allowed on non production environments.

This method has been used in other DfE projects within the program.

## Consequences

Developers will no longer be able to access a locally running CI docker instance through their browser because the header will be missing from their browser requests

Integration and Deployment tests can be reenabled in the pipeline. The security test pipeline will also now function on demand. However uploading Playwright reports directly to the GitHub Action run has to be stopped to prevent the test secret being exposed in the traces. If the test secret is ever exposed during test reporting it needs to be changed in azure.
