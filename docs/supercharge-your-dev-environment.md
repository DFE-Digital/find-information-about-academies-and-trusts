# Supercharge your dev environment

Use this documentation to help supercharge your dev environment. We recommend using Rider or Visual Studio with ReSharper.

- [Set up continuous testing](#set-up-continuous-testing)
- [Configure linting and code cleanup](#configure-linting-and-code-cleanup)
  - [Linting markdown](#linting-markdown)
  - [Linting cypress tests](#linting-cypress-tests)
  - [Formatting cypress tests](#formatting-cypress-tests)

## Set up continuous testing

We recommend setting Rider to run unit tests on save, for fast feedback on changes.

- Go to Settings -> Plugins and check that `dotCover` is enabled
- Go to Settings -> Build, Execution, Deployment -> dotCover -> Continuous Testing and select 'Trigger Continuous Testing on **Save**'
- Go to or open a Unit Tests session (Tests -> Create New Session), open the 'Continuous testing modes' menu and select 'Run all tests'

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
You can install the [markdownlint VS code extension](https://marketplace.visualstudio.com/items?itemName=DavidAnson.vscode-markdownlint) to check your files locally.

### Linting cypress tests

Tests are written in TypeScript and we are using `typescript-eslint` and `eslint-plugin-cypress` for linting.
You can install [ESLint VS code extension](https://marketplace.visualstudio.com/items?itemName=dbaeumer.vscode-eslint) to highlight errors caught by the linter as you code.

### Formatting cypress tests

We recommend using VS code to format your code as you work.

In your [settings](https://code.visualstudio.com/docs/getstarted/settings) change these values:

- `Editor: Format On Save` => :white_check_mark:
- `Editor: Format On Type` => :white_check_mark:
- `Files: Insert Final Newline` => :white_check_mark:
- `TypeScript › Format: Semicolons` => `insert`

Alternatively, you add the following to your [settings.json file](https://code.visualstudio.com/docs/getstarted/settings#_settings-json-file)

```json
{
    "editor.formatOnSave": true,
    "editor.formatOnType": true,
    "files.insertFinalNewline": true,
    "typescript.format.semicolons": "insert"
}
```

### Linting Sonar rules

Include the following extension in your IDE installation: [SonarQube for IDE](https://marketplace.visualstudio.com/items?itemName=SonarSource.sonarlint-vscode)

Update your [settings.json file](https://code.visualstudio.com/docs/getstarted/settings#_settings-json-file) to include the following

```json
"sonarlint.connectedMode.connections.sonarcloud": [   
    {
        "connectionId": "DfE",
        "organizationKey": "dfe-digital",
        "disableNotifications": false
    }   
]
```

Then follow [these steps](https://youtu.be/m8sAdYCIWhY) to connect to the SonarCloud instance.
