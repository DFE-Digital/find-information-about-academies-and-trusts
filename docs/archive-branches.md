# Archive branches

## Add no access page

**Date**: 2025-01-02

**User stories**:

- [User Story 135671: Build: Authentication - Add access-denied screen][user-story-135671]
- [User Story 186345: Build: Security regression test and release access denied user flow][user-story-186345]
- [User Story 187074: Tech debt: Auth override for Cypress tests needs rewriting][user-story-187074]

### Summary

The "Add no access page" work was a rewriting of the authentication and authorisation of the application in order to add a friendly page to redirect unauthorised users to. This is described in [archived ADR Handle users authorisation state with roles][adr-add-no-access-page] and [User Story 135671: Build: Authentication - Add access-denied screen][user-story-135671]

| Branch                                                                                                                  | Purpose                                            | Branch history                             | Condition                                                                                                                                                                |
| ----------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------- | ------------------------------------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------ |
| [`archive/add-no-access-page`][branch-add-no-access-page]                                                               | Feature branch                                     | All code reviewed                          | In working order but contains critical bug which allows the general public access to unprotected parts of the application                                                |
| [`archive/add-no-access-page-add-ui-tests`][branch-add-no-access-page-add-ui-tests]                                     | UI test branch                                     | Branched from feature with unreviewed code | Contains first pass at rewriting test authentication bypass handler (which doesn't work and should be disregarded) and the start of some UI tests for the no-access page |
| [`archive/add-no-access-page-add-security-integration-tests`][branch-add-no-access-page-add-security-integration-tests] | `WebApplicationFactory` based security test branch | Branched from feature with unreviewed code | Contains comprehensive security tests which work locally but not in CI. These .NET based security tests are useful outside of the no-access page work                    |

All branches were cleaned and rebased onto main at the end of December 2024 to bring in core changes to `Program.cs` and the split of UI pages to sub pages.

### Why did we archive these branches?

This work ended up taking much longer than expected and it was uncertain how much longer would be required. The security implications of getting it wrong, the complexity of the problem space and the lack of a standard convention to follow all contributed to the cost. As this was low priority and low impact work it was shelved until more pressing work is completed.

### What is left to do?

#### Independent to no access page work

1. We should tackle [User Story 187074: Tech debt: Auth override for Cypress tests needs rewriting][user-story-187074] first as it is independent to this work but is required for the UI tests to work after the work is complete.
    - It is potentially complex but will also have benefits outside of the no access work.
    - The results of this work may be re-utilised in any work derived from [`archive/add-no-access-page-add-security-integration-tests`][branch-add-no-access-page-add-security-integration-tests].
2. We could split out the new security tests from [`archive/add-no-access-page-add-security-integration-tests`][branch-add-no-access-page-add-security-integration-tests] as the setup is not fundamentally tied to the no access page work.
    - This would give us automated confidence in the security setup of our .NET application.
    - If tackling separately to the no access page work then the test cases will need to be altered to reflect the current auth behaviour.
    - The last piece of this puzzle is getting the tests to run in CI (which will likely involve removing external service dependencies via the `ApplicationWithMockedServices`).

#### To complete the no access page work

1. We should fix the [Task 187010: Bug: Unauthenticated external user can access accessibility statement + other footer pages][task-186345]
    - This requires rethinking how we approach authentication vs authorisation and perhaps using policy-based access rather than anonymous access so that all users must be authenticated to access any part of FIAT.
    - The rethink also means changing documentation like the [archived ADR Handle users authorisation state with roles][adr-add-no-access-page] to be explicit that unauthenticated users (i.e. external users) should have no access whatsoever to any part of Find information about academies and trusts.
    - This approach change will impact the tests written so far on [`archive/add-no-access-page-add-ui-tests`][branch-add-no-access-page-add-ui-tests] and [`archive/add-no-access-page-add-security-integration-tests`][branch-add-no-access-page-add-security-integration-tests].
2. We should complete the UI tests
    - This should be simple (given that [User Story 187074: Tech debt: Auth override for Cypress tests needs rewriting][user-story-187074] has been completed first).
3. We will need to update the Security tests (if that )
4. Before deploying to prod, we will need to make changes to the authentication and role configuration in Azure.

[adr-add-no-access-page]:
  https://github.com/DFE-Digital/find-information-about-academies-and-trusts/blob/archive/add-no-access-page/docs/adrs/0021-handle-users-authorisation-state-with-roles.md
[branch-add-no-access-page]:
  https://github.com/DFE-Digital/find-information-about-academies-and-trusts/tree/archive/add-no-access-page
[branch-add-no-access-page-add-ui-tests]:
  https://github.com/DFE-Digital/find-information-about-academies-and-trusts/tree/archive/add-no-access-page-add-ui-tests
[branch-add-no-access-page-add-security-integration-tests]:
  https://github.com/DFE-Digital/find-information-about-academies-and-trusts/tree/archive/add-no-access-page-add-security-integration-tests
[user-story-135671]:
  https://dfe-gov-uk.visualstudio.com/Academies-and-Free-Schools-SIP/_workitems/edit/135671
[user-story-186345]:
  https://dfe-gov-uk.visualstudio.com/Academies-and-Free-Schools-SIP/_workitems/edit/186345/
[user-story-187074]:
  https://dfe-gov-uk.visualstudio.com/Academies-and-Free-Schools-SIP/_workitems/edit/187074
[task-186345]:
  https://dfe-gov-uk.visualstudio.com/Academies-and-Free-Schools-SIP/_workitems/edit/186345/
