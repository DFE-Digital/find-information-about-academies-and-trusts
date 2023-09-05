# 2. Use NetEscapades for security headers

Date: 2023-09-05

## Status

Accepted

## Context

Up until now, we have been setting static response security headers in a custom middleware. We encountered some scenarios where we need to use inline `<script>` tags:

* The GOV.UK design system requires the use of inline script tags (to add the `js-enabled` class name).
* We may also need to use inline script to initialise the accessible-autocomplete component, which we will be using for search.

These uses of inline script should be secured with nonces.

## Decision

We will use [NetEscapades.AspNetCore.SecurityHeaders nuget package](https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders) to secure our inline scripts. This package is widely used across RSD products and is recommended by the [SDD technical documentation repo](https://github.com/DFE-Digital/sdd-technical-documentation/blob/main/how_to_guides/security_hardening_in_aspnet_core_web_apps.md).

In order to keep our security header configuration consistent, we will also use this package to configure our other security headers.

## Consequences

We have added nonce attributes to inline scripts using the [NetEscapades.AspNetCore.SecurityHeaders nuget package](https://github.com/andrewlock/NetEscapades.AspNetCore.SecurityHeaders).

We have made further use of NetEscapade security header methods to replace the majority of security headers previously set in our middleware—so that all of the security headers are set in one place.

We still need to manually set the X-Robots-Tag using the custom middleware, to define our desired search engine behaviour.
