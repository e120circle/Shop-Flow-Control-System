#FROM microsoft/aspnetcore
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base

# install System.Drawing native dependencies
RUN apt-get update \
    && apt-get install -y --allow-unauthenticated \
        libc6-dev \
        libgdiplus \
        libx11-dev \
     && rm -rf /var/lib/apt/lists/*
	 
WORKDIR /app
COPY . .
ENTRYPOINT ["dotnet", "MainForm.dll"]