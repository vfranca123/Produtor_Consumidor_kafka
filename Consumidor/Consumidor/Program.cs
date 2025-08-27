using Confluent.Kafka;
using Consumidor.config;
using Consumidor.Model;
using Consumidor.Services;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<ConsumerConfig>(ConsumerConfiguration.getConsumerCofig());
builder.Services.AddSingleton<ConsumerService>();
builder.Services.AddHttpClient();
//SWAGGER
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
