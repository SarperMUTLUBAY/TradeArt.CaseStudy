using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TradeArt.CaseStudy.Consumer;
using TradeArt.CaseStudy.Consumer.Configs;

var host = Host.CreateDefaultBuilder(args)
			   .ConfigureServices((hostContext, services) => {
								      services.Configure<RabbitMQConfigurations>(hostContext.Configuration.GetSection("RabbitMQConfigurations"));
								      services.AddSingleton<IteratorConsumer>();
							      })
			   .Build();

var consumer = host.Services.GetRequiredService<IteratorConsumer>();
consumer.Consume();