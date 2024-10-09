# 18. Run FIAT database migrations on Docker container start

**Date**: 2024-10-09

## Status

Accepted

## Context

We have been asked to update our font to match the new DfE standard font named [Inter](https://rsms.me/inter/). We tried following the steps outlined in the [DfE design manual](https://design.education.gov.uk/design-system/dfe-frontend/styles/typography) to update the font through Sass, however even after updating Content Security Policy and adding exceptions did not allow the font to load, and it was blocked by COEP policy. Exact error is as follows:
`net::ERR_BLOCKED_BY_RESPONSE.NotSameOriginAfterDefaultedToSameOriginByCoep`.

## Decision

So as to not take up an extended amount of time solving this issue, we chose to host the fonts ourselves and use a NPM package to help manage and install them. The Inter font project recommended the package [Inter UI](https://www.npmjs.com/package/inter-ui) which we will use to load the fonts. However we should look into trying to load the fonts from the CDN in the future as this is the recommended way of doing it and will decrease our reliance on external packages.
A tech debt ticket has been created to review this in the future [here](User Story 183724: Tech debt: Review how we are serving the Inter font).

## Consequences

- Added the NPM package inter-ui to the project.
- Although this works we may have to incurr technical debt to switch to using the CDN method in the future or update/change the package to update the font in the future.
