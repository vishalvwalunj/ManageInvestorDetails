using DapperASPNetCore.Contracts;
using DapperASPNetCore.Repository;
using DapperASPNetCore.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Data;

namespace DapperASPNetCore
{
    public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{

            string connectionString = Configuration.GetConnectionString("SqlConnection");
            services.AddSingleton<IDbConnection>(sp => new SqlConnection(connectionString));
    
            services.AddScoped<IInvestorRepository, InvestorRepository>();
            services.AddScoped<IInvestorService, InvestorService>();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Investor API", Version = "v1" });
            });

            // Add controllers (API)
            services.AddControllers();

        }

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

            app.UseSwagger();

            // Enable Swagger UI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Investor API V1");
                c.RoutePrefix = string.Empty;  // Makes Swagger UI available at the root URL (http://localhost:5000/)
            });


            app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
