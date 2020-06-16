using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TicTacToe.Shared;

namespace TicTacToe.Server
{
    public sealed class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSignalR();
            services.AddMvc();
            services.AddSingleton<ServerUsersState>();
            services.AddSingleton<IUsersState>(p => p.GetService<ServerUsersState>());
            services.AddSingleton<IActiveUserState>(p => p.GetService<ServerUsersState>());
            services.AddSingleton<IEnumerable<UserModel>>(p => p.GetService<ServerUsersState>());
           
            services.AddSingleton<ServerFieldModel>();
            services.AddSingleton<ICheckableField>(p => p.GetService<ServerFieldModel>());
            services.AddSingleton<ISettableField>(p => p.GetService<ServerFieldModel>());
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
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<GameHub>(Paths.GameHub);
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });
        }
    }
}