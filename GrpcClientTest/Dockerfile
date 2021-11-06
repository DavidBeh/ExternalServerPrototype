FROM mcr.microsoft.com/dotnet/runtime:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["GrpcClientTest/GrpcClientTest.csproj", "GrpcClientTest/"]
RUN dotnet restore "GrpcClientTest/GrpcClientTest.csproj"
COPY . .
WORKDIR "/src/GrpcClientTest"
RUN dotnet build "GrpcClientTest.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GrpcClientTest.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrpcClientTest.dll"]
