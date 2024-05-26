using System.Windows;
using System.Windows.Controls;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.View
{
    public partial class MenuWindow : Window
    {
        public MenuWindow(string tipoUsuario)
        {
            InitializeComponent();
            ConfigurarOpcoes(tipoUsuario);
        }

        private void ConfigurarOpcoes(string tipoUsuario)
        {
            ModifyButtons(false, btnEstoque, btnPedidos, btnConsultas, btnServicos, btnOrcamento);

            switch (tipoUsuario)
            {
                case "Cliente":
                case "Vendedor":
                    ModifyButtons(true, btnConsultas, btnOrcamento);
                    break;
                case "Técnico":
                    ModifyButtons(true, btnEstoque, btnServicos);
                    break;
                case "Gerente":
                    ModifyButtons(true, btnEstoque, btnPedidos, btnConsultas, btnOrcamento);
                    break;
                case "Administrador":
                    ModifyButtons(true, btnEstoque, btnPedidos, btnConsultas, btnServicos, btnOrcamento);
                    break;
            }
        }

        private void ModifyButtons(bool enable, params Button[] buttons)
        {
            foreach (var button in buttons)
            {
                button.IsEnabled = enable;
            }
        }

        private void btnEstoque_Click(object sender, RoutedEventArgs e)
        {
            new EstoqueWindow().Show();
        }

        private void btnPedidos_Click(object sender, RoutedEventArgs e)
        {
            new PedidoWindow().Show();
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
            new OrcamentoWindow().Show();
        }
    }
}
