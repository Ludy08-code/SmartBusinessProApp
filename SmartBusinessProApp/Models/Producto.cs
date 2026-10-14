using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartBusinessProApp.Models
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public int CategoriaId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; } // Columna 'Stock' en la BD
        public bool Activo { get; set; } = true;

        // Navegación
        public virtual Categoria Categoria { get; set; } = null!;
    }
}