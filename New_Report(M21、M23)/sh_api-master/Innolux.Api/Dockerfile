# #See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
#ENV TZ=Asia/Shanghai
#ENV LD_LIBRARY_PATH="/app/clidriver/lib"
#WORKDIR /app
#COPY ./sources.list /etc/apt/
#RUN apt-get -y update && apt-get install -y libxml2
#EXPOSE 80

FROM reportbase:v3.0 AS build
#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Innolux.Api/Innolux.Api.csproj", "Innolux.Api/"]
COPY ["Innolux.Application/Innolux.Application.csproj", "Innolux.Application/"]
COPY ["Innolux.Core/Innolux.Core.csproj", "Innolux.Core/"]
COPY ["Innolux.EFCore/Innolux.EFCore.csproj", "Innolux.EFCore/"]
COPY ["Innolux.Biz/Innolux.Biz.csproj", "Innolux.Biz/"]
RUN dotnet restore --source /root/.nuget/packages/ "Innolux.Api/Innolux.Api.csproj"
#RUN dotnet restore  "Innolux.Api/Innolux.Api.csproj"
COPY . .
WORKDIR "/src/Innolux.Api"
RUN dotnet build --source /root/.nuget/packages/ "Innolux.Api.csproj" -c Release -o /app/build
#RUN dotnet build  "Innolux.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish --source /root/.nuget/packages/ "Innolux.Api.csproj" -c Release -o /app/publish
#RUN dotnet publish "Innolux.Api.csproj" -c Release -o /app/publish

#FROM base AS final
FROM aspnet:6.0-1 AS final
WORKDIR /app
COPY --from=publish /app/publish .
Env PATH=$PATH:"/app/bin/x64/Release/net6.0/clidriver/bin:/app/bin/x64/Release/net6.0/clidriver/lib"
## ENV PATH=$PATH:"/app/clidriver/bin:/app/clidriver/lib"

ENTRYPOINT ["dotnet", "Innolux.Api.dll"]

###################################################################################################
# #See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
# #For .NET 5.0
# FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
# ##For .NET Core 3.1
# #FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base

# WORKDIR /app

# #incase of any library loading issues, enabled the next line to collect more information
# #ENV LD_DEBUG=bindings:symbols

# #Make sure the path is set to location where clidriver is present. For .NET Core, the path will vary
# ENV LD_LIBRARY_PATH="/app/clidriver/lib"
# #ENV LD_LIBRARY_PATH="/app/bin/x64/Debug/net5.0/clidriver/lib"

# #install the dependency library libxml2
# RUN apt-get -y update && apt-get install -y libxml2
# #On some systems as found out by Jason McCombie, additional dependency installation may be needed as described in below links
# #https://www.ibm.com/docs/en/db2/11.5?topic=running-restrictions-linking-libdb2so
# #https://www.ibm.com/docs/en/opw/8.0.0?topic=solutions-libdb2so-cannot-be-loaded
# # apt-get install -y pam x86_64
# # apt-get install -y pam.i686
# #On Red Hat
# #yum install -y pam x86_64
# #yum install -y pam.i686

# #For .NET Core 3.1 it will be FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
# FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
# WORKDIR /src

# # COPY . ./
# # RUN dotnet restore "./DockerTest.csproj"
# # RUN dotnet restore --source /root/.nuget/packages/ "Innolux.Api/Innolux.Api.csproj"
# RUN dotnet restore  "Innolux.Api/Innolux.Api.csproj"

# # RUN dotnet build "DockerTest.csproj" -c Release -o /app/build
# #RUN dotnet build "DockerTest.csproj" -c Debug -o /app/build
# # RUN dotnet build --source /root/.nuget/packages/ "Innolux.Api.csproj" -c Release -o /app/build
# RUN dotnet build  "Innolux.Api.csproj" -c Release -o /app/build

# FROM build AS publish
# # RUN dotnet publish "DockerTest.csproj" -c Release -o /app/publish
# #RUN dotnet publish "DockerTest.csproj" -c Debug -o /app/publish
# # RUN dotnet publish --source /root/.nuget/packages/ "Innolux.Api.csproj" -c Release -o /app/publish
# RUN dotnet publish "Innolux.Api.csproj" -c Release -o /app/publish

# ##For applications targeting IBM database servers i.e. Db2 for z/OS or Db2 for i, a Db2 Connect license is needed.
# ##There are two licensing options available:
# ##1. Server based license -
# ##The easiest and recommended way is to use the server based licensing of Db2 Connect where the
# ## license is installed in the Db2 for z/OS or Db2 for i database servers that allows unlimited number of clients/applications/users to connect.
# ##2. Client/ user-based license -
# ##User-based licensing of Db2 Connect requires the license file to be copied explicitly to clidriver/license folder
# ##NuGet cache is one of the locations to keep the license file.
# ##Application output directory is another location but each time the application is built, the file may have to copied explicitly.
# ##Adding the license file as part of the project and copying it to <output directory>/clidriver/license/ as part of build is also an option.
# ##Note: db2consv_ee.lic is the license file for the user based Db2 Connect Enterprise Edition.
# # COPY ./db2consv_ee.lic /app/publish/clidriver/license/db2consv_ee.lic

# FROM base AS final
# WORKDIR /app

# COPY --from=publish /app/publish .

# Env PATH=$PATH:"/app/bin/x64/Release/net5.0/clidriver/bin:/app/bin/x64/Release/net5.0/clidriver/lib"
# #Env PATH=$PATH:"/app/bin/x64/Debug/net5.0/clidriver/bin:/app/bin/x64/Debug/net5.0/clidriver/lib"

# ENTRYPOINT ["dotnet", "Innolux.Api.dll"]
