
# Medical Examiner API

## Okta Integration Configuration Requirements

To get access to the API you need to configure Okta integration. Populate the Client Id and Secret with those specified in the application.

The OKTA_URL is your okta domain.

```json
  "Okta": {
    "ClientId": "...",
    "ClientSecret": "...",
    "Authority": "{OKTA_URL}/oauth2/default",
    "Audience": "api://default",
    "IntrospectUrl": "{OKTA_URL}/oauth2/default/v1/introspect"
  },
```

##Loading locations data into a CosmosDB

Locations data needs to be loaded into a CosmosDB database.

There is a json file in the Source_Data folder that contains a list of locations to be loaded.

Loading can be facilitated by using the MedicalExaminers.ReferenceDataLoader. To do this:

Navigate to [Project directory]\MedicalExaminer.ReferenceDataLoader\bin\Release\netcoreapp2.1\publish

In this folder there is a file RunMedicalExaminerReferenceDataLoader.bat

Edit this file to set the following:
endpoint of Cosmos DB 
primary Key
databaseID
json file to import (with full path)
containerId
