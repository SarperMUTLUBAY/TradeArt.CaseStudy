using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using TradeArt.CaseStudy.Core.Clients.RabbitMQ;
using TradeArt.CaseStudy.Core.Configs;
using TradeArt.CaseStudy.Facade.Implementations;
using TradeArt.CaseStudy.Facade.Interfaces;
using TradeArt.CaseStudy.Integration.GraphQL.BlockTap;

var builder = WebApplication.CreateBuilder(args);

// Configurations
builder.Services.Configure<RabbitMQConnectionConfiguration>(builder.Configuration.GetSection("RabbitMQConfigurations:Connection"));
builder.Services.Configure<RabbitMQQueueConfiguration>(builder.Configuration.GetSection("RabbitMQConfigurations:Queues"));

// Add services to the container.
builder.Services.AddScoped<IGraphQLClient>(_ => new GraphQLHttpClient(builder.Configuration.GetValue<string>("GraphQLConfigurations:Url"), new SystemTextJsonSerializer()));

builder.Services.AddScoped<ICaseStudyFacade, CaseStudyFacade>();

// Clients
builder.Services.AddScoped<IRabbitMqClient, RabbitMqClient>();

// Integrations
builder.Services.AddScoped<IBlockTapGraphQLIntegration, BlockTapGraphQLIntegration>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();