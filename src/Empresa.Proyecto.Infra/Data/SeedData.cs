using Microsoft.Extensions.DependencyInjection;
using Empresa.Proyecto.Core.Entities;
using System;
using System.Linq;

namespace Empresa.Proyecto.Infra.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = serviceProvider.GetRequiredService<MyProjectContext>())
            {
                // Verificar si ya existen registros
                if (context.SimpleEntity.Any())
                {
                    return; // Ya hay datos, no realizar el seed
                }

                // Agregar registros iniciales
                context.SimpleEntity.AddRange(
                    new SimpleEntity { Name = "Nuevo", Value = "ValorNuevo", Created = DateTime.UtcNow, Modified = DateTime.UtcNow },
                    new SimpleEntity { Name = "Existente", Value = "ValorExistente", Created = DateTime.UtcNow, Modified = DateTime.UtcNow },
                    new SimpleEntity { Name = "Reconstruido", Value = "ValorReconstruido", Created = DateTime.UtcNow, Modified = DateTime.UtcNow }
                );

                // Guardar los cambios
                context.SaveChanges();
            }
        }
    }
}
