FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app
COPY ./ ./

COPY Core/Core.csproj /Core/
COPY Infra/Infra.csproj /Infra/
COPY Presentation/Presentation.csproj /Presentation/

RUN dotnet restore /Presentation/Presentation.csproj

RUN dotnet build Presentation/Presentation.csproj -c Release -o /app/build

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0    
WORKDIR /app
EXPOSE 80
COPY --from=build-env /app/build .
ENTRYPOINT ["dotnet", "Presentation.dll"]
