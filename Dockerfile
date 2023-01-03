FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CurvesWebEditor/CurvesWebEditor.csproj", "CurvesWebEditor/"]
COPY ["Curves/Curves.csproj", "Curves/"]
COPY ["TransformStructures/TransformStructures.csproj", "TransformStructures/"]
RUN dotnet restore "CurvesWebEditor/CurvesWebEditor.csproj"
COPY . .
WORKDIR "/src/CurvesWebEditor"
RUN dotnet build "CurvesWebEditor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CurvesWebEditor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CurvesWebEditor.dll"]
