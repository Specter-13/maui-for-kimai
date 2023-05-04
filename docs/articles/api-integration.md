# Integration and authorization of Kimai API

This article describes integration and authorization of Kimai API. This API contains
the OpenAPI specification, which defines a standard for HTTP APIs which allows both
humans and computers to discover and understand the capabilities of the service. This
standardized specification can be used for auto-generation of the API client for different
applications.

I used NSwag toolchain, specifically the NSwagStudio application, for the generation
of the API client for the stable version of Kimai 1.30.11. The API client uses .NET HTTP
client for sending and receiving request and NewtfonSoft.Json library for serializing and
deserializing requests.


## Authorization and login context

For testing purposes, authorization tokens were added to every request globally as Default
Request Headers. These headers can be set only once on the initialization of the HTTP
client. However, this approach is not feasible for the case where a new user or server is
added to the application and new credentials are needed. As a result of this, custom HTTP
Message handler is introduced.
The HTTP client factory (object, which handles initialization and de-initialization
of HTTP clients) is initialized on application startup and added to the DI container.
This HTTP client factory is provided with my custom HTTP request handler named
AuthHandler. AuthHandler contains override of SendAsync method from HTTP client.
This is main method, which is invoked every time, when new HTTP request is send. In
this overridden method, authorization headers are added to each request. The example of
the overridden SendAsync method is shown in the code snippet below.

```C#
protected override async Task<HttpResponseMessage> SendAsync(request, cancellationToken)
{
    request.Headers.Add("X-AUTH-USER", _loginContext.UserName);
    request.Headers.Add("X-AUTH-TOKEN", _loginContext.ApiPassword);
    return await base.SendAsync(request, cancellationToken);
}
```

The credentials needed for authorization are provided by a special class called LoginContext.
The LoginContext is a singleton object, which contains essential information about the cur-
rently logged user to a specific Kimai server. It is derived from Observable object (from
the Community Toolkit), which means it implements the INotifyPropertyChanged inter-
face. That means, when an authorization context is changed (for example, user will log in
or log out into application), the UI and viewmodels are notified about the change and are
reinitialized accordingly.

All this is glued together with the implementation of Login service. The Login
service provides methods for logging in and logging out users to the Kimai server. These
methods initialize and deinitialize accordingly LoginContext with credentials and also reini-
tialize HTTP clients that are used in API services (specific abstractions of the generated
API client). 