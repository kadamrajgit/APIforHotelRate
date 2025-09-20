FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HotelAvailabilityApi/HotelAvailabilityApi.csproj", "HotelAvailabilityApi/"]
RUN dotnet restore "HotelAvailabilityApi/HotelAvailabilityApi.csproj"
COPY . .
WORKDIR "/src/HotelAvailabilityApi"
RUN dotnet build "HotelAvailabilityApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HotelAvailabilityApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY Data ./Data
ENTRYPOINT ["dotnet", "HotelAvailabilityApi.dll"]
