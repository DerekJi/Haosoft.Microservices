## Start RabbitMQ
```bash
docker-compose up -d
```

## Generate password hash for RabbitMQ
```bash
./encode_password.sh password
```

## Reference
* https://devops.datenkollektiv.de/creating-a-custom-rabbitmq-container-with-preconfigured-queues.html
* https://artofcode.wordpress.com/2022/03/28/how-to-run-rabbitmq-with-a-predefined-queue-using-docker-compose/