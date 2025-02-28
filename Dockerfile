ARG CI_REGISTRY_IMAGE

FROM $CI_REGISTRY_IMAGE:builder as deps

RUN mkdir -p /src/packages

FROM hub.nexdev.net/base/dotnet/core/sdk:8.0-nodejs-v20 AS builder

ENV NUGET_PACKAGES=/src/packages

COPY --from=deps $NUGET_PACKAGES/. $NUGET_PACKAGES/

WORKDIR /src/Fanex.Athena.ExtService

COPY Fanex.Athena.ExtService/package.json ./
COPY Fanex.Athena.ExtService/package-lock.json ./

RUN node -v
RUN npm -v
RUN npm install

WORKDIR /src

COPY NuGet.Config .
RUN dotnet nuget add source http://nuget.nexdev.net/api/v2

COPY Fanex.Athena.ExtService.Resources/Fanex.Athena.ExtService.Resources.csproj ./
RUN dotnet restore Fanex.Athena.ExtService.Resources.csproj -v n

COPY Fanex.Athena.ExtService.Core/Fanex.Athena.ExtService.Core.csproj ./
RUN dotnet restore Fanex.Athena.ExtService.Core.csproj -v n

COPY Fanex.Athena.ExtService.Common/Fanex.Athena.ExtService.Common.csproj ./
RUN dotnet restore Fanex.Athena.ExtService.Common.csproj -v n

COPY Fanex.Athena.ExtService.BetList/Fanex.Athena.ExtService.BetList.csproj ./
RUN dotnet restore Fanex.Athena.ExtService.BetList.csproj -v n

COPY Fanex.Athena.ExtService.Report/Fanex.Athena.ExtService.Report.csproj ./
RUN dotnet restore Fanex.Athena.ExtService.Report.csproj -v n

COPY Fanex.Athena.ExtService/Fanex.Athena.ExtService.csproj ./
RUN dotnet restore Fanex.Athena.ExtService.csproj -v n

COPY Fanex.Athena.ExtService.Resources ./Fanex.Athena.ExtService.Resources
COPY Fanex.Athena.ExtService.Core ./Fanex.Athena.ExtService.Core
COPY Fanex.Athena.ExtService.Common ./Fanex.Athena.ExtService.Common
COPY Fanex.Athena.ExtService.BetList ./Fanex.Athena.ExtService.BetList
COPY Fanex.Athena.ExtService.Report ./Fanex.Athena.ExtService.Report
COPY Fanex.Athena.ExtService ./Fanex.Athena.ExtService

WORKDIR /src/Fanex.Athena.ExtService

RUN npm run build-pro

RUN dotnet publish "Fanex.Athena.ExtService.csproj" -c Release -o /publish \
  --runtime linux-x64 \
  --self-contained false

FROM hub.nexdev.net/base/dotnet/core/aspnet:8.0-alpine

ENV ASPNETCORE_URLS=http://*:5000
ENV DOTNET_HOSTBUILDER__RELOADCONFIGONCHANGE=false
EXPOSE 5000

WORKDIR /app

COPY --from=builder /publish .

ENTRYPOINT ["dotnet", "Fanex.Athena.ExtService.dll"]
