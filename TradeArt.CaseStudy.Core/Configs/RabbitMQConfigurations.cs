namespace TradeArt.CaseStudy.Core.Configs;

public class RabbitMQConnectionConfiguration {
	public string HostName { get; set; }
}

public class RabbitMQQueueConfiguration {
	public string IteratorQueue { get; set; }
}