# 20. Handle users authorisation state with roles

**Date**: 2024-10-28

## Status

Accepted

## Context

Currently, when a new user tries to access FIAT without having been added to the correct Azure AD group, they encounter a daunting default error page from Mucrosoft which tells them to contact their "admin".

We want to create a better user experience for people trying to get access to FIAT by redirecting unauthorised users to a page with helpful information.

We were entirely delegating both the **authorisation** and **authentication** to the Microsoft Azure Enterprise application which meant that we were unable to redirect on login failure. Some other applications in the program have enabled a no access page redirect by assigning Roles to user groups in the Microsoft Azure Enterprise application and then moving **authorisation** into the actual web application's codebase.

## Decision

We want to keep consistency with other products in RSD and we also couldn't see another way to enable a redirect to one of our own pages so we decided to adopt the same approach.

- Users permitted to use FIAT will still be onboarded in the same way by being added to the correct Microsoft Entra group.
- **Authentication** is still managed by `Microsoft.Identity.Web` and Microsoft Entra.
- **Authorisation** will be managed by role claims configured in the Microsoft Azure Enterprise application and then evaluated by the FIAT application.

This requires some changes to the authorisation process in FIAT and has implications for security.

- By default every page and route in FIAT will be secured and require the correct user role claim to access.
- The no-access, accessibility, cookies and privacy pages will be specifically opened to unauthorised users.
- The cookies banner should still function for unauthorised users.
- The header and footer will not render components which link to protected areas of the site (such as the Home breadcrumb or header trust search box) for unauthorised users.

## Consequences

There is a disconnect between how we persist login information (in a cookie) and what a user's most up to date role information may be.

When we release the role based authorisation some current users of the may have outdated role claims in their login cookie - we don't want these existing users to face any change or interruption to their use of the application so we mapped out the different states a user can be in at the point they try to access a page on FIAT and whether or not they should be able to access secured pages.

| Example user                                                                                                                                              | Has login cookie   | Has FIAT user role claim in login cookie | Has FIAT user role in azure | Should have access to secured FIAT pages?         |
| --------------------------------------------------------------------------------------------------------------------------------------------------------- | ------------------ | ---------------------------------------- | --------------------------- | ------------------------------------------------- |
| A long-term FIAT user                                                                                                                                     | :heavy_check_mark: | :heavy_check_mark:                       | :heavy_check_mark:          | :muscle: has access                               |
| FIAT User that has just had permissions revoked                                                                                                           | :heavy_check_mark: | :heavy_check_mark:                       | :x:                         | :warning: has access until end of browser session |
| User that followed the get access link on the no access page and has just had access granted (also existing users at the time of release of this feature) | :heavy_check_mark: | :x:                                      | :heavy_check_mark:          | :muscle: has access                               |
| User that has not been setup to use FIAT but has tried to access FIAT before                                                                              | :heavy_check_mark: | :x:                                      | :x:                         | :no_entry: no access                              |
| Invalid state - a cookie can't contain a role if the cookie does not exist                                                                                | ~~:x:~~            | ~~:heavy_check_mark:~~                   | ~~:heavy_check_mark:~~      | n/a                                               |
| Invalid state - a cookie can't contain a role if the cookie does not exist                                                                                | ~~:x:~~            | ~~:heavy_check_mark:~~                   | ~~:x:~~                     | n/a                                               |
| Brand new User that has just been told they have access                                                                                                   | :x:                | :x:                                      | :heavy_check_mark:          | :muscle: has access                               |
| Brand new User that has not been setup to use FIAT                                                                                                        | :x:                | :x:                                      | :x:                         | :no_entry: no access                              |

This exercise allows us to ensure that we cover all permutations of user state to ensure that the user journey for each user is as simple and intuitive as possible for both new and existing users.

We also found that there were several different types of journey that an authorised or unauthorised user might want to make and we needed to behave differently for each

| Request                                                         | Authorised FIAT user outcome | Unauthorised user outcome                                                            |
| --------------------------------------------------------------- | ---------------------------- | ------------------------------------------------------------------------------------ |
| Is navigating to a standard fiat page (like home or a trust)    | Should go to intended page   | Should be redirected to no-access with return url of page they were trying to access |
| Is navigating to an always accessible fiat page (like cookies)  | Should go to intended page   | Should go to intended page                                                           |
| Is navigating to no-access with return url                      | Should go to return url      | Should go to no-access page with return url                                          |
| Is navigating to no-access on purpose (i.e. without return url) | Should go to no-access page  | Should go to no-access page                                                          |

## Implementation

We created a new flow triggered upon navigation to the "No Access" page to ensure that a user's role is up to date and we're not relying on stale claims in a cookie:

```mermaid
---
title: Flow upon navigating to "No Access" page 
---
flowchart TB
    %% ~~~~~ Node declarations ~~~~~
    %% Entry nodes
    auth-fail-redirect(["**Entry point**: ASP.NET core framework redirecting to no-access due to auth failure (original path included as return url)"])
    direct-nav-with-return(["**Entry point**: Direct visit to no-access page with return url (e.g. page refresh)"])
    direct-nav-no-return(["**Entry point**: Direct visit to no-access page without return url"])

    %% Main nodes
    local-return-url{Is there a local return url?}
    on-get[On GET for no-access page]
    has-role{"`Does user have FIAT user role claim? 
            (Note that claims are persisted in the login cookie and will not be retrieved from Azure)`"}
    refresh-login-cookie["Force re-login by removing login cookie (if it exists)"]
    login-retry["Mark in TempData that we're 'RetryingLogin'"]
    login-redirect-return-url[Redirect to local return url]
    login-retry-auth-fail-redirect[ASP.NET core framework redirects back to no-access page]
    login-user-logs-in[User logins in]
    already-retried-login{Does TempData show that we've already retried the login to update the role claims?}
    remove-tempdata["Remove 'RetryingLogin' from TempData"]
    redirect-return-url[Redirect to local return url]

    %% Exit nodes
    render-no-access([Render no-access page])
    render-requested-page([Render requested page])

    %% ~~~~~ Chart links ~~~~~
    auth-fail-redirect --> on-get
    direct-nav-with-return --> on-get
    direct-nav-no-return --> on-get

    on-get --> local-return-url
    local-return-url -- yes --> has-role
    local-return-url -- no --> render-no-access

    has-role -- yes - auth success --> redirect-return-url --> render-requested-page
    has-role -- no --> already-retried-login

    already-retried-login -- no - cookie might be stale --> refresh-login-cookie --> login-retry
    already-retried-login -- yes - auth has failed --> remove-tempdata --> render-no-access
    
    login-retry --> login-redirect-return-url --> login-user-logs-in
    login-user-logs-in -- auth success --> render-requested-page
    login-user-logs-in -- auth failure --> login-retry-auth-fail-redirect --> on-get
```

Triggering this flow on the `OnGet` of the no access page (rather than by using an authentication handler or cookie authentication events) ensures that we're only doing this extra work in the exceptional circumstance of a user auth failure and gives us easy access to the intended route and the Tempdata. It also allows us to handle cases such as when auth succeeds for a new user but they're just refreshing an old browser tab which had previously redirected to the no-access page.
