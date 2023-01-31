FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TradeArt.CaseStudy.Api/TradeArt.CaseStudy.Api.csproj", "TradeArt.CaseStudy.Api/"]
COPY ["TradeArt.CaseStudy.Business/TradeArt.CaseStudy.Business.csproj", "TradeArt.CaseStudy.Business/"]
COPY ["TradeArt.CaseStudy.Model/TradeArt.CaseStudy.Model.csproj", "TradeArt.CaseStudy.Model/"]
COPY ["TradeArt.CaseStudy.Common/TradeArt.CaseStudy.Common.csproj", "TradeArt.CaseStudy.Common/"]
RUN dotnet restore "TradeArt.CaseStudy.Api/TradeArt.CaseStudy.Api.csproj"
COPY . .
WORKDIR "/src/TradeArt.CaseStudy.Api"
RUN dotnet build "TradeArt.CaseStudy.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TradeArt.CaseStudy.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TradeArt.CaseStudy.Api.dll"]
