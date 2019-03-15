
# Medical Examiner API

## Running the API

You now have 3 options for running the API depending on your requirements.

### Running the API via Visual Studio using IIS Express (non docker)

Make sure to select the startup project as `MedicalExaminer.API` and run as normal

### Running the API via Visual Studio using Docker

Make sure to select the startup project as `docker-compose` and run as normal.

### Running the API via Docker Compose

* Open a power shell terminal
* Change directory to the root of the API; i.e. `.../MedicalExaminer.API/`
* Run `docker-compose up --build`
* If you change anything you need to rebuild and run again using the above command

## Okta Integration Configuration Requirements

To get access to the API you need to configure Okta integration. Populate the Client Id and Secret with those specified in the application.

The OKTA_URL is your okta domain.

```json
  "Okta": {
    "ClientId": "...",
    "ClientSecret": "...",
    "Authority": "{OKTA_URL}/oauth2/default",
    "Audience": "api://default",
    "IntrospectUrl": "{OKTA_URL}/oauth2/default/v1/introspect",
  },
```

## Okta SDK Integration

This allows us to query the users within Okta and look up their details; we need to add 2 more fields to the Okta settings:

```json
  "Okta": {
    "Domain": "https://dev-XOXOXO.oktapreview.com",
    "SdkToken": "..."
  },
```

The SdkToken is generated within OKTA.

## Loading locations data into a CosmosDB

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

# FAQ

## Switching between Docker, Visual Studio and IIS

If you decide to shake things up a bit and switch between these you need to make sure you clear down the other before starting.

If you used IIS previously make sure you remove the site from IIS to prevent port conflicts; you can do this from the task tray context menu.

If you used docker-compose you might need to destroy the docker containers before you hit play in visual studio since VS does it slightly differently.

And visa-versa if you use Visual studio first and want to use docker-compose directly. Just clear down the containers and start again

## Delete all my dontainers

Please make sure you aren't running other projects or containers

You can run this in a powershell window to stop all containers and remove them all.

```
docker stop $(docker ps -a -q)
docker rm $(docker ps -a -q)
```
