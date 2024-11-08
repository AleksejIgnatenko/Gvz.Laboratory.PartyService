using Confluent.Kafka;
using Gvz.Laboratory.PartyService;
using Gvz.Laboratory.PartyService.Abstractions;
using Gvz.Laboratory.PartyService.Kafka;
using Gvz.Laboratory.PartyService.Repositories;
using Gvz.Laboratory.PartyService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
