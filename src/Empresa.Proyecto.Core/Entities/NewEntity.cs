using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Empresa.Proyecto.Core.Entities
{
    public class NewEntity : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        public int SimpleEntityId { get; set; }

        [ForeignKey("SimpleEntityId")]
        public SimpleEntity? SimpleEntity { get; set; }

        public DateTime Creado { get; set; } = DateTime.UtcNow;

        public DateTime Modificado { get; set; } = DateTime.UtcNow;
    }
}
