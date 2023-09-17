FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG TARGETARCH
WORKDIR /source

# Copy and build/publish
COPY Src/Domain/. ./Domain
COPY Src/Application/. ./Application
RUN dotnet publish ./Application/Application.csproj -a $TARGETARCH -o /app

# Final image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
ENTRYPOINT ["./Application"]
