using PariSportif;
using PariSportif.Data;
using Microsoft.EntityFrameworkCore;
using PariSportif.Services;
using PariSportif.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Base de données MySQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    )
);

// Controllers
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Enregistrement repo + service
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IWalletService, WalletService>();
builder.Services.AddScoped<IJwtService, JwtService>();

var app = builder.Build();

// Swagger uniquement en développement
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

// Appliquer les migrations + seed
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var retries = 10;

    while (true)
    {
        try
        {
            context.Database.Migrate();
            break;
        }
        catch
        {
            if (--retries == 0) throw;
            Thread.Sleep(3000);
        }
    }

    // IMPORTANT : seed safe
    if (!context.Database.CanConnect())
        return;

    // IMPORTANT FIX ICI
    context.Database.EnsureCreated(); // 🔥 sécurité (optionnel mais utile en dev)

    if (!context.Users.Any())
    {
        DbInitializer.Seed(context);
    }
}

app.Run();