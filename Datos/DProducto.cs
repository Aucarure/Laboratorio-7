using Microsoft.Data.SqlClient;
using System.Data;
using Entidades;

namespace Datos
{
    public class DProducto
    {
        Conexion conexion = new Conexion();

        public List<Producto> Listar()
        {
            using SqlConnection connection = conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_Productos_Listar", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            return Leer(connection, cmd);
        }

        public List<Producto> Buscar(string texto)
        {
            using SqlConnection connection = conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_Productos_Buscar", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@texto", texto);
            return Leer(connection, cmd);
        }

        public Producto? ObtenerPorId(int id)
        {
            using SqlConnection connection = conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_Productos_ObtenerPorId", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idproducto", id);
            return Leer(connection, cmd).FirstOrDefault();
        }

        public int Insertar(Producto p)
        {
            using SqlConnection connection = conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_Productos_Insertar", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            AgregarParametros(cmd, p);
            connection.Open();
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public bool Actualizar(Producto p)
        {
            using SqlConnection connection = conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_Productos_Actualizar", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idproducto", p.IdProducto);
            AgregarParametros(cmd, p);
            connection.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool Eliminar(int id)
        {
            using SqlConnection connection = conexion.ObtenerConexion();
            using SqlCommand cmd = new SqlCommand("sp_Productos_Eliminar", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@idproducto", id);
            connection.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        private static void AgregarParametros(SqlCommand cmd, Producto p)
        {
            cmd.Parameters.AddWithValue("@nombreProducto", p.NombreProducto);
            cmd.Parameters.AddWithValue("@cantidadPorUnidad", p.CantidadPorUnidad);
            cmd.Parameters.AddWithValue("@precioUnidad", p.PrecioUnidad);
            cmd.Parameters.AddWithValue("@unidadesEnExistencia", p.UnidadesEnExistencia);
            cmd.Parameters.AddWithValue("@categoriaProducto", p.CategoriaProducto);
        }

        private static List<Producto> Leer(SqlConnection connection, SqlCommand cmd)
        {
            List<Producto> lista = new List<Producto>();
            connection.Open();
            using SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Producto
                {
                    IdProducto = Convert.ToInt32(dr["idproducto"]),
                    NombreProducto = dr["nombreProducto"].ToString() ?? string.Empty,
                    CantidadPorUnidad = dr["cantidadPorUnidad"].ToString() ?? string.Empty,
                    PrecioUnidad = dr["precioUnidad"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["precioUnidad"]),
                    UnidadesEnExistencia = dr["unidadesEnExistencia"] == DBNull.Value ? (short)0 : Convert.ToInt16(dr["unidadesEnExistencia"]),
                    CategoriaProducto = dr["categoriaProducto"].ToString() ?? string.Empty,
                    Suspendido = dr["suspendido"] != DBNull.Value && Convert.ToInt16(dr["suspendido"]) == 1
                });
            }
            return lista;
        }
    }
}   