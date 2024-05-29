using System.Windows;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.View
{
    public partial class AdicionarServicoDialog : Window
    {
        public Servico NovoServico { get; private set; }

        public AdicionarServicoDialog(Funcionario funcionario)
        {
            InitializeComponent();
            CarregarTecnicos(funcionario);
            CarregarOrcamentos();
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

        private void CarregarOrcamentos()
        {
            var orcamentos = OrcamentoRepository.ObterOrcamentos();
            foreach (var orcamento in orcamentos)
            {
                string orcamentoFormatado = $"{orcamento.Id} - {orcamento.TipoAparelho} - {orcamento.DataOrcamento.ToShortDateString()}";
                cmbOrcamento.Items.Add(orcamentoFormatado);
            }
        }



        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            if (cmbOrcamento.SelectedItem != null &&
                !string.IsNullOrWhiteSpace(txtDescricao.Text) &&
                cmbTecnico.SelectedItem != null)
            {
                // Extrair o ID do orçamento da string selecionada na combobox
                string orcamentoSelecionado = cmbOrcamento.SelectedItem.ToString();
                int orcamentoId = int.Parse(orcamentoSelecionado.Split('-')[0].Trim());

                // Obter os IDs do técnico e do orçamento selecionados
                var tecnicoId = (cmbTecnico.SelectedItem as Funcionario)?.Id ?? 0;

                // Criar o novo serviço com os IDs obtidos
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
