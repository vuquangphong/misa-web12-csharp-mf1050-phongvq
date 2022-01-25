using MISA.Fresher.Web12.Core.Interfaces.Infrastructure;
using MISA.Fresher.Web12.Core.Interfaces.Services;
using MISA.Fresher.Web12.Core.Services;
using MISA.Fresher.Web12.Infrastructure.Repositories;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Dependendcy Injection (19/01/2022)
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<IEmployeeServices, EmployeeServices>();
builder.Services.AddScoped<IDepartmentServices, DepartmentServices>();
builder.Services.AddScoped<IPositionServices, PositionServices>();

// CORS Lisence (24/01/2022)
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
});

// Newtonsoft JSON (25/01/2022)
builder.Services.AddMvc()
        .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

// CORS Lisence
app.UseCors(options => options.AllowAnyOrigin());

app.Run();

