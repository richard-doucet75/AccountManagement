# Account Management Services
Implements a set over services to manage user accounts. see Swagger Endpoint (default) for details.

Currently implements:
+ Create Account
+ Change Password
+ Change Email
+ Login
+ GetAccount

## Description ##
This is sample code using dotnet in a clean domain style.  It uses docker-compose and powershell scripts to provide a good developer experience.  It exposes the domain over a minimal api with a swagger interface.

## Developer Prerequisites ##
Requires the following:
+ dotnet 7.0
+ docker
+ Powershell
+ An IDE of your choosing (Rider, Visual Studio)

## Setup ##
From the solution folder run the following commands:
### admin-setup-dev-environment-variables.ps1 ###
This will setup environment variables needed for docker-compose and the ef migrations.  You should restart any IDEs you may have running so that the changes can take effect.

### run-migrations.ps1  ##
This will start the sql-server service in docker-compose and run the ef migrations.

You can launch the application either though your IDE or by running the docker-compose-up.ps1 script.  This script will ensure docker-compose start up the application using the same profile as the IDE.



## Technical Note ##
### Keeping Microsoft.AspNetCore.Authentication.JwtBearer ** Compile Warning ** ###
** Known Issue:  ** This component uses a preview version of System.Text.Json which causes a conflict warning with the Framework.

