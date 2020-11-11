FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 as runtime
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as builder

EXPOSE 5000
WORKDIR /src
COPY src/ProductmanagementCore.csproj .
RUN dotnet restore

COPY src .
RUN dotnet build -c Release -o /app

FROM builder AS publish
RUN dotnet publish -c Release -o /app

FROM runtime as final

WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ProductmanagementCore.dll"]