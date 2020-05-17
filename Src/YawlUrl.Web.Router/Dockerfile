#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /router
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
COPY ["src/YawlUrl.Web.Router/YawlUrl.Web.Router.csproj", "src/YawlUrl.Web.Router/"]
COPY ["src/YawlUrl.Application/YawlUrl.Application.csproj", "src/YawlUrl.Application/"]
COPY ["src/YawlUrl.Domain/YawlUrl.Domain.csproj", "src/YawlUrl.Domain/"]
COPY ["src/YawlUrl.Core/YawlUrl.Core.csproj", "src/YawlUrl.Core/"]
COPY ["src/YawlUrl.Common/YawlUrl.Common.csproj", "src/YawlUrl.Common/"]
COPY ["src/YawlUrl.Infrastructure.AspNetCore/YawlUrl.Infrastructure.AspNetCore.csproj", "src/YawlUrl.Infrastructure.AspNetCore/"]
COPY ["src/YawlUrl.Infrastructure.MongoDb/YawlUrl.Infrastructure.MongoDb.csproj", "src/YawlUrl.Infrastructure.MongoDb/"]
COPY ["src/YawlUrl.Infrastructure/YawlUrl.Infrastructure.csproj", "src/YawlUrl.Infrastructure/"]
RUN dotnet restore "src/YawlUrl.Web.Router/YawlUrl.Web.Router.csproj"
COPY . .
WORKDIR "/src/YawlUrl.Web.Router"
RUN dotnet build "YawlUrl.Web.Router.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YawlUrl.Web.Router.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YawlUrl.Web.Router.dll"]