using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;

namespace Empresa.Proyecto.Infra.Data
{
    public class MyProjectContextFactory : IDesignTimeDbContextFactory<MyProjectContext>
    {
        public MyProjectContext CreateDbContext(string[] args)
        {
            // Cargar configuración desde appsettings.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Necesita FileExtensions
                .AddJsonFile("appsettings.json")
                .Build();

            // Obtener la cadena de conexión
            var connectionString = configuration.GetConnectionString("DBConnection");

            // Configurar opciones para MyProjectContext
            var optionsBuilder = new DbContextOptionsBuilder<MyProjectContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new MyProjectContext(optionsBuilder.Options);
        }
    }
}
