
# Medical Examiner Tool Box

The Medical Examiner Tool Box is a simple MVC application designed to be run locally during development to facilitate running routine actions for testing and development.

## Dockerised

The MVC end point is dockerised and runs on port 9001;

```bash
docker-compose build
docker-compose up
```

## Setup

The project utilises the same configuration keys as the API project so simply copy your `appsettings.*.json` files to this project.

## Testing

### Impersonate (WARNING; MODIFIES DATABASE)

This feature allows you to switch user accounts by forcing the login details for the account to be the one your using via okta.

The user you want to impersonate is given your email so logging in via okta with that email now links you to the selected user.

All other users have their email addresses reset to avoid any cross over.

### Generate (WARNING; DESTROYS LOCATION, USER AND PERMISSION DATA)

This feature allows you to generate a set of location, user and permission data for testing.

WARNING: All existing Locations, Users and Permissions are deleted from the configured database!!

