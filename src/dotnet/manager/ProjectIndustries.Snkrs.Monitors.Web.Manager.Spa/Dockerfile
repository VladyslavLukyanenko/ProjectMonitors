# build backend
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS snkrs-dotnet-build-env
RUN pwd && ls -alh
WORKDIR /snkrs
COPY ./src ./

WORKDIR manager/ProjectIndustries.Snkrs.Monitors.Web.Manager.Spa
RUN dotnet restore && dotnet publish -c Release -o ../../dist


# build client app
FROM node:12.18.3 AS snkrs-angular-build-env
     
ENV NPM_CONFIG_LOGLEVEL warn
ARG app_env

WORKDIR /angular-tmp
COPY ./src/manager/ProjectIndustries.Snkrs.Monitors.Web.Manager.Spa/ClientApp .
RUN npm i && npm run build:prod

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine

WORKDIR /app
COPY --from=snkrs-dotnet-build-env /snkrs/dist .
COPY --from=snkrs-angular-build-env /angular-tmp/dist ./ClientApp/dist

ENTRYPOINT ["dotnet", "ProjectIndustries.Snkrs.Monitors.Web.Manager.Spa.dll"]
EXPOSE 443
EXPOSE 80
