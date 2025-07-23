using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using SagaOrchestrator.Data;
using SagaOrchestrator.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<SagaDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IOrchestrator, Orchestrator>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddHttpClient("CatalogueMcService", client =>
{
    client.BaseAddress = new Uri(config["Services:Catalogue"]);
});

builder.Services.AddHttpClient("InventaireMcService", client =>
{
    client.BaseAddress = new Uri(config["Services:Inventaire"]);
});

builder.Services.AddHttpClient("VenteMcService", client =>
{
    client.BaseAddress = new Uri(config["Services:Vente"]);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Saga Orchestrator synchrone",
        Version = "v1",
        Description = "Un service d'orchestration de saga pour les tâches du domaine"
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SagaDbContext>();
    //db.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Saga Orchestrator");
    c.RoutePrefix = "swagger";
});

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.MapControllers();

app.Run();

public partial class Program { }
