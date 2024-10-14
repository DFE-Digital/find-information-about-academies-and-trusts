# Automated Test creation guide

Use this documentation to assist in automated UI test creation.

## Automated UI tests

### Before creating your automated UI test it is worth first considering

- What areas of the site under test will you be hitting?
- Are there currently usable test hooks/selectors in place that Cypress can use to interact with said page.
- Can your test make use of existing Page Objects and if not what new Page Objects will you need to create in support of your test case/s being added.

### Our current UI automation structure is as such

- We have The E2E tests folder which then houses the regression tests. Within this folder you will see the test specs and some sub test spec folders for areas that may house clustered test areas such as trusts.
- We also have the 'Pages' folder, this houses the page objects that we have in place for reusability and test transparency.
  - Within the Page objects we use an elements structure to make our various objects even more reusable - These should be named to both be clear and explicit to what said element covers.

### When creating your test it is important to have the following tenets in mind

- **Reusability:** Could this test possibly be extended to cover or covered by existing tests? Also when creating page objects can we in any way make these more generic to cover any future features or site functionality.
- **Maintainability:** Our tests should not be flaky and should give us a clear vision of the state of our system. Therefore when creating a test you must be mindful to use robust code that will hold up over time but also be easy to update should something like a html hook change or element of a page moves.
- **Test independance:** Tests should be broken down into independent tests wherein if they fail you know exactly why and were said test is failing for quick triage. Its important that they aren't branching out or covering too many diverging paths and each cover a clear and concise test flow.
- **Data stability:** The tests are running against a real environment with data changing continuously outside of our control. Try to avoid relying on the test data being in a certain state (e.g. "Trust 'x' has 13 academies" or "Searching for 'y' gives 122 results") by being a little fuzzy (e.g. "Trust 'x' has more than 1 academy" or "Searching for 'y' gives several pages of results"). Where this isn't possible (e.g. testing for different behaviour when a trust is missing data) use the page object to clearly state what the data requirement is and to make it easy to change when the test data changes (e.g. governancePage.navigateToEmptyGovernancePage())
- **Confidence:** When creating said test it is very important to also run said test with a negative scenario on any assertions to ensure that you aren't getting a false positive when running said assertation - For example I could have something like 'this.elements.OfstedPage.table().should('contain', 'School name')' but it would be prudent to also run it one time with something like 'this.elements.OfstedPage.table().should('contain', 'abcdefg') which shouldn't exist on the page in this example. This will make clear if you assertions can really be trusted and if they are robust.

### Reporting

- For reporting we currently use Mocha reports - stipulations of this can be amended in the cypress.config.ts. This is also generated when we run in the pipeline.

### Test step naming patterns

- Test steps should be named as such:
  - 'Check' --> For something with a validation we use the term check e.g checkSearchBarIsPresent
  - 'Click' --> This is used for interactive items that can be clicked e.g clickHomeButton
  - 'Enter' --> This should be used when entering data into a field or object e.g enterTextInHomeSearchBar

### Rough automated UI test creation flow

1. Create a branch for your feature/s under test
2. Create the test spec file/s (.cy.ts) - If this is a sub category area of the site like trust for example this may need to be allocated a sub folder within the e2e test section.
3. Next we will want to set up any page object pages you may need for the creation of your tests - this is created in the cypress/pages folder level and not the cypress/e2e folder level.
4. Now we have the correct files created we want to start creating our elements and other supporting code in the page objects so we can start making the building blocks for the tests to pull from.
5. Now the building blocks are in place we want to use these to create test steps in our test spec/s - running the test throughout the creation of each step to ensure we are creating the test as desired and all the elements, functions, methods, classes etc are working correctly.
6. Once our test is created and passing (having checked it multiple times to ensure that the test will not flake out and ran negative tests of any assertions to check for false positives) we can push our code up for a PR and then hopefully merge said tests into the wider suite.
