﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinhaAPIEstoque.Data;
using MinhaAPIEstoque.Services;
using padaria_project.Extensions;
using System.Text;


namespace MinhaAPIEstoque
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Este método é usado para adicionar serviços ao contêiner.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration); // Adicione esta linha para registrar a IConfiguration
           
            // Configuração do banco de dados SQLite
            services.AddDbContext<MeuDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ProdutoService>();
            services.AddScoped<ApiService>();

            // Configuração do HttpClient
            services.AddHttpClient<ApiService>();

            services.AddSwaggerConfiguration();

            // Configuração do Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "padaria_project", Version = "v1" });
            });

            // Configuração da autenticação JWT
            //var key = Encoding.ASCII.GetBytes(GenerateRandomKey());


            // Configuração da autenticação JWT
            var jwtSettings = Configuration.GetSection("Jwt").Get<JwtSettings>();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });


            // Configuração do HttpClient
            //services.AddHttpClient<ApiService>();

            // Configuração para usar a codificação UTF-8 no pipeline de solicitação HTTP
            //services.AddMvc().AddMvcOptions(options => options.OutputFormatters.Add(new Utf8JsonOutputFormatter()));

            // Configuração do CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAny",
                    builder =>
                    {
                        builder.AllowAnyHeader()
                               .AllowAnyOrigin()
                               .AllowAnyMethod();
                    });
            });

            // Configuração das páginas Razor
            services.AddRazorPages();
        }

        // Configuração da autenticação JWT
        public class JwtSettings
        {
            public string Secret { get; set; }
        }


        // Este método é usado para configurar o pipeline de solicitação HTTP.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "padaria_project V1");
                    c.RoutePrefix = string.Empty;
                });
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Aplicando autenticação JWT
            app.UseAuthentication();

            // Middleware para habilitar o CORS
            app.UseCors("AllowAny");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }

    }
}
