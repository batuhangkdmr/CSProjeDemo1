# 1. Build aşaması
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app

COPY *.sln ./
COPY CSProjeDemo1.*/*.csproj ./
RUN dotnet restore

COPY . ./
WORKDIR /app/CSProjeDemo1.Web
RUN dotnet publish -c Release -o /out

# 2. Run aşaması
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /out .

ENTRYPOINT ["dotnet", "CSProjeDemo1.Web.dll"]
