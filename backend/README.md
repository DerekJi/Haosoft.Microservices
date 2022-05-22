# Hulu Microservices Audit Logging

## Key Features

* CQRS (*Command Query Responsibility Segregation*) Pattern
* Publish-Subscribe Pattern for commands
* WebAPI for queries
* No authentication at the stage

## Infrastructures
* Database: Postgres with replication(s)
* Message Queue: RabbitMQ

## Components
* Abstractions: models & interfaces
* Repository & Migrations
* MQ Service for commands
* Http API for queries
* Unit Tests
* Integration Test Service

## Audit Message

* Time: 
* Level: INFO, DEBUG, ERROR
* Event: 
* Message: Human readable string
* Http Context: host, http method, path, remote address, user id …
* Platform Context: host, pod, container, namespace …
* Runtime Context: class, file, method, module …
* Business Context: 
* Custom Data: JSON formatted string
