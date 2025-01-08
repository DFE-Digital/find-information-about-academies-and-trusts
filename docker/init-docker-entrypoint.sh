#!/bin/bash

# exit on failures
set -e
set -o pipefail

while getopts "c:" opt; do
  case $opt in
    c)
      CONNECTION_STRING=$OPTARG
      ;;
    *)
      ;;
  esac
done

/sql/migratefiatdb -v --connection "$CONNECTION_STRING"
