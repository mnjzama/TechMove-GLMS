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
WORKDIR /src
COPY . .

RUN dotnet restore "./TechMoveAPI.csproj"
RUN dotnet publish "./TechMoveAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TechMoveAPI.dll"]