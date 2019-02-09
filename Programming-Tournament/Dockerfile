#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 63922
EXPOSE 44394

FROM microsoft/dotnet:2.1-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY ["Programming-Tournament/Programming-Tournament.csproj", "Programming-Tournament/"]
RUN dotnet restore "Programming-Tournament/Programming-Tournament.csproj"
COPY . .
WORKDIR "/src/Programming-Tournament"
RUN dotnet build "Programming-Tournament.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Programming-Tournament.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Programming-Tournament.dll"]