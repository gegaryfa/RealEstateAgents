# Real Estate Agents

![GitHub stars](https://img.shields.io/github/stars/gegaryfa/RealEstateAgents)

.Net Core API -  This is a simple API that fetches properties(houses, apartments etc.) from an external API and return the top Agents with the most listings.

## Getting Started

Clone the repo and run it locally to spin up the project.

The project setup is based on Domain Driven Design.

### Prerequisites

* [`GNU Make`](https://www.gnu.org/software/make/) - optional
* [`.Net Core v5.0`](https://dotnet.microsoft.com/download/dotnet/5.0)
* [`Docker`](https://www.docker.com/get-started)
* Your favorite IDE/editor


### Installing / Using

You can run the `RealEstateAgents.WebApi` whch will start the project at 'http://localhost:57712/swagger/index.html'. This landing page is the swagger of the API.

After doing a request to the API, the properties of this specific request are being cached to save bandwidth and time.

## Built With

* [.Net Core v5.0](https://dotnet.microsoft.com/download/dotnet/5.0)
* [AutoMapper](https://automapper.org/) - A convention-based objet-objet mapper.
* [MediatR](https://github.com/jbogard/MediatR) - Simple mediator implementation in .NET.
* [Swagger](https://swagger.io/) - Tools for documenting APIs.
* [RestEase](https://github.com/canton7/RestEase) - REST API client library for .NET.
* [Polly](https://github.com/App-vNext/Polly) - Resilience and transient-fault-handling library.

## Sources
A good read on distributed caching for .Net Core: [Distributed caching in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-3.1).


## Todo
 * Add Docker support
 * Support Redis cache
 * Add unit tests 😁

## Authors

* **George Garyfallou** - *Initial work* - [gegaryfa](https://github.com/gegaryfa)

## Acknowledgments

* Hat tip to anyone whose code was used

