using Confluent.Kafka;
using Gvz.Laboratory.PartyService;
using Gvz.Laboratory.PartyService.Abstractions;
using Gvz.Laboratory.PartyService.Kafka;
using Gvz.Laboratory.PartyService.Middleware;
using Gvz.Laboratory.PartyService.Repositories;
using Gvz.Laboratory.PartyService.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<GvzLaboratoryPartyServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var producerConfig = new ProducerConfig
{
    BootstrapServers = "kafka:29092"
};
builder.Services.AddSingleton<IProducer<Null, string>>(new ProducerBuilder<Null, string>(producerConfig).Build());
builder.Services.AddScoped<IPartyKafkaProducer, PartyKafkaProducer>();

var consumerConfig = new ConsumerConfig
{
    BootstrapServers = "kafka:29092",
    GroupId = "party-group-id",
    AutoOffsetReset = AutoOffsetReset.Earliest
};
builder.Services.AddSingleton(consumerConfig);

builder.Services.AddSingleton<AddManufacturerKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<AddManufacturerKafkaConsumer>());

builder.Services.AddSingleton<UpdateManufacturerKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<UpdateManufacturerKafkaConsumer>());

builder.Services.AddSingleton<DeleteManufacturerKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<DeleteManufacturerKafkaConsumer>());

builder.Services.AddSingleton<AddSupplierKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<AddSupplierKafkaConsumer>());

builder.Services.AddSingleton<UpdateSupplierKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<UpdateSupplierKafkaConsumer>());

builder.Services.AddSingleton<DeleteSupplierKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<DeleteSupplierKafkaConsumer>());

builder.Services.AddSingleton<AddProductKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<AddProductKafkaConsumer>());

builder.Services.AddSingleton<UpdateProductKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<UpdateProductKafkaConsumer>());

builder.Services.AddSingleton<DeleteProductKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<DeleteProductKafkaConsumer>());

builder.Services.AddSingleton<AddUserKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<AddUserKafkaConsumer>());

builder.Services.AddSingleton<UpdateUserKafkaConsumer>();
builder.Services.AddHostedService(provider => provider.GetRequiredService<UpdateUserKafkaConsumer>());

builder.Services.AddScoped<IManufacturerRepository, ManufacturerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISupplierRepository, SupplierRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IPartyService, PartyService>();
builder.Services.AddScoped<IPartyRepository, PartyRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
