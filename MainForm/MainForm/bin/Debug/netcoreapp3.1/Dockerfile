#FROM microsoft/aspnetcore
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS base
WORKDIR /app
COPY . .
ENTRYPOINT ["dotnet", "MainForm.dll"]