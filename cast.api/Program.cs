using System.Text.Json.Serialization;
using cast.api.core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddSingleton<CompilationService>();

// Nur für Minimal APIs konfigurieren
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        // Erlaube dein Frontend (am besten 127.0.0.1 UND localhost angeben)
        policy.WithOrigins("http://127.0.0.1:3000", "http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod(); 
    });
});

var app = builder.Build();
app.UseCors("frontend");
app.MapPost("/api/compile", ([FromBody]CompilationRequest request, CompilationService service) =>
{
    Console.WriteLine(request.Input);
    return service.Compile(request.Input);
});

app.Run();

[JsonSerializable(typeof(CompilationResult))]
[JsonSerializable(typeof(CompilationRequest))]
// WICHTIG: Wenn du einen Request-Body hast (z.B. ein CompilationRequest-Objekt), 
// musst du das hier auch als [JsonSerializable(typeof(DeinRequest))] hinzufügen!
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}