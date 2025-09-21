using Marcador.Domain.Interfaces;
using Marcador.Infrastructure.Persistence;
using Marcador.Infrastructure.Repositories;
using Marcador.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Marcador.Application.Abstractions.Services;  
using Marcador.Infrastructure.Services;          

namespace Marcador.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var conn = config.GetConnectionString("SqlServer")
                  ?? throw new InvalidOperationException("ConnectionStrings:SqlServer no configurado");
        services.AddDbContext<MarcadorDbContext>(opt =>
            opt.UseSqlServer(conn));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IPasswordHasher, PasswordHasher>();

        var jwt = new JwtOptions();
        config.GetSection("Jwt").Bind(jwt);
        services.AddSingleton(jwt);
        services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEquipoService, EquipoService>();
        services.AddScoped<IJugadorService, JugadorService>();
        services.AddScoped<IPartidoService, PartidoService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ILogoService, LogoService>();
        return services;
    }
}
