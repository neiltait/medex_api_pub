
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
