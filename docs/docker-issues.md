# Docker Issues

## On Windows

There can be some issues with running Docker on Windows. If your container doesn't start or you see an error like this `/bin/bash^M: bad interpreter: No such file or directory` it may be caused by line ending differences in the bash shell script (which we've been unable to fix in the remote repository).

1. Open git bash
2. Navigate to the docker directory
3. Rewrite your line endings by running:

   ```bash
   sed -i -e 's/\r$//' web-docker-entrypoint.sh
   ```

4. Stage your changes (they will disappear after being staged)
5. Rebuild and run the Docker container - it should now work

## On Macs

If you are running on Apple M1 chip the SQL Server image (used by the test db) may not work. This can be fixed by:

- Docker Settings > General: [X] Use virtualization framework and
- Docker Settings > Features in Development: [X] Use Rosetta...
