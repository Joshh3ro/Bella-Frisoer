FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy only project files first to leverage docker layer caching
COPY ["BellaCuts.WebUi/BellaCuts.WebUi.csproj", "BellaCuts.WebUi/"]
COPY ["BellaCuts.Application/BellaCuts.Application.csproj", "BellaCuts.Application/"]
COPY ["BellaCuts.Domain/BellaCuts.Domain.csproj", "BellaCuts.Domain/"]
COPY ["BellaCuts.Infrastructure/BellaCuts.Infrastructure.csproj", "BellaCuts.Infrastructure/"]

RUN dotnet restore "BellaCuts.WebUi/BellaCuts.WebUi.csproj"

# Copy the rest of the sources
COPY . .

WORKDIR /src/BellaCuts.WebUi
RUN dotnet build "BellaCuts.WebUi.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR /src/BellaCuts.WebUi
RUN dotnet publish "BellaCuts.WebUi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BellaCuts.WebUi.dll"]
