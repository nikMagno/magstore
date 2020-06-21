using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MagStore.Connection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AppWeb
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
            //Configuração para criar os cookies
            services.AddAuthentication("CookieAuthentication")
            .AddCookie("CookieAuthentication", config =>
            {
                config.Cookie.Name = "UserLoginCookie";
                config.LoginPath = "/Login/UsuarioLogin";
                config.LogoutPath = "/Login/Logout";
                // config.AccessDeniedPath = "/Login/AccessDenied";
            });

            services.AddControllersWithViews();

            //Configurando a injeção da conexão
            //Utilizando o padrão singleton para criar uma única conexão para a aplicação
            services.AddSingleton<IConnection, Connection>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //Quem é você
            app.UseAuthentication();
            
            //Verifica Permissões 
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
