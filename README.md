# AlfieBot
A handy bot for online convention things.

How should I invite alfiebot to a server?

https://discordapp.com/oauth2/authorize?client_id=709207294159618109&scope=bot&permissions=738323520 is the link to invite to the server. The permissions granted in this link are general non-admin types of permissions. Adjust as you wish. 

## What can AlfieBot do?

AlfieBot can do some basic chatting, and roll dice! 

Feature suggestions are accepted, just file an issue with your suggestion or find me (gasgesgos) floating around the interwebs.

## Dev setup

It's recommended to use Visual Studio 2019 for development, community edition should have all of the tools/widgets/things to get going. Clone the repo, open the .sln, win.

### Settings/Configuration

BotSettings:BotToken is the configuration key to add your bot token to. The bot token can be found in your Discord development portal, after you define an application and create a bot user for that application. 

The token can be set as an environment variable (not recommended) or a (User Secret)[https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=windows] (recommended).

## Build/deploy

TBD - will likely be in Azure DevOps

## Hosting

Alfiebot lives in the cloud, hosted in Azure.
