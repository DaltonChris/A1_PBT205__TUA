using Moq;
using PBT_205_A1;
using RabbitMQ.Client;

[TestClass]
public class ChatFormTests
{
    private Mock<IConnection> mockConnection;
    private Mock<IModel> mockChannel;
    private Mock<IConnectionFactory> mockConnectionFactory;

    private readonly string _HostName = "localhost";
    private readonly string _RoutingKey = "chat_room";
    private readonly string _ExchangeName = "chat_exchange";

    [TestInitialize]
    public void Setup()
    {
        mockConnection = new Mock<IConnection>();
        mockChannel = new Mock<IModel>();
        mockConnectionFactory = new Mock<IConnectionFactory>();

        mockConnectionFactory.Setup(f => f.CreateConnection()).Returns(mockConnection.Object);
        mockConnection.Setup(c => c.CreateModel()).Returns(mockChannel.Object);
    }

}
