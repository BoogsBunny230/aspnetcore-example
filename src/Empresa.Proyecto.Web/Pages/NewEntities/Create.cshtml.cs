using Empresa.Proyecto.Core.Entities;
using Empresa.Proyecto.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Empresa.Proyecto.Web.Pages.NewEntities
{
    public class CreateModel : PageModel
    {
        private readonly IAsyncRepository<Empresa.Proyecto.Core.Entities.NewEntity> _newEntityRepo;
        private readonly IAsyncRepository<SimpleEntity> _simpleEntityRepo;

        public CreateModel(
            IAsyncRepository<Empresa.Proyecto.Core.Entities.NewEntity> newEntityRepo,
            IAsyncRepository<SimpleEntity> simpleEntityRepo)
        {
            _newEntityRepo = newEntityRepo;
            _simpleEntityRepo = simpleEntityRepo;
        }

        [BindProperty]
        public Empresa.Proyecto.Core.Entities.NewEntity NewEntity { get; set; } = new Empresa.Proyecto.Core.Entities.NewEntity();

        public IReadOnlyList<SimpleEntity> SimpleEntities { get; set; } = new List<SimpleEntity>();

        public string? ErrorMessage { get; set; }

        public async Task OnGetAsync()
        {
            // Cargar las opciones de SimpleEntity para el dropdown
            SimpleEntities = await _simpleEntityRepo.ListAllAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Por favor, corrige los errores.";
                return Page();
            }

            try
            {
                NewEntity.Creado = DateTime.UtcNow;
                NewEntity.Modificado = DateTime.UtcNow;
                await _newEntityRepo.AddAsync(NewEntity);

                return RedirectToPage("/NewEntities/Create"); // Redirigir a la misma página para refrescar la tabla
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ocurrió un error al guardar: {ex.Message}";
                return Page();
            }
        }

        public async Task<JsonResult> OnPostGetAllNewEntities()
        {
            try
            {
                // Obtener todas las entidades NewEntity
                var newEntities = await _newEntityRepo.ListAllAsync();

                // Transformar datos para incluir el nombre de la opción (SimpleEntity)
                var transformedEntities = newEntities.Select(ne => new
                {
                    ne.Id,
                    ne.Nombre,
                    SimpleEntityName = ne.SimpleEntity.Name, // Nombre de la opción seleccionada
                    ne.Creado,
                    ne.Modificado
                }).ToList();

                // Devolver datos en el formato esperado por DataTables
                return new JsonResult(new
                {
                    data = transformedEntities,
                    recordsTotal = transformedEntities.Count,
                    recordsFiltered = transformedEntities.Count
                });
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error al cargar los datos: {ex.Message}";
                return new JsonResult(new { error = "Ocurrió un error al procesar los datos." });
            }
        }
    }
}
