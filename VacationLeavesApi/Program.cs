using Newtonsoft.Json.Serialization;
using VacationLeavesApi.Brokers;
using VacationLeavesApi.Concrete;
using VacationLeavesApi.Data;
using VacationLeavesApi.Interfaces;
using VacationLeavesApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson(opt =>
    {
        opt.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DnacloudDb202001benefitContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.CustomMiddlewareRegistering();
builder.Services.AddTransient<IPubSub, PubSub>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.ConfigureCustomeExceptionMiddleware();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
