﻿namespace Acerola.WebApi
{
    using Acerola.WebApi.Filters;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using System.IO;
    using System.Reflection;
    using Autofac.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Autofac;

    public sealed class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(DomainExceptionFilter));
                options.Filters.Add(typeof(ValidateModelAttribute));
            });

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();

                options.IncludeXmlComments(
                    Path.ChangeExtension(
                        typeof(Startup).Assembly.Location,
                        "xml"));

                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = Configuration["App:Title"],
                    Version = Configuration["App:Version"],
                    Description = Configuration["App:Description"],
                    TermsOfService = Configuration["App:TermsOfService"]
                });

                options.CustomSchemaIds(x => x.FullName);
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ConfigurationModule(Configuration));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseMvc();

            app.UseSwagger()
               .UseSwaggerUI(c =>
               {
                   c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
               });
        }
    }
}
