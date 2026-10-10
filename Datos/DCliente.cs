using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Entidades;

namespace Datos
{
    public class DCliente
    {
        Conexion conexion = new Conexion();
        public List<Cliente> ListarClientes()
        {
            List<Cliente> lista = new List<Cliente>();

            try
            {
                using SqlConnection connection = conexion.ObtenerConexion();
                connection.Open();

                using SqlCommand cmd = new SqlCommand("sp_Clientes_Listar", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Cliente cliente = new Cliente
                    {
                        IdCliente = dr["idCliente"].ToString() ?? string.Empty,
                        NombreCompania = dr["NombreCompañia"].ToString() ?? string.Empty,
                        NombreContacto = dr["NombreContacto"].ToString(),
                        CargoContacto = dr["CargoContacto"].ToString(),
                        Direccion = dr["Direccion"].ToString(),
                        Ciudad = dr["Ciudad"].ToString(),
                        Region = dr["Region"].ToString(),
                        CodPostal = dr["CodPostal"].ToString(),
                        Pais = dr["Pais"].ToString(),
                        Telefono = dr["Telefono"].ToString(),
                        Fax = dr["Fax"].ToString(),
                        Eliminado = Convert.ToBoolean(dr["Eliminado"])
                    };
                    lista.Add(cliente);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lista;
        }

        public void Crear()
        {
        }

        public void Actualizar()
        {
        }

        public void Eliminar()
        {
        }
       
    }
}
