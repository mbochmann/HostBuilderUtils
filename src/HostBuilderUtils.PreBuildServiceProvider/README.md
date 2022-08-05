# PreBuildServiceProvider

PreBuildServiceProvider is an easy extension for using services through `IServiceProvider` before the application is built.
This adds the ability to already log while configuring your application or use other useful tools.

## Usage
1. Add the nuget package to your project
```ps
Install-Package HostBuilderUtils.PreBuildServiceProvider
```
or
```ps
dotnet add package HostBuilderUtils.PreBuildServiceProvider
```
2. Add the using directive
```csharp
using HostBuilderUtils.PreBuildServiceProvider;
```
4. Add `AddPreBuildServiceProvider()` to your host configuration
```csharp
Host
  .CreateDefaultBuilder(args)
  .AddPreBuildServiceProvider();
```
5. Use the new overloading method of `ConfigureServices` and make use of the `IServiceProvider`
```csharp
Host
  .CreateDefaultBuilder(args)
  .AddPreBuildServiceProvider()
  .ConfigureServices((ctx, services, serviceProvider) =>
  {
      if (serviceProvider?.GetService<ILogger<Program>>() is ILogger logger)
          logger.LogInformation("Configure services");
  })
```
## Position of commands and constraints

Please position the command:
1. After adding a `ServiceProviderFactory` if you add one
2. After configuring services which should already be resolvable when using the overloaded `ConfigureServices`
3. Before `ConfigureServices` with the overloaded `IServiceProvider`
4. You can call `AddPreBuildServiceProvider()` only once.

Example:
```csharp
Host
  .CreateDefaultBuilder(args)
  .UseUnityServiceProvider() //Adding a custom ServiceProviderFactory like Unity
  .ConfigureServices((ctx, services) => 
  {
      //Configuring services which should be available when using the overloaded method
      //services.AddSingleton(...);
  })
  .AddPreBuildServiceProvider()//Makes the IServiceProvider available
  .ConfigureServices((ctx, services, serviceProvider) =>
  {
      //Resolve services which are already added to services
      if (serviceProvider?.GetService<ILogger<Program>>() is ILogger logger)
          logger.LogInformation("Configure services");
  })
  .ConfigureServices((ctx, services) => 
  {
      //You can call configure services like usual more than once
  });
```
