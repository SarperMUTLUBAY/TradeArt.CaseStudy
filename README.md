# TradeArt.CaseStudy

[![Run Unit Tests](https://github.com/SarperMUTLUBAY/TradeArt.CaseStudy/actions/workflows/unit_tests.yml/badge.svg)](https://github.com/SarperMUTLUBAY/TradeArt.CaseStudy/actions/workflows/unit_tests.yml)

## Docker Commands

### Build:

```
docker build -t tradeart_case_study . -f TradeArt.CaseStudy.Api/Dockerfile
```
### Run with compose (replace $API_PORT with the value of your own, default port 8082):

```
docker-compose --env-file docker.env up -d
```

### Run without compose (replace $API_PORT with the value of your own):

#### We need a rabbitmq instance for async process (Case Study Task 2).

#### With network
```
docker network create tradeart-case-study-network
docker run --hostname rabbitmq --name rabbitmq --network tradeart-case-study-network -p 5672:5672 -p 15672:15672 -d rabbitmq:3-management-alpine
docker run --name=tradeart_case_study -p $API_PORT:80 --restart=always --network tradeart-case-study-network -e ASPNETCORE_ENVIRONMENT=Development -e RabbitMQConfigurations__Connection__HostName=rabbitmq -d tradeart_case_study
```

#### Without network
```
docker run --hostname rabbitmq --name rabbitmq -p 5672:5672 -p 15672:15672 -d rabbitmq:3-management-alpine
docker run --name=tradeart_case_study -p $API_PORT:80 --restart=always --link rabbitmq -e ASPNETCORE_ENVIRONMENT=Development -e RabbitMQConfigurations__Connection__HostName=rabbitmq -d tradeart_case_study
```

### Try (replace $API_PORT with the value from above):

```
API Swagger: http://localhost:$API_PORT/swagger

RabbitMQ Management Console Credentials: guest/guest
RabbitMQ Management Console: http://localhost:15672/#/
```


### Run Tests

```
dotnet test -c Release --logger "console;verbosity=detailed"
```