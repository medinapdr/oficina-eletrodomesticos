using OficinaEletrodomesticos.Models;
using System.Windows;

namespace OficinaEletrodomesticos.View
{
    public partial class PedidoWindow : Window
    {
        private readonly Estoque _estoque;

        public PedidoWindow()
        {
            InitializeComponent();
            _estoque = new Estoque();
            DataContext = this;

            dgPedidos.ItemsSource = _estoque.ConsultarPedidos();
            cmbPeca.ItemsSource = _estoque.ConsultarEstoque();

            txtQuantidade.TextChanged += (s, e) => AtualizarValorTotal();
            txtValorUnitario.TextChanged += (s, e) => AtualizarValorTotal();
        }

        private void AtualizarValorTotal()
        {
            if (decimal.TryParse(txtValorUnitario.Text, out var valorUnitario) && int.TryParse(txtQuantidade.Text, out var quantidade))
            {
                txtValorTotal.Text = (valorUnitario * quantidade).ToString("F2");
            }
            else
            {
                txtValorTotal.Text = string.Empty;
            }
        }

        private void AdicionarPedido_Click(object sender, RoutedEventArgs e)
        {
            if (cmbPeca.SelectedItem == null || string.IsNullOrEmpty(txtQuantidade.Text) || string.IsNullOrEmpty(txtValorTotal.Text) || string.IsNullOrEmpty(txtFornecedor.Text))
            {
                MessageBox.Show("Por favor, preencha todos os campos.");
                return;
            }

            var peca = cmbPeca.SelectedItem as Peca;

            var novoPedido = new Pedido
            {
                Peca = new Peca
                {
                    Id = peca.Id,
                    Nome = peca.Nome,
                    Fabricante = peca.Fabricante,
                    Quantidade = int.Parse(txtQuantidade.Text),
                },
                Quantidade = int.Parse(txtQuantidade.Text),
                ValorUnitario = decimal.Parse(txtValorUnitario.Text),
                ValorTotal = decimal.Parse(txtValorTotal.Text),
                Fornecedor = txtFornecedor.Text,
                DataCriacao = DateTime.Now
            };

            if (_estoque.AdicionarPedido(novoPedido))
            {
                dgPedidos.ItemsSource = _estoque.ConsultarPedidos(); // Atualize o DataGrid
                MessageBox.Show("Pedido adicionado com sucesso!");
            }
            else
            {
                MessageBox.Show("Falha ao adicionar o pedido.");
            }
        }

        private void ConfirmarRecebimento_Click(object sender, RoutedEventArgs e)
        {
            if (dgPedidos.SelectedItem is Pedido pedido )
            {
                if(_estoque.ConfirmarRecebimentoPedido(pedido)) 
                {
                    dgPedidos.ItemsSource = _estoque.ConsultarPedidos();
                }
                else
                {
                    MessageBox.Show("Esse pedido já foi recebido.");
                }
            }
            else
            {
                MessageBox.Show("Selecione um pedido para confirmar o recebimento.");
            }
        }
    }
}
