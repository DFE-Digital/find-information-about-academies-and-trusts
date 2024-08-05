# 9. Memory cache frequently accessed data

Date: 2024-08-05

## Status

Accepted

## Context

The application is suffering noticeable performance issues due to frequently accessed data (such as "when was the GIAS pipeline last run") being slow to retrieve from the database.

## Decision

Use a Memory cache to store frequently accessed data that is slow to retrieve and not often updated.

## Consequences

The performance improves however care must be taken not to allow the memory cache to grow too large otherwise this could cause the application to slow down as it consumes all of its allocated runtime resources.
