using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Empresa.Proyecto.Core.Entities;
using Empresa.Proyecto.Core.Interfaces;

namespace Empresa.Proyecto.Web.Pages
{
    public class BaseCatalogPageModel<T>: PageModel where T : BaseEntity
    {
        private readonly IAsyncRepository<T> Repo;
        private readonly ILogger<BaseCatalogPageModel<T>> Logger;

        public IReadOnlyList<T> Entidades { get; set; } = null!;

        public BaseCatalogPageModel(ILogger<BaseCatalogPageModel<T>> logger, IAsyncRepository<T> repo)
        {
            Repo = repo;
            Logger = logger;
        }

        public async Task<JsonResult> OnPostCatalog(int start = 0, int length = 10)
{
    try
    {
        // Obtener todos los registros
        var catalog = await Repo.ListAllAsync();

        // Total de registros antes de paginación
        var totalRecords = catalog.Count;

         var orderedCatalog = catalog.OrderBy(e => 
        e.GetType().GetProperty("Name")?.GetValue(e, null)?.ToString()).ToList();

        // Aplicar paginación
        var paginatedCatalog = orderedCatalog.Skip(start).Take(length).ToList();

        // Devolver datos en el formato esperado por DataTables
        return new JsonResult(new
        {
            data = paginatedCatalog,
            recordsTotal = totalRecords,
            recordsFiltered = totalRecords
        });
    }
    catch (Exception ex)
    {
        // En caso de error, registrar y devolver mensaje de error genérico
        Logger.LogError(ex, "Error al obtener los datos del catálogo");
        return new JsonResult(new { error = "Ocurrió un error al procesar los datos." });
    }
}

    }
}
