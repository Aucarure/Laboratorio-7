using System;
using System.Collections.Generic;
using System.Text;

using Datos;
using Entidades;

namespace Negocio
{
    public class NProducto
    {
        private readonly DProducto _datos = new DProducto();

        public List<Producto> Listar() => _datos.Listar();

        public List<Producto> Buscar(string texto)
            => string.IsNullOrWhiteSpace(texto) ? _datos.Listar() : _datos.Buscar(texto.Trim());

        public Producto? ObtenerPorId(int id) => _datos.ObtenerPorId(id);

        public int Insertar(Producto p)
        {
            Validar(p);
            return _datos.Insertar(p);
        }

        public bool Actualizar(Producto p)
        {
            if (p.IdProducto <= 0) throw new ArgumentException("Producto no válido.");
            Validar(p);
            return _datos.Actualizar(p);
        }

        public bool Eliminar(int id)
        {
            if (id <= 0) throw new ArgumentException("Producto no válido.");
            return _datos.Eliminar(id);
        }

        private static void Validar(Producto p)
        {
            if (string.IsNullOrWhiteSpace(p.NombreProducto))
                throw new ArgumentException("El nombre es obligatorio.");
            if (p.NombreProducto.Length > 40)
                throw new ArgumentException("El nombre no puede exceder 40 caracteres.");
            if (p.CantidadPorUnidad.Length > 20)
                throw new ArgumentException("Cantidad por unidad: máximo 20 caracteres.");
            if (p.PrecioUnidad < 0)
                throw new ArgumentException("El precio no puede ser negativo.");
            if (p.UnidadesEnExistencia < 0)
                throw new ArgumentException("El stock no puede ser negativo.");
            if (p.CategoriaProducto.Length > 20)
                throw new ArgumentException("Categoría: máximo 20 caracteres.");
        }
    }
}