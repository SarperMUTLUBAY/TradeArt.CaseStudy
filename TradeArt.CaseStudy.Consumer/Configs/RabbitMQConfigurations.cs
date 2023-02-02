namespace TradeArt.CaseStudy.Consumer.Configs;

public class RabbitMQConfigurations {
	public RabbitMQConnectionConfiguration Connection { get; set; }
	public RabbitMQQueueConfiguration Queues { get; set; }
}

public class RabbitMQConnectionConfiguration {
	public string HostName { get; set; }
}

public class RabbitMQQueueConfiguration {
	public string IteratorQueue { get; set; }
}