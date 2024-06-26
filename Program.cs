using ScoreHubAPI.Controllers.ErrorHandling;
using ScoreHubAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

await app.Services.InitializeDbAsync();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseCors("AllowAllOrigins");

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}"
    );

app.Run();
