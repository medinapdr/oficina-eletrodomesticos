using System.Windows;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.View
{
    public partial class AdicionarServicoDialog : Window
    {
        public Servico NovoServico { get; private set; }

        public AdicionarServicoDialog()
        {
            InitializeComponent();
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtOrcamentoId.Text, out int orcamentoId) && !string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                NovoServico = new Servico
                {
                    Orcamento = new Orcamento { Id = orcamentoId },
                    Descricao = txtDescricao.Text,
                    Status = StatusServico.Parado
                };
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Por favor, insira todos os dados necessários.");
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
