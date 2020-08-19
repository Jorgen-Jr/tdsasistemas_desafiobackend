using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using DesafioBackend.Models;
using DesafioBackend.Middlewares;

namespace DesafioBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Adcionando o contexto do banco de dados �s services,
            services.AddDbContext<MedicoContext>(opt =>
                opt.UseNpgsql("Medicos"));
            services.AddDbContext<MedicoContext>(opt =>
                opt.UseNpgsql("Especialidades"));

            services.AddControllers();
            //Adcionando o service de autentica��o do token jwt.
            services.AddTokenAuthentication(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            // Fazer o uso de autentica��o para acessar as rotas com [Authorize]
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
