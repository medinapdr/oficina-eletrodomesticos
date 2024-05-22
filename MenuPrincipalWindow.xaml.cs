using System.Windows;
using OficinaEletrodomesticos.Data;

namespace OficinaEletrodomesticos
{
    public partial class MenuPrincipalWindow : Window
    {
        public MenuPrincipalWindow(string tipoUsuario)
        {
            InitializeComponent();
            ConfigurarOpcoes(tipoUsuario);
        }

        private void ConfigurarOpcoes(string tipoUsuario)
        {
            btnEstoque.IsEnabled = false;
            btnPedidos.IsEnabled = false;
            btnConsultas.IsEnabled = false;
            btnServicos.IsEnabled = false;
            btnOrcamento.IsEnabled = false;

            switch (tipoUsuario)
            {
                case "Cliente":
                    btnConsultas.IsEnabled = true;
                    btnOrcamento.IsEnabled = true;
                    break;
                case "Vendedor":
                    btnConsultas.IsEnabled = true;
                    btnOrcamento.IsEnabled = true;
                    break;
                case "Técnico":
                    btnEstoque.IsEnabled = true;
                    btnServicos.IsEnabled = true;
                    break;
                case "Gerente":
                    btnEstoque.IsEnabled = true;
                    btnPedidos.IsEnabled = true;
                    btnConsultas.IsEnabled = true;
                    btnOrcamento.IsEnabled = true;
                    break;
                case "Administrador":
                    btnEstoque.IsEnabled = true;
                    btnPedidos.IsEnabled = true;
                    btnConsultas.IsEnabled = true;
                    btnServicos.IsEnabled = true;
                    btnOrcamento.IsEnabled = true;
                    break;
            }
        }

        private void btnEstoque_Click(object sender, RoutedEventArgs e)
        {
            EstoqueWindow estoque = new EstoqueWindow();
            estoque.Show();
        }

        private void btnPedidos_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Pedidos clicado");
        }

        private void btnConsultas_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Consultas clicado");
        }

        private void btnServicos_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Serviços clicado");
        }

        private void btnOrcamento_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Orçamento clicado");
        }
    }
}
