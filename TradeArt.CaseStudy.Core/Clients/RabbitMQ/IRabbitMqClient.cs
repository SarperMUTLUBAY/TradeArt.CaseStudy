namespace TradeArt.CaseStudy.Core.Clients.RabbitMQ;

public interface IRabbitMqClient {
	/// <summary>
	/// Send message to queue
	/// </summary>
	/// <param name="queue">Queue name for send to message</param>
	/// <param name="message">Message data</param>
	/// <typeparam name="T">Generic message type parameter.</typeparam>
	/// <returns>Returns true when the operation succeeds, false if it fails</returns>
	/// <exception cref="ArgumentNullException">Returns when queue name or message is null</exception>
	/// <exception cref="CaseStudyException">Returns when rabbitmq host name is null</exception>
	bool PublishToQueue<T>(string queue, T message);
	
	/// <summary>
	/// Send bulk message to queue
	/// </summary>
	/// <param name="queue">Queue name for send to messages</param>
	/// <param name="messages">Messages data list</param>
	/// <typeparam name="T">Generic message type parameter.</typeparam>
	/// <returns>Returns true when the operation succeeds, false if it fails</returns>
	/// <exception cref="ArgumentNullException">Returns when queue name null or messages is null or empty</exception>
	/// <exception cref="CaseStudyException">Returns when rabbitmq host name is null</exception>
	bool BulkPublishToQueue<T>(string queue, List<T> messages);
}