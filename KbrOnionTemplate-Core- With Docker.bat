@echo off
set /p "app=Enter application name: "
rem echo Are You Sure?
choice /c YN /M "Are You Sure?"
if %errorlevel%==1 goto yes
if %errorlevel%==2 goto no
:yes
echo Creating %app%
if not exist %app% mkdir %app%
cd %app%

rem dotnet new mvc -au Individual -uld -o %app%.Mvc
dotnet new blazor -au None -o %app%.WebUi
dotnet new classlib -o %app%.Application
dotnet new classlib -o %app%.Domain
dotnet new nunit -o %app%.Domain.Test
dotnet new classlib -o %app%.Infrastructure

dotnet add %app%.WebUi reference %app%.Application
dotnet add %app%.WebUi reference %app%.Domain
dotnet add %app%.WebUi reference %app%.Infrastructure
dotnet add %app%.Domain.Test reference %app%.Domain
dotnet add %app%.Application reference %app%.Domain
dotnet add %app%.Infrastructure reference %app%.Domain
dotnet add %app%.Infrastructure reference %app%.Application

dotnet new sln -n %app%
dotnet sln %app%.sln add %app%.WebUi/%app%.WebUi.csproj
dotnet sln %app%.sln add %app%.Application/%app%.Application.csproj
dotnet sln %app%.sln add %app%.Domain/%app%.Domain.csproj
dotnet sln %app%.sln add %app%.Domain.Test/%app%.Domain.Test.csproj
dotnet sln %app%.sln add %app%.Infrastructure/%app%.Infrastructure.csproj

dotnet add %app%.WebUi/%app%.WebUi.csproj package Microsoft.EntityFrameworkCore
dotnet add %app%.WebUi/%app%.WebUi.csproj package Microsoft.EntityFrameworkCore.SqlServer
dotnet add %app%.WebUi/%app%.WebUi.csproj package Microsoft.EntityFrameworkCore.Tools
dotnet add %app%.WebUi/%app%.WebUi.csproj package Microsoft.EntityFrameworkCore.Design
dotnet add %app%.WebUi/%app%.WebUi.csproj package Microsoft.VisualStudio.Web.CodeGeneration.Design
dotnet add %app%.WebUi/%app%.WebUi.csproj package Microsoft.AspNetCore.Components.QuickGrid.EntityFrameworkAdapter

dotnet add %app%.Application/%app%.Application.csproj package Microsoft.Extensions.DependencyInjection
dotnet add %app%.Application/%app%.Application.csproj package Microsoft.Extensions.Configuration.Abstractions

dotnet add %app%.Infrastructure/%app%.Infrastructure.csproj package Microsoft.EntityFrameworkCore
dotnet add %app%.Infrastructure/%app%.Infrastructure.csproj package Microsoft.EntityFrameworkCore.SqlServer
dotnet add %app%.Infrastructure/%app%.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Tools
dotnet add %app%.Infrastructure/%app%.Infrastructure.csproj package Microsoft.Extensions.DependencyInjection
dotnet add %app%.Infrastructure/%app%.Infrastructure.csproj package Microsoft.Extensions.Configuration.Abstractions

dotnet add %app%.Domain.Test/%app%.Domain.Test.csproj package Moq

rem Add Docker support
cd %app%.WebUi
dotnet new tool-manifest
dotnet tool install Microsoft.Web.LibraryManager.Cli
dotnet add package Docker.DotNet
dotnet add package Microsoft.VisualStudio.Azure.Containers.Tools.Targets
echo FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build > Dockerfile
echo WORKDIR /src >> Dockerfile
echo COPY ["%app%.WebUi.csproj", "."] >> Dockerfile
echo RUN dotnet restore "%app%.WebUi.csproj" >> Dockerfile
echo COPY . . >> Dockerfile
echo RUN dotnet build "%app%.WebUi.csproj" -c Release -o /app/build >> Dockerfile
echo FROM build AS publish >> Dockerfile
echo RUN dotnet publish "%app%.WebUi.csproj" -c Release -o /app/publish >> Dockerfile
echo FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final >> Dockerfile
echo WORKDIR /app >> Dockerfile
echo COPY --from=publish /app/publish . >> Dockerfile
echo ENTRYPOINT ["dotnet", "%app%.WebUi.dll"] >> Dockerfile
cd ..

@echo off
echo Update nuget packages
rem "https://www.reddit.com/r/dotnet/comments/1757s1o/upgrading_multiple_nuget_packages_in_jetbrains/?rdt=62769"
dotnet tool install --global dotnet-outdated-tool 
dotnet outdated --upgrade
:no