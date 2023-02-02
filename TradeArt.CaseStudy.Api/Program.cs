using TradeArt.CaseStudy.Core.Clients.RabbitMQ;
using TradeArt.CaseStudy.Core.Configs;
using TradeArt.CaseStudy.Facade.Implementations;
using TradeArt.CaseStudy.Facade.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RabbitMQConnectionConfiguration>(builder.Configuration.GetSection("RabbitMQConfigurations:Connection"));
builder.Services.Configure<RabbitMQQueueConfiguration>(builder.Configuration.GetSection("RabbitMQConfigurations:Queues"));

// Add services to the container.
builder.Services.AddScoped<ICaseStudyFacade, CaseStudyFacade>();
builder.Services.AddScoped<IRabbitMqClient, RabbitMqClient>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();