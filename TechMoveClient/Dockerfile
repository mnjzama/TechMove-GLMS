# Author: PROG7311-2026-EMWVL (Lecturer Repository)
# URL: https://github.com/PROG7311-2026-EMWVL/Hello-PROG7311/tree/main/12%20-%20Docker%20Compose
# Date: [n.d]
# Date Accessed: 15 May 2026

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

USER root

RUN mkdir /keys && chown app:app /keys

RUN apt-get update && apt-get install -y libgssapi-krb5-2

USER app

WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY . .
RUN dotnet restore "./TechMoveClient.csproj"
RUN dotnet publish "./TechMoveClient.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TechMoveClient.dll"]