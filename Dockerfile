# 1. Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копіюємо csproj і відновлюємо залежності
COPY *.sln ./
COPY DivinePromo/*.csproj ./DivinePromo/
RUN dotnet restore

# Копіюємо решту файлів і збираємо
COPY DivinePromo/. ./DivinePromo/
WORKDIR /src/DivinePromo
RUN dotnet publish -c Release -o /app/publish

# 2. Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish ./

# Render передає порт у змінній PORT
ENV ASPNETCORE_URLS=http://0.0.0.0:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "DivinePromo.dll"]
