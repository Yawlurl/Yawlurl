#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Src/YawlUrl.Web.Api/YawlUrl.Web.Api.csproj", "Src/YawlUrl.Web.Api/"]
COPY ["Src/YawlUrl.Application/YawlUrl.Application.csproj", "Src/YawlUrl.Application/"]
COPY ["Src/YawlUrl.Domain/YawlUrl.Domain.csproj", "Src/YawlUrl.Domain/"]
COPY ["Src/YawlUrl.Core/YawlUrl.Core.csproj", "Src/YawlUrl.Core/"]
COPY ["Src/YawlUrl.Common/YawlUrl.Common.csproj", "Src/YawlUrl.Common/"]
COPY ["Src/YawlUrl.Infrastructure.AspNetCore/YawlUrl.Infrastructure.AspNetCore.csproj", "Src/YawlUrl.Infrastructure.AspNetCore/"]
COPY ["Src/YawlUrl.Infrastructure.MongoDb/YawlUrl.Infrastructure.MongoDb.csproj", "Src/YawlUrl.Infrastructure.MongoDb/"]
COPY ["Src/YawlUrl.Infrastructure/YawlUrl.Infrastructure.csproj", "Src/YawlUrl.Infrastructure/"]
RUN dotnet restore "Src/YawlUrl.Web.Api/YawlUrl.Web.Api.csproj"
COPY . .
WORKDIR "/src/Src/YawlUrl.Web.Api"
RUN dotnet build "YawlUrl.Web.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "YawlUrl.Web.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "YawlUrl.Web.Api.dll"]