using Datos;
using Entidades;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace aplicacion_01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DgClientes.ItemsSource = ListarClientes();
        }
        private readonly Conexion _conexion = new Conexion();
        public List<Cliente> ListarClientes()
        {
            List<Cliente> lista = new List<Cliente>();

            try
            {
                using SqlConnection connection = _conexion.ObtenerConexion();
                connection.Open();

                using SqlCommand cmd = new SqlCommand("sp_Clientes_Listar", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(MapearCliente(dr));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return lista;
        }

        public Cliente? BuscarClientePorId(string idCliente)
        {
            Cliente? cliente = null;

            try
            {
                using SqlConnection connection = _conexion.ObtenerConexion();
                connection.Open();

                using SqlCommand cmd = new SqlCommand("sp_Clientes_ObtenerPorId", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@idCliente", idCliente));

                using SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    cliente = MapearCliente(dr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return cliente;
        }

        private Cliente MapearCliente(SqlDataReader dr)
        {
            return new Cliente
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
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtBuscarId.Text))
            {
                MessageBox.Show("Ingresa un ID de cliente.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Cliente? cliente = BuscarClientePorId(TxtBuscarId.Text.Trim());
            DgClientes.ItemsSource = cliente is null ? new List<Cliente>() : new List<Cliente> { cliente };

            if (cliente is null)
            {
                MessageBox.Show("No se encontró un cliente activo con ese ID.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnMostrarTodos_Click(object sender, RoutedEventArgs e)
        {
            TxtBuscarId.Clear();
            DgClientes.ItemsSource = ListarClientes();
        }
        private void BtnIrProductos_Click(object sender, RoutedEventArgs e)
        {
            ProductosWindow ventana = new ProductosWindow();
            ventana.Owner = this;
            ventana.ShowDialog();
        }
    }
}