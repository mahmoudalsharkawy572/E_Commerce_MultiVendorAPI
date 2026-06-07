using ECommerce.API.Extensions;
using ECommerce.Application.Extensions;
using ECommerce.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

#region ===================== SERVICES =====================

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.AddPresentation();

#endregion

var app = builder.Build();

#region ===================== PIPELINE =====================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();