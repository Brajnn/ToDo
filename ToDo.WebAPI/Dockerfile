FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app


COPY *.sln .
COPY ToDo.Application/*.csproj ./ToDo.Application/
COPY ToDo.Infrastructure/*.csproj ./ToDo.Infrastructure/
COPY ToDo.Domain/*.csproj ./ToDo.Domain/
COPY ToDo.WebAPI/*.csproj ./ToDo.WebAPI/
COPY ToDo.Tests/*.csproj ./ToDo.Tests/
RUN dotnet restore

# Kopiowanie reszty plików i publikowanie aplikacji
COPY . .
WORKDIR /app/ToDo.WebAPI
RUN dotnet publish -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "ToDo.WebAPI.dll"]