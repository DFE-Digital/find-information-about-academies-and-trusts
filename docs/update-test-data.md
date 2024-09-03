# Update test data

Accessibility and UI tests are written using Cypress, with external dependencies (e.g. APIs) mocked using a fake database.

We used to use a console application to generate test data, Cypress tests are strongly hardcoded against a saved version of this fake data in `tests/DFE.FindInformationAcademiesTrusts.CypressTests/data/`.

To update this data:

- Start the fake database container:
  
  ```bash
  docker compose -f ~/docker/docker-compose-db.yml up -d --build
  ```

- Use a SQL query to build a new version of a table you want to replace. e.g.

  ```sql
  --Updated GIAS group link
  SELECT 
      [gl].[URN],
      [gl].[Group UID],
      [gl].[Group ID],
      [gl].[Group Name],
      [gl].[Companies House Number],
      [gl].[Group Type (code)],
      [gl].[Group Type],
      [gl].[Closed Date],
      [gl].[Group Status (code)],
      [gl].[Group Status],
      [gl].[Joined date],
      [e].[EstablishmentName],
      [gl].[TypeOfEstablishment (code)],
      [e].[TypeOfEstablishment (name)],
      [gl].[PhaseOfEducation (code)],
      [e].[PhaseOfEducation (name)],
      [e].[LA (code)],
      [e].[LA (name)],
      [gl].[Incorporated on (open date)],
      [gl].[Open date],
      [gl].[URN_GroupUID]
  FROM gias.GroupLink gl
    JOIN gias.Establishment e ON gl.URN = e.URN
  ```

- Use a database tool to transform the results of the SQL query into an INSERT statement
- Replace the relevant INSERT segment in `tests/DFE.FindInformationAcademiesTrusts.CypressTests/data/insertScript.sql`

## How this data is used to build a database

The docker compose files `docker-compose.ci.yml` and `docker-compose-db.yml` both create a database using the fake data

Both compose files do the following:

- Create a container with SQL running
- Another container will run the create and insert scripts against the running SQL instance
- You will then have a running SQL instance with a sip database that has the fake data loaded
- From this point you can either run the fiat application or run the automated tests against it

`docker-compose.ci.yml` will also run the fiat application in another container (although you will not be able to access this application via your web browser as it is set to only allow authentication via header token)
