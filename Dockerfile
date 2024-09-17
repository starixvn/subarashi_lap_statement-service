FROM hub.nexdev.net/base/dotnet/core/sdk:5.0.102-ca-patch-buster-slim AS builder

RUN apt-get update  \
  && curl --silent --location https://deb.nodesource.com/setup_14.x | bash -  \
  && apt-get install -y nodejs

RUN node -v
RUN npm -v

WORKDIR /src/Fanex.Athena.ExtService

COPY Fanex.Athena.ExtService/package.json ./
COPY Fanex.Athena.ExtService/package-lock.json ./

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
  --runtime alpine-x64 \
  --self-contained false

FROM hub.nexdev.net/base/dotnet/core/aspnet:5.0-alpine

ENV ASPNETCORE_URLS=http://*:5000
EXPOSE 5000

WORKDIR /app

COPY --from=builder /publish .

ENTRYPOINT ["dotnet", "Fanex.Athena.ExtService.dll"]
