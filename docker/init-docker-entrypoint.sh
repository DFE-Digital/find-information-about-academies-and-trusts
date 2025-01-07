#!/bin/bash

# exit on failures
set -e
set -o pipefail

while getopts "a:f" opt; do
  case $opt in
    a)
      ACADEMIES_CONNECTION_STRING=$opt
      ;;
    f)
      FIAT_CONNECTION_STRING=$opt
      ;;
    *)
      usage
      ;;
  esac
done

/sql/migrateacademiesdb -v --connection "$ACADEMIES_CONNECTION_STRING"
/sql/migratefiatdb -v --connection "$FIAT_CONNECTION_STRING"
