using System.Text.Json.Serialization;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using kafkaclient.web.Core.Config;
using kafkaclient.web.Core.Mappings;
using kafkaclient.web.Infrastructure.IoC;
using kafkaclient.web.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddInfrastructure(builder.Configuration);

var config = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new ClusterMapper());
});

var mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddResponseCompression(options => {
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});


// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "Mi API",
        Description = "Descripción de mi API"
    });
});

builder.Services.AddFluentValidationAutoValidation(config => {
    config.DisableDataAnnotationsValidation = false;
});
builder.Services.AddValidatorsFromAssemblyContaining<PagingValidator>();
builder.Services.Configure<ApiBehaviorOptions>(o => {
    o.InvalidModelStateResponseFactory = HandlerErrorRequest.ConfigureHandlerErrorRequest;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}else{
    Console.WriteLine("Development mode enabled");
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Configure response error unexpected
app.ConfigureHandlerErrorUnexpected();

// Configure middleware for error not found
app.UseMiddleware<ErrorCustomHandlerMiddleware>();

// Configure middleware for error not found
app.ConfigureHandlerErrorNotFound();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseResponseCompression();
app.UseSwagger(x => x.SerializeAsV2 = true);

// app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
