using Database.Migrations;
using Infrastructure;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddTicTacToeServices(builder.Configuration);

var app = builder.Build();
await app.Services.GetRequiredService<IMigrateDatabase>().Migrate();
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