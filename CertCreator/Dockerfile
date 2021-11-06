FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CertCreator/CertCreator.csproj", "CertCreator/"]
RUN dotnet restore "CertCreator/CertCreator.csproj"
COPY . .
WORKDIR "/src/CertCreator"
RUN dotnet build "CertCreator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CertCreator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CertCreator.dll"]
