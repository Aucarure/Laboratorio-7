using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Entidades;
using Negocio;

namespace aplicacion_01
{
    public partial class ProductosWindow : Window
    {
        private readonly NProducto _nProducto = new NProducto();
        private int _idSeleccionado = 0;

        public ProductosWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) => CargarProductos();

        private void CargarProductos()
        {
            try
            {
                DgProductos.ItemsSource = _nProducto.Listar();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al listar: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnBuscar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DgProductos.ItemsSource = _nProducto.Buscar(TxtBuscar.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnMostrarTodos_Click(object sender, RoutedEventArgs e)
        {
            TxtBuscar.Clear();
            CargarProductos();
        }

        private void DgProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgProductos.SelectedItem is not Producto p) return;
            _idSeleccionado = p.IdProducto;
            TxtNombre.Text = p.NombreProducto;
            TxtCantidad.Text = p.CantidadPorUnidad;
            TxtPrecio.Text = p.PrecioUnidad.ToString();
            TxtStock.Text = p.UnidadesEnExistencia.ToString();
            TxtCategoria.Text = p.CategoriaProducto;
        }

        private void BtnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!decimal.TryParse(TxtPrecio.Text, out decimal precio))
                    throw new ArgumentException("El precio debe ser numérico.");
                if (!short.TryParse(TxtStock.Text, out short stock))
                    throw new ArgumentException("El stock debe ser un número entero.");

                Producto p = new Producto
                {
                    IdProducto = _idSeleccionado,
                    NombreProducto = TxtNombre.Text.Trim(),
                    CantidadPorUnidad = TxtCantidad.Text.Trim(),
                    PrecioUnidad = precio,
                    UnidadesEnExistencia = stock,
                    CategoriaProducto = TxtCategoria.Text.Trim()
                };

                if (_idSeleccionado == 0) _nProducto.Insertar(p);
                else _nProducto.Actualizar(p);

                MessageBox.Show("Producto guardado correctamente.", "Aviso",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                LimpiarFormulario();
                CargarProductos();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Validación",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (_idSeleccionado == 0)
            {
                MessageBox.Show("Selecciona un producto.", "Aviso",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBox.Show("¿Eliminar el producto seleccionado?", "Confirmar",
                MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;

            try
            {
                _nProducto.Eliminar(_idSeleccionado);
                LimpiarFormulario();
                CargarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnNuevo_Click(object sender, RoutedEventArgs e) => LimpiarFormulario();

        private void LimpiarFormulario()
        {
            _idSeleccionado = 0;
            TxtNombre.Clear();
            TxtCantidad.Clear();
            TxtPrecio.Clear();
            TxtStock.Clear();
            TxtCategoria.Clear();
            DgProductos.SelectedItem = null;
        }
    }
}
