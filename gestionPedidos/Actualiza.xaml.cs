using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace gestionPedidos
{
    /// <summary>
    /// Lógica de interacción para Actualiza.xaml
    /// </summary>
    public partial class Actualiza : Window
    {
        SqlConnection conexionSql;
        public Actualiza()
        {
            InitializeComponent();
            string con = ConfigurationManager.ConnectionStrings["GestionPedidos.Properties.Settings.GestionPedidosConnectionString"].ConnectionString;
            conexionSql = new SqlConnection(con);
        }

        private void btnCerrar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();            
            main.Show();
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {            
            try
            {
                string sql = "UPDATE CLIENTE  SET nombre=@nomb, direccion=@dir, poblacion=@pob, telefono=@cell WHERE Id=@ClienteId";
                SqlCommand SqlComando = new SqlCommand(sql, conexionSql);
                conexionSql.Open();
                SqlComando.Parameters.AddWithValue("@ClienteId", lbIdCliente.Content);
                SqlComando.Parameters.AddWithValue("@nomb", impNombre.Text);
                SqlComando.Parameters.AddWithValue("@dir", impDir.Text);
                SqlComando.Parameters.AddWithValue("@pob", impPob.Text);
                SqlComando.Parameters.AddWithValue("@cell", impCell.Text);
                SqlComando.ExecuteNonQuery();
                conexionSql.Close();
                MessageBox.Show("Cliente Actualizado Correctamente");

                MainWindow main = new MainWindow();
                main.Show();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
