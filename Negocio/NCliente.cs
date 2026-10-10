using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Datos;
using Entidades;

namespace Negocio
{
    public class NCliente
    {
        public List<Cliente> Listar()
        {
            DCliente dCliente = new DCliente();
            return dCliente.ListarClientes();
        }

        public List<Cliente> ListarPorNombre(String nombre)
        {
            DCliente dCliente = new DCliente();
            var clientes = dCliente.ListarClientes();
            var clientesFiltrados = 
                clientes.Where(c => c.NombreCompania.Contains(nombre)).ToList();
            return clientesFiltrados;
        }
    }
}
