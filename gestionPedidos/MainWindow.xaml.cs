using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace gestionPedidos
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection conexionSql;
        public MainWindow()
        {
            InitializeComponent();

            string con = ConfigurationManager.ConnectionStrings["GestionPedidos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;
            conexionSql = new SqlConnection(con);

            mostrarClientes();
            mostraTodosPedidos();
        }

        private void mostrarClientes()
        {
            try
            {
                string sql = "SELECT * FROM CLIENTE";
                SqlDataAdapter adapterSql = new SqlDataAdapter(sql, conexionSql);
                using (adapterSql)
                {
                    DataTable clienteTabla = new DataTable();
                    adapterSql.Fill(clienteTabla);

                    listBox1.DisplayMemberPath = "nombre";
                    listBox1.SelectedValuePath = "Id";
                    listBox1.ItemsSource = clienteTabla.DefaultView;
                }
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void mostrarPedido()
        {
            try
            {
                string sql = "select p.*, CONCAT (c.nombre,'-> ',p.Id, ' ', p.fechaPedido,' ',p.formaPago) as concatenado from Cliente as c INNER JOIN Pedido as p ON c.Id=p.cCliente where c.Id = @clienteId";
                SqlCommand SqlComando = new SqlCommand(sql, conexionSql);
                SqlDataAdapter adapterSql = new SqlDataAdapter(SqlComando);

                using (adapterSql)
                {
                    SqlComando.Parameters.AddWithValue("@clienteId", listBox1.SelectedValue);
                    DataTable pedidosTabla = new DataTable();
                    adapterSql.Fill(pedidosTabla);

                    listBox2.DisplayMemberPath = "concatenado";
                    listBox2.SelectedValuePath = "Id";
                    listBox2.ItemsSource = pedidosTabla.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message);
            }
        }

        private void mostraTodosPedidos()
        {
            try { 
                string sql = "SELECT *, CONCAT (c.nombre,'-> ',p.Id, ' ', p.fechaPedido,' ',p.formaPago) as concatenado FROM PEDIDO as p, CLIENTE as c WHERE c.Id = p.cCliente";
                SqlDataAdapter adapterSql = new SqlDataAdapter(sql, conexionSql);

                using (adapterSql)
                {
                    DataTable todosPedidosTabla = new DataTable();
                    adapterSql.Fill(todosPedidosTabla);

                    listBox2.DisplayMemberPath = "concatenado";                
                    listBox2.SelectedValuePath = "Id";
                    listBox2.ItemsSource = todosPedidosTabla.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_pedidos_Click(object sender, RoutedEventArgs e)
        {
            mostraTodosPedidos();
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
        {           
            Close();
        }

        private void btnBorrar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "DELETE FROM PEDIDO WHERE Id = @pedidoId";
                SqlCommand SqlComando = new SqlCommand(sql, conexionSql);
                conexionSql.Open();
                SqlComando.Parameters.AddWithValue("@pedidoId", listBox2.SelectedValue);
                SqlComando.ExecuteNonQuery();
                conexionSql.Close();
                MessageBox.Show("Pedido Eliminado Correctamente con OID->"+listBox2.SelectedValue);
                mostraTodosPedidos();
            }catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "INSERT INTO CLIENTE (nombre, direccion, poblacion, telefono) VALUES (@nomb, @dir, @pob, @cell)";
                SqlCommand SqlComando = new SqlCommand(sql, conexionSql);
                conexionSql.Open();
                SqlComando.Parameters.AddWithValue("@nomb", impNombre.Text);
                SqlComando.Parameters.AddWithValue("@dir", impDir.Text);
                SqlComando.Parameters.AddWithValue("@pob", impPob.Text);
                SqlComando.Parameters.AddWithValue("@cell", impCell.Text);
                SqlComando.ExecuteNonQuery();
                conexionSql.Close();
                MessageBox.Show("Cliente Agregado Correctamente");
                mostrarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            impNombre.Clear();
            impDir.Clear();
            impPob.Clear();
            impCell.Clear();
        }

        private void btnBorrarCl(object sender, RoutedEventArgs e)
        {
            try
            {
                string sql = "DELETE FROM CLIENTE WHERE Id = @ClienteId";
                SqlCommand SqlComando = new SqlCommand(sql, conexionSql);
                conexionSql.Open();
                SqlComando.Parameters.AddWithValue("@ClienteId", listBox1.SelectedValue);
                SqlComando.ExecuteNonQuery();
                conexionSql.Close();
                MessageBox.Show("Cliente Eliminado Correctamente con OID->" + listBox1.SelectedValue);
                mostrarClientes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            mostrarPedido();
        }

        private void btnActualizar_Click(object sender, RoutedEventArgs e)
        {
            Actualiza actualizar = new Actualiza();            
            Close();
            actualizar.Show();


            try
            {
                string sql = "SELECT * FROM CLIENTE WHERE Id=@ClienteId";
                SqlCommand SqlComando = new SqlCommand(sql, conexionSql);
                SqlDataAdapter adapterSql = new SqlDataAdapter(SqlComando);
                using (adapterSql)
                {
                    SqlComando.Parameters.AddWithValue("@ClienteId", listBox1.SelectedValue);
                    DataTable dt = new DataTable();
                    adapterSql.Fill(dt);
                    actualizar.lbIdCliente.Content = dt.Rows[0]["Id"].ToString();
                    actualizar.impNombre.Text = dt.Rows[0]["nombre"].ToString();
                    actualizar.impDir.Text = dt.Rows[0]["direccion"].ToString();
                    actualizar.impPob.Text = dt.Rows[0]["poblacion"].ToString();
                    actualizar.impCell.Text = dt.Rows[0]["telefono"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
