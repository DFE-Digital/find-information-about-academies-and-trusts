# 16. Create a new Cypress test suite to replace both the old Cypress suite and old Playwright suite

**Date**: 2024-10-08

## Status

Accepted

## Context

The old Cypress suite was not quite fit for purpose for both test and development use cases.

The tests used a journey based test approach which does not quite fit the flow and layout of FIAT. It also did not fully replace/encompass that which existed before it in the Playwright tests.

From a test perspective the old suite did not quite hit the target of being easy to understand from someone from a test background who may be less experienced with coding and more developer based knowledge sets.

From a dev perspective the old suite did not quite give them the confidence that their code was being adequately tested.


## Decision

The decision was to create a new Cypress suite that both encompassed the old Playwright coverage and create a Cypress suite created from the ground up with a test mindset behind each part of said suite.

This was then worked on my Test in close tandem with the Dev team ensuring that no old coverage was missed and that we also captured all requirements from test and dev.

We also adapted the pipeline to be more in line with the new suite as previously we had deployment tests and the UI tests but we landed on just having a dedicated UI test run.

As part of the work we also assessed the previously used docker/faker system and moved this away to pointing at test and dev as they are closer to what the live product is like and give us more confidence rather than running against hard coded/faker data.


## Consequences
The end result was a robust suit of over 60 tests that not only captured old covered functionality but expanded upon and brought more confidence to the tech team.

We now also have a good jumping off point to create new tests in as the sprints go forward.