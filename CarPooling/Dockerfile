FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["CarPooling/CarPooling.csproj", "CarPooling/"]
RUN dotnet restore "CarPooling/CarPooling.csproj"
COPY . .
WORKDIR "/src/CarPooling"
RUN dotnet build "CarPooling.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CarPooling.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CarPooling.dll"]