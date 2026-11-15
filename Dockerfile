FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY SumaqAgro.Platform/SumaqAgro.Platform.csproj SumaqAgro.Platform/
RUN dotnet restore SumaqAgro.Platform/SumaqAgro.Platform.csproj
COPY SumaqAgro.Platform/ SumaqAgro.Platform/
RUN dotnet publish SumaqAgro.Platform/SumaqAgro.Platform.csproj -c Release --no-restore -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0
RUN apt-get update && apt-get install -y --no-install-recommends fonts-dejavu-core && rm -rf /var/lib/apt/lists/*
WORKDIR /app
COPY --from=build /app .
USER $APP_UID
EXPOSE 8080
CMD ["sh", "-c", "exec dotnet SumaqAgro.Platform.dll --urls http://0.0.0.0:${PORT:-8080}"]
