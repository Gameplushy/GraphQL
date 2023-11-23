# GraphQL Project

A GraphQL API using C#, EntityFramework and HotChocolate.
This is the in-memory version. ENTITIES WILL NOT BE SAVED BETWEEN RELOADS.

## Prerequisites

You'll need :
- Visual Studio 2022 (with "ASP.NET and web development" kit) (Optional, you can use the cmd if you don't need to debug it.)
- DotNET 7 (download the SDK [here](https://dotnet.microsoft.com/en-us/download/dotnet/7.0))

## How to begin

### Using Visual Studio
Open the solution (.sln file) using Visual Studio. In case there are compiler errors showing up, you might need to get the NuGet packages; right-click the solution and select "Restore NuGet Packages".

### Using cmd
Go to the solution's folder. Use the command `dotnet restore`.

### Using the executable
Download the zip [here](https://github.com/Gameplushy/GraphQL/releases/tag/in-memory-release).

## How to use

### Using Visual Studio
Build and execute the code. A web page should open. Add to the URL `/graphql`. This will open a BananaCakeTop (GraphQL testing page). All you have to do is click "Create Document" the first time and you will be able to test the API using this page.

### Using cmd
In cmd, use `dotnet run`. Follow the given URL (`Now listening on: http://localhost:<port number>`)). Add to the URL `/graphql`. This will open a BananaCakeTop (GraphQL testing page). All you have to do is click "Create Document" the first time and you will be able to test the API using this page.

### Using the exe
Execute the exe file. Follow the given URL (`Now listening on: http://localhost:<port number>`)). Add to the URL `/graphql`. This will open a BananaCakeTop (GraphQL testing page). All you have to do is click "Create Document" the first time and you will be able to test the API using this page.