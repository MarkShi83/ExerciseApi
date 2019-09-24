using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace ExerciseApi
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
			services
                .AddSwaggerGen()
                .AddMvc(options => options.RespectBrowserAcceptHeader = true)
                .AddJsonOptions(
                    options =>
                        {
                            options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                            options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                        })
                .AddXmlSerializerFormatters();
        }

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
                app.UseHsts();
			}

			app
                .UseHttpsRedirection()
                .UseMvc()
                .UseSwagger();
		}
	}
}
