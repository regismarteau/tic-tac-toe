# Infrastructure
A project in charge of binding every layer of the application by configuring for example the dependencies injection.
This is here where are defined all of the technical implementations such as repositories or dispatchers.

These services remain critical for the good health of the application, and they are as much as possible tested indirectly trought Acceptance tests.

## Outbox pattern
To deal with asynchronous process and side effects, a home-made outbox pattern is provide here to ensure eventual consistency. It doesn't pretend to be prod-ready. I would rather recommend to use a more dedicated and reliable tool to deal with this, such as a message broker like `RabbitMq` ou `Kafka` if necessary.
Also, [MassTransit](https://github.com/MassTransit/MassTransit) is a pretty good plug-and-play tool to interface with those services, and offer good Outbox pattern possibilities.