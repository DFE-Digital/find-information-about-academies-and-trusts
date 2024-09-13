# 12. Use ClosedXML Library for Excel File Manipulation

**Date**: 2024-09-13

## Status

Accepted

## Context

We need a reliable way to handle Excel file manipulation in our FIAT project. Various libraries were evaluated, including ClosedXML, EPPlus, and NPOI, each with its own pros and cons.

- **ClosedXML** is widely used, open-source (MIT license), and a wrapper around OpenXML, making it a strong candidate due to its ease of use and low licensing risk. It has 75 million downloads and is widely accepted in the development community.
- **EPPlus** is another popular choice but requires a commercial license for certain use cases, which incurs additional costs.
- **NPOI** is an alternative, but there are concerns regarding its association with political issues.

While ClosedXML is actively maintained, the authors have indicated that breaking changes are introduced in some versions, which implies some caution is required when upgrading.

## Decision

We will use the ClosedXML library for Excel file manipulation within FIAT. This decision is based on its:

- Prevalence and wide community usage (15.5k daily downloads).
- Open-source MIT license, which avoids potential licensing costs.
- Simplified wrapper around OpenXML, making it easier for developers to interact with Excel files.

Given that our use case is straightforward and doesn't rely on complex or bleeding-edge functionality, the risk of using ClosedXML is minimal. However, we will take care when upgrading to newer versions, reviewing the release notes for breaking changes.

Should we encounter issues in the future, we will migrate to OpenXML, Microsoft's maintained package for manipulating Excel files. While OpenXML is more complex to use, it is a more stable and long-term solution should ClosedXML become unsuitable.

## Consequences

- The library offers a straightforward way to manipulate Excel files, reducing development complexity and time.
- The open-source nature of the library ensures no additional licensing costs, unlike EPPlus.
- Some risk of breaking changes when updating the library exists, so a strategy for version management and thorough testing during upgrades will be essential.
  
This ADR will be revisited if major issues arise or if simpler alternatives (like directly using OpenXML) prove more beneficial in the future.
