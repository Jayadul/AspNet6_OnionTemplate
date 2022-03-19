using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Formatters;
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
services.AddFramework(builder.Configuration,connectionString);
services.AddControllers();
services.AddApiVersioningExtension();
services.AddJsonMultipartFormDataSupport(JsonSerializerChoice.Newtonsoft);
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Web.Api", Version = "v1" });
});
//services.AddMvc(setupAction =>
//{
//    setupAction.Filters.Add(
//        new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
//    setupAction.Filters.Add(
//        new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
//    setupAction.Filters.Add(
//        new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
//    setupAction.Filters.Add(
//        new ProducesDefaultResponseTypeAttribute());
//    setupAction.Filters.Add(
//        new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized));

//    setupAction.Filters.Add(
//        new AuthorizeFilter());

//    setupAction.ReturnHttpNotAcceptable = true;

//    setupAction.OutputFormatters.Add(new XmlSerializerOutputFormatter());

//    var jsonOutputFormatter = setupAction.OutputFormatters
//        .OfType<SystemTextJsonOutputFormatter>().FirstOrDefault();

//    if (jsonOutputFormatter != null)
//    {
//        // remove text/json as it isn't the approved media type
//        // for working with JSON at API level
//        if (jsonOutputFormatter.SupportedMediaTypes.Contains("text/json"))
//        {
//            jsonOutputFormatter.SupportedMediaTypes.Remove("text/json");
//        }
//    }
//});

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
//app.UseSerilogRequestLogging();
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
