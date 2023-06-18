# Fut Muse

[![CI](https://github.com/giosn/fut-muse/actions/workflows/ci.yml/badge.svg)](https://github.com/giosn/fut-muse/actions/workflows/ci.yml) [![Staging](https://github.com/giosn/fut-muse/actions/workflows/staging.yml/badge.svg?branch=staging)](https://github.com/giosn/fut-muse/actions/workflows/staging.yml) [![Deployment](https://github.com/giosn/fut-muse/actions/workflows/cd.yml/badge.svg)](https://github.com/giosn/fut-muse/actions/workflows/cd.yml)

## What is it?

Have you ever wanted to find out more about your favorite football (soccer) player, whether he's currently active playing for your favorite club, a historical legend or a retired figure? Do you find yourself jumping from one website to another, becoming overwhelmed by a bunch of stats and charts when all you really care about is knowing what trophies that player has under his name? Oh, and don't forget about ads and buggy websites that are difficult to interact with.

Look no further, [Fut Muse](https://www.futmuse.com) is a website designed for visualizing football player profile and achievements data in a simple, user-friendly manner. Here, you won't find overcomplicated charts or over-the-top stats no one really understands. Just type the name of the player you want to search and quickly find the information you really care about.

## How does it work?

The player data you see on Fut Muse is [web-scraped](https://en.wikipedia.org/wiki/Web_scraping) from the well-known, reputable site you might've never heard of, [Transfermarkt.](https://www.transfermarkt.com) Why Transfermarkt you may be wondering? Well, there are many websites out there where you can find football data about players, but the majority of them are not up-to-date or have limited databases. This is where Transfermakt stands out, not only does it have a huge database, but it also has the latest information and is being constantly updated by an awesome team of data scouts scattered around the globe. 

## How is it made?

Fut Muse uses [Angular](https://angular.io) and [Angular Material](https://material.angular.io) for the user interface and [ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/getting-started/?view=aspnetcore-6.0&tabs=window) for the web API where the web scraping happens and the player data is mapped for easy retrieval. Below is the complete list of tools that are used for this project.

Tool name | Description | Version
--|--|--
Angular | Web framework | [14](https://v14.angular.io/docs)
Angular Material | Component library | [14](https://v14.material.angular.io)
Jasmine | Unit testing | [4.3.0](https://jasmine.github.io)
Karma | Test runner | [6.4.0](https://karma-runner.github.io/latest/index.html)
Cypress | End-to-end testing | [12](https://docs.cypress.io/guides/overview/why-cypress)
<span>ASP.</span>NET Core | Web framework | [6](https://docs.microsoft.com/en-us/aspnet/core/getting-started/?view=aspnetcore-6.0&tabs=windows)
Azure Static Web Apps | Hosting | [Latest](https://learn.microsoft.com/en-us/azure/static-web-apps/)
Azure App Service | Hosting | [Latest](https://learn.microsoft.com/en-us/azure/app-service/)

## How do I run this on my machine?

### Installation

For the web API, make sure to have the latest [.NET 6.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) installed. For installing the client dependencies, navigate to the `client/` directory and run `npm i`, use **Node v16** or higher.

### Development

Open the solution located at `api/FutMuse.sln` with [Visual Studio](https://visualstudio.microsoft.com) and run the project, make sure to set a value for the `UserAgent` key in the `api/appsettings.Development.json` file, otherwise requests will always return `403` (read about user-agent [here](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/User-Agent)). The web API will be available to test at [http://localhost:5000](http://localhost:5000), the solution has to be rebuilt every time a change is made to the API source files. To run the dev server at [http://localhost:4200](http://localhost:4200) for the Angular app run `npm start` in the `client/` directory, it will automatically reload if you change any of the client source files.

For linting the client code, run `npm run lint`.

### Testing

For unit testing, run `npm run test` in the `client/` directory to launch Karma on Google Chrome. For e2e testing run `npm run cy:open` to launch Cypress' Electron instance, choose the *E2E Testing option* on the left, select your desired browser and click on the *Start E2E Testing* option on the bottom. From there you can start running whatever specs are available. Any changes to the client source files will trigger a test run on both testing methods.

Optionally, for a headless one-time test run, run `npm run test:headless` and `npm run cy:run` for unit and e2e testing respectively.

### Build

A [ScrapeOps API key](https://scrapeops.io) is needed for the web API release build to work, once you create an account (it's free) make sure to add yours to the `api/appsettings.json` file (follow the `api/appsettings.Development.json` structure `Secrets:SCRAPEOPS_API_KEY`). Run `dotnet publish -c Release` in the `api/` directory to build the web API, the artifacts will be available in the defaut location under the same directory. For building the client app, run `npm run build:deploy` in the `client/` directory, the artifacts will be available in the `client/dist/` directory.