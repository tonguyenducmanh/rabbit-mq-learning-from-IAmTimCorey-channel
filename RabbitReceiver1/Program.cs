using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

//ứng dụng receiver thứ nhất ( vào trong rabit mq đọc message)
//created by tdmanh1 21/05/2023
//test tạo ra ứng dụng gửi và nhận message cho rabbitmq

int _timeWait = 2;

// tạo ra factory
ConnectionFactory factory = new ConnectionFactory();
// khai báo tài khoản, mật khẩu và địa chỉ của rabbitmq ( được tạo ra theo file readme.md )
factory.Uri = new Uri("amqp://guest:guest@localhost:5672");
// khai báo tên dùng trong application console này
// vì sau có thể có nhiều app sender dùng tạo queue, dùng để phân biệt
factory.ClientProvidedName = "Rabbit Receiver1 App";

// tạo kết nối
IConnection cnn = factory.CreateConnection();

// tạo ra model
IModel channel = cnn.CreateModel();

// exchange, channel, queue là 3 tab ở trong localhost:8080 (rabbitmq) sau khi đăng nhập thành công

// cấu hình 3 trường bên dưới phải giống bên project RabbitSender
string exchangeName = "manh-DemoExchange";
string routingKey = "manh-demo-routing-key";
string queueName = "manh-DemoQueue";

// gán 3 biến trên vào chanel object
channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
channel.QueueDeclare(queueName, false, false, false, null);
channel.QueueBind(queueName, exchangeName, routingKey, null);

// thiết lập Quality of Services
// prefetchSize = 0 : không quan tâm size của message
// prefetchCount = 1 : chỉ muốn nhận 1 message 1 thời điểm
// global : chỉ áp dụng cho instance hiện tại
channel.BasicQos(0,1,false);
Console.OutputEncoding = Encoding.UTF8;
var consumer  = new EventingBasicConsumer(channel);

// nhận từng message về 1 lần
consumer.Received += (sender, args) =>
{
    // dừng 2 giây giả lập tải message từ internet về
    Task.Delay(TimeSpan.FromSeconds(_timeWait)).Wait(); 

    var body = args.Body.ToArray();

    // biến các bytes[] được tạo ra từ message bên sender thành string đọc được
    string message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Message nhận được từ Rabbit MQ: {message}");

    // loại trừ delivery tag
    channel.BasicAck(args.DeliveryTag, false);
};

string consumerTag = channel.BasicConsume(queueName, false, consumer);


// phải có dòng này vì là console app, không có console app mặc định chạy xong sẽ đóng
// thêm vào để enter để đóng
Console.ReadLine();

// xóa message đã đọc đi
channel.BasicCancel(consumerTag);

channel.Close();
cnn.Close();