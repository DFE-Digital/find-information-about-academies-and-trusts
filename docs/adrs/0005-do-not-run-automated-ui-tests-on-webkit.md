# 5. Do not run automated UI tests on Webkit

Date: 2023-12-06

## Status

Accepted

## Context

We have always run UI tests on multiple Playwright emulated browsers - chromium, firefox, edge and webkit. Since implementing functionality involving cookies (such as anti-forgery tokens and analytics consent) we discovered that an issue in the Playwright Webkit browser itself prevents cookies from being properly managed and this is affecting a wide range of tests - some of which have nothing to do with cookies. See <https://github.com/microsoft/playwright/issues/5236> for more information.

## Decision

We initially tried skipping Webkit specifically on the affected tests but as this problem became more widespread and unpredictable we decided to stop our automated tests from running on Webkit altogether.

## Consequences

UI regression and automated a11y issues may go unnoticed in the pipeline on webkit based browsers
