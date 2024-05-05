using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;

namespace padaria_project.Extensions
{
    public static class SwaggerSetup
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "padaria_project",
                    Version = "v1",
                    Description = "Descrição do Seu Projeto",
                    Contact = new OpenApiContact
                    {
                        Name = "Romario",
                        Email = "romariosilva08@hotmail.com"
                    }
                });

                // Caminho para o arquivo XML de documentação do seu código
                string xmlPath = Path.Combine(AppContext.BaseDirectory, "padaria_project.xml");
                opt.IncludeXmlComments(xmlPath);
            });

            return services;
        }
    }
}
