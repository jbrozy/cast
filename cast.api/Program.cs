using System.Text.Json;
using System.Text.Json.Serialization;
using cast.api.core;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateSlimBuilder(args);

builder.Services.AddSingleton<CompilationService>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:3000", "http://localhost:3000", "http://87.106.28.73:3000")
            .AllowAnyHeader()
            .AllowAnyMethod(); 
    });
});

var app = builder.Build();
app.UseCors("frontend");
app.MapPost("/api/compile", ([FromBody]CompilationRequest request, CompilationService service) 
    => service.Compile(request.Input));

app.UseDefaultFiles(new DefaultFilesOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, @"www")),
    RequestPath = ""
});

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, @"www")),
    RequestPath = ""
});

app.Run();

[JsonSerializable(typeof(CompilationResult))]
[JsonSerializable(typeof(CompilationRequest))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{
}
