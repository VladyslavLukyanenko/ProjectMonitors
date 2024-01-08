#!/usr/bin/env bash

mv "../app/snkrs-monitors-management-api" "../app/snkrs-monitors-management-api_tmp"
java -jar ./openapi-generator-cli-4.2.3.jar generate -i "http://localhost:5000/swagger/v1/swagger.json" -g typescript-angular -c codegen-clients-config.json -o "../app/snkrs-monitors-management-api"
rm -r "../app/snkrs-monitors-management-api_tmp"
