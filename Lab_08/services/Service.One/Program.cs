using KafkaFlow;
using KafkaFlow.Serializer;
using Microsoft.EntityFrameworkCore;
using Service.Common;
using Service.One;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(
    optionsBuilder =>
        optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"),
            o => o.UseNetTopologySuite()));

const string topicName = "field-reading";

builder.Services.AddKafka(kafka => kafka
    .UseMicrosoftLog()
    .AddCluster(cluster => cluster
        .OnStarted(_ => Console.WriteLine("Successfully connected to Kafka"))
        .WithBrokers(new[] { builder.Configuration.GetConnectionString("Kafka") })
        .AddProducer("main", producer => producer
            .DefaultTopic(topicName)
            .AddMiddlewares(m =>
                m.AddSerializer<JsonCoreSerializer>()))));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

var kafkaBus = app.Services.CreateKafkaBus();
await kafkaBus.StartAsync();

DatabaseInitializer.Seed(app.Services);

app.Run();