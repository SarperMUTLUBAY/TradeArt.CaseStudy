namespace TradeArt.CaseStudy.Core.Clients.RabbitMQ;

public interface IRabbitMqClient {
	bool PublishToQueue<T>(string queue, T message);
	bool BulkPublishToQueue<T>(string queue, List<T> messages);
}