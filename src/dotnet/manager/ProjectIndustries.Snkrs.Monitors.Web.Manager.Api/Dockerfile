FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS snkrs-dotnet-build-env
WORKDIR /snkrs
COPY ./src/dotnet ./
WORKDIR manager/ProjectIndustries.Snkrs.Monitors.Web.Manager.Api
RUN dotnet restore && dotnet publish -c Release -o ../../dist

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=snkrs-dotnet-build-env /snkrs/dist .
ENTRYPOINT ["dotnet", "ProjectIndustries.Snkrs.Monitors.Web.Manager.Api.dll"]
EXPOSE 443
EXPOSE 80
