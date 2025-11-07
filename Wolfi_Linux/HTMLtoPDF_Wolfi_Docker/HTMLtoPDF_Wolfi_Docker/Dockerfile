# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM cgr.dev/chainguard/aspnet-runtime:latest-dev AS base
USER root

RUN apk update && \
   apk upgrade && \
   apk add --update ca-certificates && \
   apk add chromium --update-cache --repository http://nl.alpinelinux.org/alpine/edge/community \
   rm -rf /var/cache/apk/*
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM cgr.dev/chainguard/dotnet-sdk:latest-dev AS build
USER root
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HTMLtoPDF_Wolfi_Docker.csproj", "."]
RUN dotnet restore "./HTMLtoPDF_Wolfi_Docker.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./HTMLtoPDF_Wolfi_Docker.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HTMLtoPDF_Wolfi_Docker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HTMLtoPDF_Wolfi_Docker.dll"]