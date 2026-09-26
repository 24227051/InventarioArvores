using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using InventarioArvores.Data;

namespace InventarioArvores
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ==========================================
            // 1. CONFIGURAÇÃO DO SQL SERVER + IDENTITY
            // ==========================================
            var sqlConnectionString = builder.Configuration.GetConnectionString("SqlContext");

            // Banco de dados do Identity (SQL Server)
            builder.Services.AddDbContext<IdentityDataContext>(options =>
                options.UseSqlServer(sqlConnectionString));

            // Ativa a autenticação do Identity
            builder.Services.AddAuthorization();
            builder.Services.AddIdentityApiEndpoints<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<IdentityDataContext>();

            // ==========================================
            // 2. CONFIGURAÇÃO DO MONGODB
            // ==========================================
            var mongoConnectionString = builder.Configuration.GetConnectionString("MongoContext");

            // Registra o IMongoClient como Singleton (padrão recomendado pela equipe do Mongo)
            builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(mongoConnectionString));

            // Injeta o banco de dados específico para ficar fácil de usar nos Controllers/Services
            builder.Services.AddScoped(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase("InventarioArvoresDb"); // Nome do seu banco NoSQL
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .AllowAnyHeader());
                //options.AddPolicy("PermitirAngular", policy =>
                //{
                //    policy.WithOrigins("https://thankful-ocean-0d8fef90f.1.azurestaticapps.net/") // URL do seu Static App Angular
                //          .AllowAnyHeader()
                //          .AllowAnyMethod();
                //});
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseCors("AllowAllOrigins");
            //app.UseCors("PermitirAngular");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapIdentityApi<IdentityUser>();
            app.MapControllers();

            app.Run();
        }
    }
}
