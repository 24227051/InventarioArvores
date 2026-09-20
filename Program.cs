using MongoDB.Driver;

namespace InventarioArvores
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

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
            });

            var app = builder.Build();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseAuthorization();

            app.UseCors("AllowAllOrigins");

            app.MapControllers();

            app.Run();
        }
    }
}
