FROM mcr.microsoft.com/dotnet/core/runtime:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk AS build
WORKDIR /src
COPY ["/MainForm.csproj", "MainForm/"]
RUN dotnet restore "MainForm/MainForm.csproj"
COPY . .
WORKDIR "/src/MainForm"
RUN dotnet build "MainForm.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MainForm.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MainForm.dll"]