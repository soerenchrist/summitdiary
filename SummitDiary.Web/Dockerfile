FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["SummitDiary.Web/SummitDiary.Web.csproj", "SummitDiary.Web/"]
RUN dotnet restore "SummitDiary.Web/SummitDiary.Web.csproj"
COPY . .
WORKDIR "/src/SummitDiary.Web"
RUN dotnet build "SummitDiary.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SummitDiary.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SummitDiary.Web.dll"]
