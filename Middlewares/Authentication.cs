using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace DesafioBackend.Middlewares
{
    public static class Authentication
    {
        public static IServiceCollection AddTokenAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var secret = config.GetSection("JwtConfig").GetSection("secret").Value;

            var key = Encoding.ASCII.GetBytes(secret);
            services.AddAuthentication(token =>
            {
                token.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                token.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(token =>
            {
                token.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    //Por hora, não validar Issuer nem Audience
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    //ValidIssuer = "issuer",
                    //ValidAudience = "audience"
                };
            });

            return services;
        }
    }
}
