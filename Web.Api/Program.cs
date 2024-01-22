using System.Reflection;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web.Business.Cqrs;
using Web.Business.Mapper;
using Web.Business.Service;
using Web.Data.DbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
string connection = builder.Configuration.GetConnectionString("MsSqlConnection");
builder.Services.AddDbContext<TradeDbContext>(options => options.UseSqlServer(connection));

// Move the creation and registration of MapperConfiguration here
var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile(new MapperConfig()));
builder.Services.AddSingleton(mapperConfig.CreateMapper());

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(CreateBuyTradeCommand).GetTypeInfo().Assembly));
builder.Services.AddScoped<UpdateSharePrice>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.Run();