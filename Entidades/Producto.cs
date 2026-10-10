using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades
{
    public class Producto
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public string CantidadPorUnidad { get; set; } = string.Empty;
        public decimal PrecioUnidad { get; set; }
        public short UnidadesEnExistencia { get; set; }
        public string CategoriaProducto { get; set; } = string.Empty;
        public bool Suspendido { get; set; }
    }
}
