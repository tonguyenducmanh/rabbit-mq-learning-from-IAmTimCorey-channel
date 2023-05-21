# rabbit-mq-learning-from-IAmTimCorey-channel
học rabbit mq qua channel youtube IAmTimCorey

#các bước để chạy rabbitmq trong docker

1. mở docker
2. mở terminal, gõ lệnh: docker run -d --hostname rabbitmq --name rabbitmq-server -p 8080:15672 -p 5672:5672 rabbitmq:3-management
3. giải thích : 8080 là cổng port muốn truy cập vào trên docker, 15672 là port sender để gửi queue vào rabbitmq
4. mở trình duyệt gõ localhost:8080 là sẽ chạy được
5. tài khoản, mật khẩu đăng nhập mặc định là guest
