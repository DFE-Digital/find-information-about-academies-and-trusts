# FIAT Performance tests

This directory holds the performance test scripts for the FIAT service.

Tests are written for the [k6](https://k6.io) performance testing tool.

## Setup

You will need k6 installed to be able to run these tests. Details on how to do so are available in their [documentation](https://grafana.com/docs/k6/latest/get-started/installation/).

## Configuration

The variables you will need to set are defined below:

| Variable | Description | Example |
|---|---|---|
| `AUTHORIZATION_HEADER` | The header used to inject auth in test environments | `authorization-header` |
| `BASE_URL` | The url of the service to be tested | `https//localhost:5000` |

## Running the tests

To run an individual script, navigate to the correct directory and run

`k6 run -e AUTHORIZATION_HEADER=<your-value> -e BASE_URL=<your-value> <your-script-name>`

## Results

By default, metrics are output to the console at the end of the tests, including any checks that are run as part of the test scripts.