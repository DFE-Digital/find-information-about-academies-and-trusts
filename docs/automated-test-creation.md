# Automated Test creation guide

Use this documentation to assist in automated UI test creation.

## Automated UI tests
Before creating your automated UI test it is worth first considering:
- What areas of the site under test will you be hitting? 
- Are there currently usable test hooks/selectors in place that Cypress can use to interact with said page.
- Can your test make use of existing Page Objects and if not what new Page Objects will you need to create in support of your test case/s being added.

Our current UI automation structure is as such:
- We have The E2E tests folder which then houses the regression tests. Within this folder you will see the test specs and some sub test spec folders for areas that may house clustered test areas such as trusts.

- We also have the 'Pages' folder, this houses the page objects that we have in place for reusability and test transparency.
>> - Within the Page objects we use an elements structure to make our various objects even more reusable - These should be named to both be clear and explicit to what said element covers.

- When creating your test it is important to have the following tenets in mind:
>> - Reusability: Could this test possibly be extended to cover or covered by existing tests? Also when creating page objects can we in any way make these more generic to cover any future features or site functionality.

>> - Maintainability: Our tests should not be flaky and should give us a clear vision of the state of our system. Therefore when creating a test you must be mindful to use robust code that will hold up over time but also be easy to update should something like a html hook change or element of a page moves.

>> - Test independance: Tests should be broken down into independent tests wherein if they fail you know exactly why and were said test is failing for quick triage. Its important that they aren't branching out or covering too many diverging paths and each cover a clear and concise test flow. 

- For reporting we currently use Mocha reports - stipulations of this can be amended in the cypress.config.ts. This is also generated when we run in the pipeline.