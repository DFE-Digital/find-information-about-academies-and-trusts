# Docker Issues on Windows

There can be some issues with running Docker on Windows. If your container doesn't start or you see an error like this `/bin/bash^M: bad interpreter: No such file or directory` it may be caused by line endings in the bash shell script. To solve this you can run this command in git bash (or another shell of your choice) from the playwright directory:

 `sed -i -e 's/\r$//' ../../docker/web-docker-entrypoint.sh`

This will re write your line endings. The repo will ignore this so checking in the updated file will not fix it unfortunately.
