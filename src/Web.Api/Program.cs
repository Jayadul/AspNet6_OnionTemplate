using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Extensions;
using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Integrations;
using Web.Framework.Extensions;

var builder = WebApplication.CreateBuilder(args);
var services= builder.Services;
var connectionString = builder.Configuration.GetConnectionString("ConnStr");
// Add services to the container.
services.AddCors();
services.AddAutoMapper();
services.AddFramework((global::Microsoft.Extensions.Configuration.IConfiguration)services,connectionString);
services.AddControllers();
services.AddApiVersioningExtension();
services.AddJsonMultipartFormDataSupport(JsonSerializerChoice.Newtonsoft);
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web.Api", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web.Api v1"));
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod());
app.UseRouting();

app.UseAuthorization();
app.UseStaticFiles();
app.UseApiErrorHandlingMiddleware();
app.UseSerilogRequestLogging();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseHttpsRedirection();
app.MapControllers();
var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == Environments.Development;
var appSettingFile = isDevelopment ? $"appsettings.Development.json" : "appsettings.json";
var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile(appSettingFile, optional: true, reloadOnChange: true)
                .Build();
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .CreateLogger();
app.Run();
