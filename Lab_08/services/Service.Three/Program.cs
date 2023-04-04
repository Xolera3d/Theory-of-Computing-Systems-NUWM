using Confluent.Kafka;
using GeoTiffCOG;
using KafkaFlow;
using KafkaFlow.Serializer;
using KafkaFlow.TypedHandler;
using Service.Three;
using AutoOffsetReset = KafkaFlow.AutoOffsetReset;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddKafkaFlowHostedService(builder => builder
            .UseConsoleLog()
            .AddCluster(cluster => cluster
                .OnStarted(_ => Console.WriteLine("Successfully connected to Kafka"))
                .WithBrokers(new[] { context.Configuration.GetConnectionString("Kafka") })
                .CreateTopicIfNotExists("field-processing", 1, 1) // temp
                .AddConsumer(consumer => consumer
                    .WithAutoOffsetReset(AutoOffsetReset.Earliest)
                    .Topic("field-processing")
                    .WithGroupId("group-three")
                    .WithBufferSize(100)
                    .WithWorkersCount(3) //todo read about
                    .WithConsumerConfig(new ConsumerConfig()
                    {
                        AllowAutoCreateTopics = true, // not work https://github.com/milvus-io/milvus/issues/17981
                    })
                    .AddMiddlewares(middlewares => middlewares
                        .AddSerializer<JsonCoreSerializer>()
                        .AddTypedHandlers(h => h
                            .WithHandlerLifetime(InstanceLifetime.Singleton)
                            .AddHandler<FieldProcessingHandler>())))));

        services.AddSingleton<GeoTiff>(x => new GeoTiff("./data/soil_moisture.tif"));
    })
    .Build();

host.Run();