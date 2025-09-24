using LucasRT.RavenDB.SalesAssistant.RestApi.Application.Services.Leads;
using LucasRT.RavenDB.SalesAssistant.RestApi.Application.Services.Messages;
using LucasRT.RavenDB.SalesAssistant.RestApi.Configurations;
using Swashbuckle.AspNetCore.SwaggerUI;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddRavenDB(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddResponseCompression();
builder.Services.AddControllersConfig();
builder.Services.AddSwaggerConfig();
builder.Services.AddScoped<ILeadService, LeadService>();
builder.Services.AddScoped<IMessageService, MessageService>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", $"RavenDB Sales Assistant Demo");
        c.InjectStylesheet("/swagger-ui/SwaggerDark.css");
        c.DocExpansion(DocExpansion.None);
        c.DefaultModelExpandDepth(2);
        c.DefaultModelRendering(ModelRendering.Example);
        c.DisplayOperationId();
        c.DisplayRequestDuration();
        c.EnableDeepLinking();
        c.EnableTryItOutByDefault();
        c.EnableValidator();
        c.ShowCommonExtensions();
    });

}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();

app.MapControllers();

app.Run();
