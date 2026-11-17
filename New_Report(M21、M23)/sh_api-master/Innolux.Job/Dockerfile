#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Innolux.Job/Innolux.Job.csproj", "Innolux.Job/"]
RUN dotnet restore "Innolux.Job/Innolux.Job.csproj"
COPY . .
WORKDIR "/src/Innolux.Job"
RUN dotnet build "Innolux.Job.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Innolux.Job.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Innolux.Job.dll"]