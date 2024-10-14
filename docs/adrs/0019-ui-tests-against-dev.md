# 19. Use Dev Environment for UI Tests

**Date**: 2024-10-14  

## Status

Accepted

## Context

Previously, we had two types of Cypress tests for our UI:

- **Deployment Tests**: These pointed to the live URL of our Dev environment.
- **Regression Tests**: These ran against an isolated container environment spun up by the pipeline, using a substantial fake database setup.

The regression testing setup, while isolated and customisable, introduced overhead due to the maintenance of the fake database and the complexity of managing isolated containers. Given the project's direction and needs, we re-evaluated this approach and made the decision to streamline it.

## Decision

We decided our new suite of Cypress tests will point to the Dev environment and its live URL. This offers several benefits while addressing the challenges we faced with the isolated container approach.

## Reasons for the Decision

1. **Closer to Accurate Data**: The Dev environment is connected to the **Dev Academies Database**, which mirrors production changes to it's structure (not neccesarily it's data) more closely than the isolated database we previously used. By utilising this, we ensure that our tests are running against data that is representative of what will be in other environments. This helps us catch issues that arise due to real-world changes in the data.

2. **Simplification**: The process becomes significantly simpler. By removing the need to maintain and manage the fake database and isolated containers, we reduce the complexity of our testing pipeline.

3. **Reduced Maintenance Overhead**: Removing the fake database means we no longer need to maintain its data model or ensure that it reflects changes occurring in real databases.

## Consequences

- **Reduced Control over Test Data**: By moving away from an isolated database, we lose some control over the ability to insert and manipulate data in specific ways. While this was useful in certain edge cases, it also came with a significant overhead.

- **Less Ability to Simulate Specific Test Scenarios**: With the isolated database, we could simulate "strange" or specific data conditions. In the Dev environment, we rely more on natural data, meaning we may miss certain edge cases or need to implement workarounds for testing such scenarios.

## Trade-offs

- **Simplified Test Infrastructure vs. Data Control**: The major trade-off is between simplification and the loss of fine-grained control over the test data. This is acceptable given the current project focus and the expected benefits of quicker, more reliable test runs.

- **Test Reliability**: By using a shared Dev environment, tests might be impacted by changes happening concurrently in the environment. However, given the benefits of using more accurate data, this is a risk we are willing to take for now.

## Future Considerations

If we encounter frequent issues with data consistency or reliability due to the shared nature of the Dev environment, or if our write operations increase significantly, we may reconsider using a different environment or reintroducing some level of isolation in the testing process.
