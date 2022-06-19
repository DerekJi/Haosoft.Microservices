#!/bin/sh

## Set aliases
alias rabbitmqctl="docker exec rabbitmq rabbitmqctl"

## Startup
docker-compose up -d
