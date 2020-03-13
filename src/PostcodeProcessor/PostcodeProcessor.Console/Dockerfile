#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
 # Install Chrome
 RUN apt-get update && apt-get install -y \
 apt-transport-https \
 ca-certificates \
 curl \
 gnupg \
 hicolor-icon-theme \
 libcanberra-gtk* \
 libgl1-mesa-dri \
 libgl1-mesa-glx \
 libpango1.0-0 \
 libpulse0 \
 libv4l-0 \
 fonts-symbola \
 --no-install-recommends \
 && curl -sSL https://dl.google.com/linux/linux_signing_key.pub | apt-key add - \
 && echo "deb [arch=amd64] https://dl.google.com/linux/chrome/deb/ stable main" > /etc/apt/sources.list.d/google.list \
 && apt-get update && apt-get install -y \
 google-chrome-stable \
 --no-install-recommends \
 && apt-get purge --auto-remove -y curl \
 && rm -rf /var/lib/apt/lists/*

WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["PostcodeProcessor/PostcodeProcessor.Console/PostcodeProcessor.Console.csproj", "PostcodeProcessor/PostcodeProcessor.Console/"]
COPY ["PostcodeProcessor/PostcodeProcessor.Core/PostcodeProcessor.Core.csproj", "PostcodeProcessor/PostcodeProcessor.Core/"]
COPY ["PostcodeProcessor/PostcodeProcessor.Infrastructure/PostcodeProcessor.Infrastructure.csproj", "PostcodeProcessor/PostcodeProcessor.Infrastructure/"]
RUN dotnet restore "PostcodeProcessor/PostcodeProcessor.Console/PostcodeProcessor.Console.csproj"
COPY . .
WORKDIR "/src/PostcodeProcessor/PostcodeProcessor.Console"
RUN dotnet build "PostcodeProcessor.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PostcodeProcessor.Console.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 9515
ENTRYPOINT ["dotnet", "RightmovePostcodeToLocationId.PostcodeProcessor.Console.dll"]