using RabbitMQ.Client;
using System.Runtime.CompilerServices;
using System.Text;

//ứng dụng sender ( gửi message vào trong rabit mq )
//created by tdmanh1 21/05/2023
//test tạo ra ứng dụng gửi và nhận message cho rabbitmq

int _countSending = 30;
int _timeWait = 1000;
// tạo ra factory
ConnectionFactory factory  = new ConnectionFactory();
// khai báo tài khoản, mật khẩu và địa chỉ của rabbitmq ( được tạo ra theo file readme.md )
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
// khai báo tên dùng trong application console này
// vì sau có thể có nhiều app sender dùng tạo queue, dùng để phân biệt
factory.ClientProvidedName = "Rabbit Sender App";

// tạo kết nối
IConnection cnn = factory.CreateConnection();

// tạo ra model
IModel channel =  cnn.CreateModel();

// exchange, channel, queue là 3 tab ở trong localhost:8080 (rabbitmq) sau khi đăng nhập thành công
string exchangeName = "manh-DemoExchange";
string routingKey = "manh-demo-routing-key";
string queueName = "manh-DemoQueue";

// gán 3 biến trên vào chanel object
channel.ExchangeDeclare(exchangeName, ExchangeType.Direct); 
channel.QueueDeclare(queueName, false, false, false,null);
channel.QueueBind(queueName, exchangeName, routingKey, null);

// tạo ra vòng lặp gửi lên rabbit mq giả lập gửi đi nhiều lần
for(int i = 0;i < _countSending; i++)
{
    // gửi message vào queue, phải biến nó thành kiểu byte
    byte[] messageBodyBytes = Encoding.UTF8.GetBytes($"Hello World from Rabbit MQ lần {i +1}, thời gian gửi {DateTime.Now}");

    // đẩy 1 message lên channel này
    channel.BasicPublish(exchangeName, routingKey, null, messageBodyBytes);
    // dừng 2 giây giả lập tải message lên
    Thread.Sleep(_timeWait);
}

// đóng kết nối sau khi đẩy 1 message lên thành công
channel.Close();