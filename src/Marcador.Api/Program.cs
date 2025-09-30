using Marcador.Api.Extensions;
using Marcador.Infrastructure;
using Marcador.Infrastructure.Persistence;
using Marcador.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http://129.212.189.102:4200") 
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddValidation();
builder.Services.AddJwtAuth(builder.Configuration);
builder.Services.AddSwaggerDocumentation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowAngular");


// app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapPost("/setup/seed", async (MarcadorDbContext db, Marcador.Infrastructure.Security.IPasswordHasher hasher) =>
{
    var pwdHash = hasher.Hash("Admin123!");
    await DataSeeder.SeedAsync(db, "admin", pwdHash);
    return Results.Ok("Seed completado");
});

app.Run();
