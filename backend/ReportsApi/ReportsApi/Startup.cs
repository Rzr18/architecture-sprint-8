using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using ReportWebApi.Authorization;

namespace ReportWebApi;

public class Startup
{
    private readonly AuthPolicyRoles _authPolicy = new()
    {
        Name = "ProtheticsOnly",
        Roles = ["prothetic_user"]
    };

    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        // Add services to the container.
        services.AddControllers();

        services.AddScoped<IClaimsTransformation, AddRolesClaimsTransformation>();

        services
            .AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = _configuration.GetSection("KeycloakAuthority").Get<string>();
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });

        services
            .AddAuthorizationBuilder()
            .AddPolicy(_authPolicy.Name, policy =>
            {
                foreach (var role in _authPolicy.Roles)
                {
                    policy.RequireRole(role);
                }
            });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors(x => x
            .WithOrigins(GetAllowedOrigins(_configuration))
            .AllowAnyMethod()
            .AllowAnyHeader());


        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }

    private static string[] GetAllowedOrigins(IConfiguration configuration) =>
        configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [];
}