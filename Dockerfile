FROM mcr.microsoft.com/dotnet/runtime:3.1 AS base

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

WORKDIR /src
COPY Anki.csproj .
RUN dotnet restore "Anki.csproj"

COPY ./src ./src
RUN dotnet build "Anki.csproj" -c Release -o /app/build
# RUN dotnet publish Anki.sln -c Release -o /app/publish

FROM base AS final
WORKDIR /app
ENV LANG en_US.UTF-8
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "Anki.dll"]