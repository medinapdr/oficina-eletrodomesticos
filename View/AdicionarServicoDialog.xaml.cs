using System.Windows;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.View
{
    public partial class AdicionarServicoDialog : Window
    {
        public Servico NovoServico { get; private set; }

        public AdicionarServicoDialog(Funcionario funcionario, Orcamento orcamento = null)
        {
            InitializeComponent();
            CarregarTecnicos(funcionario);
            CarregarOrcamentos(orcamento);
        }

        private void CarregarTecnicos(Funcionario funcionario)
        {
            if (funcionario.Cargo == Cargo.Técnico)
            {
                cmbTecnico.Items.Add(funcionario.Nome);
                cmbTecnico.SelectedIndex = 0;
                cmbTecnico.IsEnabled = false;
            }
            else
            {
                var tecnicos = ServicoRepository.ObterTecnicos();
                cmbTecnico.ItemsSource = tecnicos;
                cmbTecnico.DisplayMemberPath = "Nome";
                cmbTecnico.SelectedValuePath = "Id";
            }
        }

        private void CarregarOrcamentos(Orcamento orcamento)
        {
            var orcamentos = OrcamentoRepository.ObterOrcamentos();
            foreach (var orc in orcamentos)
            {
                string orcamentoFormatado = $"{orc.Id} - {orc.DataOrcamento.ToShortDateString()} - {orc.Solicitacao.Cliente.Nome}: {orc.Solicitacao.Aparelho.Tipo} ({orc.Solicitacao.Aparelho.Marca})";
                cmbOrcamento.Items.Add(orcamentoFormatado);

                if (orcamento != null && orc.Id == orcamento.Id)
                {
                    cmbOrcamento.SelectedItem = orcamentoFormatado;
                    cmbOrcamento.IsEnabled = false;
                }
            }
        }


        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            if (cmbOrcamento.SelectedItem != null &&
                !string.IsNullOrWhiteSpace(txtDescricao.Text) &&
                cmbTecnico.SelectedItem != null)
            {
                // Extrai o ID do orçamento da string selecionada na combobox
                string orcamentoSelecionado = cmbOrcamento.SelectedItem.ToString();
                int orcamentoId = int.Parse(orcamentoSelecionado.Split('-')[0].Trim());

                // Obtém os IDs do técnico e do orçamento selecionados
                var tecnicoId = (cmbTecnico.SelectedItem as Funcionario)?.Id ?? 0;

                NovoServico = new Servico
                {
                    Orcamento = new Orcamento { Id = orcamentoId },
                    Descricao = txtDescricao.Text,
                    Status = StatusServico.Parado,
                    TecnicoResponsavel = new Funcionario { Id = tecnicoId }
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