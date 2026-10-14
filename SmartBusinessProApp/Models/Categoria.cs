using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBusinessProApp.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Activa { get; set; } = true;

        // Propiedad de navegación
        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
