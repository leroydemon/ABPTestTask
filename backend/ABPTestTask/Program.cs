using Authorization;
using Infrastructure.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorizationService(builder.Configuration);
builder.Services.AddCustomAuthorization();
builder.Services.AddCustomIdentity();
builder.Services.AddScopedService();
builder.Services.ServiceCollections(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingService>();
app.UseHsts();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
