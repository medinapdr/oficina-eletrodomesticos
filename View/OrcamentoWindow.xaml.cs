using System.Windows;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.View
{
    public partial class OrcamentoWindow : Window
    {
        private readonly ConexaoBanco _conexaoBanco;
        private Historico _historico;

        public OrcamentoWindow(ConexaoBanco conexaoBanco)
        {
            InitializeComponent();
            _conexaoBanco = conexaoBanco;
            _historico = new Historico(conexaoBanco);
        }

        private void CriarOrcamento_Click(object sender, RoutedEventArgs e)
        {
            string tipoAparelho = txtAparelho.Text;
            string nomeCliente = txtCliente.Text;
            string descricao = txtDescricao.Text;

            if (string.IsNullOrWhiteSpace(tipoAparelho) || string.IsNullOrWhiteSpace(nomeCliente) || string.IsNullOrWhiteSpace(descricao))
            {
                MessageBox.Show("Por favor, preencha todos os campos.");
                return;
            }

            Cliente cliente = new Cliente { Nome = nomeCliente };
            Aparelho aparelho = new Aparelho { Tipo = tipoAparelho, ClienteAssociado = cliente };

            SolicitacaoOrcamento solicitacao = new SolicitacaoOrcamento {
                Aparelho = aparelho,
                Cliente = cliente,
                Descricao = descricao 
            };

            Orcamento orcamento = new Orcamento { Solicitacao = solicitacao };

            if (decimal.TryParse(txtValorTotal.Text, out decimal valorTotal))
            {
                orcamento.ValorTotal = valorTotal;
            }
            else
            {
                MessageBox.Show("Por favor, insira um valor total válido.");
                return;
            }

            if (int.TryParse(txtPrazoEntrega.Text, out int prazoEntrega))
            {
                orcamento.PrazoEntrega = TimeSpan.FromDays(prazoEntrega);
            }
            else
            {
                MessageBox.Show("Por favor, insira um prazo de entrega válido.");
                return;
            }

            _historico.AdicionarOrcamento(orcamento);
            MessageBox.Show("Orçamento criado com sucesso.");
        }

        private void Fechar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
