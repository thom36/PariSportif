FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# copie solution + projets
COPY . .

# restore solution (IMPORTANT)
RUN dotnet restore PariSportifRepo.slnx

# publish projet principal
RUN dotnet publish PariSportif/PariSportif.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0

WORKDIR /app

COPY --from=build /app .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://0.0.0.0:8080

# IMPORTANT : DLL du publish
ENTRYPOINT ["dotnet", "PariSportif.dll"]