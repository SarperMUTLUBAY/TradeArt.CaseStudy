FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TradeArt.CaseStudy.Consumer/TradeArt.CaseStudy.Consumer.csproj", "TradeArt.CaseStudy.Consumer/"]
RUN dotnet restore "TradeArt.CaseStudy.Consumer/TradeArt.CaseStudy.Consumer.csproj"
COPY . .
WORKDIR "/src/TradeArt.CaseStudy.Consumer"
RUN dotnet build "TradeArt.CaseStudy.Consumer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TradeArt.CaseStudy.Consumer.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TradeArt.CaseStudy.Consumer.dll"]
