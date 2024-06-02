using System.Windows;

namespace OficinaEletrodomesticos.View
{
    public partial class ConfirmarPagamentoDialog : Window
    {
        public double ValorPagamento { get; private set; }

        public ConfirmarPagamentoDialog(double? valorAtual)
        {
            InitializeComponent();
            txtValorPagamento.Text = valorAtual?.ToString() ?? string.Empty;
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtValorPagamento.Text, out double valor))
            {
                ValorPagamento = valor;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Por favor, insira um valor válido.");
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
