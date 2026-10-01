using System.Text.Json.Serialization;
using Database.Migrations;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddTicTacToeServices(builder.Configuration);

var app = builder.Build();
await app.Services.GetRequiredService<IMigrateDatabase>().Migrate(app.Lifetime.ApplicationStopping);
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseExceptionHandler("/error");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }
