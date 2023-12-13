# Supercharge your dev environment

Use this documentation to help supercharge your dev environment. We recommend using Rider or Visual Studio with ReSharper.

- [Supercharge your dev environment](#supercharge-your-dev-environment)
- [Set up continuous testing](#set-up-continuous-testing)
- [Analyse test coverage](#analyse-test-coverage)
- [Configure linting and code cleanup](#configure-linting-and-code-cleanup)

## Set up continuous testing

We recommend setting Rider to run unit tests on save, for fast feedback on changes.

- Go to Settings -> Plugins and check that `dotCover` is enabled
- Go to Settings -> Build, Execution, Deployment -> Unit Testing -> Continuous Testing and select 'Automatically start tests in continuous testing sessions on **Save**'
- Go to or open a Unit Tests session (Tests -> Create New Session), open the 'Continuous testing modes' menu and select 'Run all tests'

## Analyse test coverage

This project uses a [mutation score](https://stryker-mutator.io/docs/) to analyse effective test coverage when opening up a new pull request.
Stryker.Net is included in our dotnet-tools manifest for checking mutation score locally.

Install tools by running `dotnet tool restore`.

Open a terminal at the root folder and type the following to run and open a Stryker report:

```bash
dotnet stryker -o
```

You will be able to find all reports in the `StrykerOutput` folder in your project root.

## Configure linting and code cleanup

We recommend setting Rider to clean code on save.

- Open settings
- Configure JavaScript linting
  - Go to Languages & Frameworks -> JavaScript -> Code Quality Tools -> ESLint
  - Select `Manual ESLint configuration`
  - In the dropdown for ESLint package, press the down arrow and select `.../DfE.FindInformationAcademiesTrusts/node_modules/standard`. If nothing is here then ensure you have done an `npm install` in the project directory
  - Tick `Run ESLint --fix on save`
  - Go to Editor > Code style > JavaScript
  - In the top right click `Set from...` and select `JavaScript Standard Style`
- Configure CSS linting
  - Go to Languages & Frameworks -> Style Sheets -> Stylelint
  - Select `Enable`
  - In the dropdown for Stylelint package, press the down arrow and select `.../DfE.FindInformationAcademiesTrusts/node_modules/styleint`. If nothing is here then ensure you have done an `npm install` in the project directory
  - In `Run for files` enter: "{\*_/_,\*}.{css,scss}"
- Configure C# linting
  - Go to Tools -> Actions on Save
  - Tick `Reformat and Cleanup Code`
  - Ensure that Profile is set to `DfE.FindInformationAcademiesTrusts`

If using Rider or Resharper then the `DfE.FindInformationAcademiesTrusts.sln.DotSettings` should be automatically identified and used for manual C# code cleanup.

You can also run linting on JavaScript, CSS and SCSS files using the command line:

```bash
cd DfE.FindInformationAcademiesTrusts
npm run lint ## for a list of issues
npm run lint:fix ## to scan and fix issues
```

### Linting markdown

We use `markdownlint` to check for lint issues on Markdown files in the pipeline.
You can install the [VS code extension](https://marketplace.visualstudio.com/items?itemName=DavidAnson.vscode-markdownlint) to check your files locally.

## Playwright tests

We recommend using VS code to write playwright tests, so that you can take advantage of the extension [Playwright Test for VSCode](https://marketplace.visualstudio.com/items?itemName=ms-playwright.playwright)

### Linting playwright tests

Tests are written in TypeScript and we are using `ts-standard` for linting.
You can configure VS code to use standard as your default formatter:

1. Install VS code extension [StandardJS - JavaScript Standard Style](https://marketplace.visualstudio.com/items?itemName=standard.vscode-standard)

2. Edit your _Settings.json_ file (workspace or user):

```json
{
  "[typescript]": {
    "editor.defaultFormatter": "standard.vscode-standard"
  }
}
```

Alternatively you can use the CLI to check and fix lint issues:

```bash
cd tests/playwright
npm run lint ## for a list of issues
npm run lint:fix ## to scan and fix issues
```
