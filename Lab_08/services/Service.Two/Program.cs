using Confluent.Kafka;
using KafkaFlow;
using KafkaFlow.Serializer;
using KafkaFlow.TypedHandler;
using Microsoft.EntityFrameworkCore;
using Service.Common;
using Service.Two;
using AutoOffsetReset = KafkaFlow.AutoOffsetReset;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddDbContext<AppDbContext>(
            optionsBuilder =>
                optionsBuilder.UseNpgsql(context.Configuration.GetConnectionString("Postgres"),
                    o => o.UseNetTopologySuite()));
        
        const string topicName = "field-reading";
        const string reduceTopicName = "field-processing";
        
        services.AddKafkaFlowHostedService(builder => builder
            .UseConsoleLog()
            .AddCluster(cluster => cluster
                .OnStarted(_ => Console.WriteLine("Successfully connected to Kafka"))
                .WithBrokers(new[] { context.Configuration.GetConnectionString("Kafka") })
                .CreateTopicIfNotExists(topicName, 1, 1)
                .AddProducer(
                    "main",
                    producer => producer
                        .DefaultTopic(reduceTopicName)
                        .AddMiddlewares(m => m.AddSerializer<JsonCoreSerializer>())
                )
                .AddConsumer(consumer => consumer
                    .WithAutoOffsetReset(AutoOffsetReset.Earliest)
                    .Topic(topicName)
                    .WithGroupId("group-two")
                    .WithBufferSize(100)
                    .WithWorkersCount(3) //todo read about
                    .WithConsumerConfig(new ConsumerConfig()
                    {
                        AllowAutoCreateTopics = true, // not work https://github.com/milvus-io/milvus/issues/17981
                    })
                    .AddMiddlewares(middlewares => middlewares
                        .AddSerializer<JsonCoreSerializer>()
                        .AddTypedHandlers(h => h
                            .WithHandlerLifetime(InstanceLifetime.Transient)
                            .AddHandler<FieldReadingHandler>())))));
    })
    .Build();

host.Run();