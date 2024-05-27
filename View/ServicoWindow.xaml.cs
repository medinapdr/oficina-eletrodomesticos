using System.Windows;
using System.Windows.Controls;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.View
{
    public partial class ServicoWindow : Window
    {
        private readonly Funcionario _funcionario;

        public ServicoWindow(Funcionario funcionario)
        {
            InitializeComponent();
            _funcionario = funcionario;
            CarregarServicos();
            CarregarMeusServicos();

            if (_funcionario.Cargo != Cargo.Técnico)
            {
                // Desabilite a tab "Meus Serviços" para funcionários que não são técnicos
                var tabItemMeusServicos = tabControlServicos.Items.Cast<TabItem>().FirstOrDefault(item => item.Header.ToString() == "Meus Servicos");
                if (tabItemMeusServicos != null)
                {
                    tabItemMeusServicos.IsEnabled = false;
                }
            }


            // Desabilite o botão "Alterar Status" para vendedores
            if (_funcionario.Cargo == Cargo.Vendedor)
            {
                btnAlterarStatus.IsEnabled = false;
            }
        }

        private void CarregarServicos()
        {
            List<Servico> servicos = ServicoRepository.ObterTodosServicos();
            dataGridServicos.ItemsSource = servicos;
        }

        private void CarregarMeusServicos()
        {
            List<Servico> meusServicos = ServicoRepository.ObterServicosPorTecnico(_funcionario.Id);
            dataGridMeusServicos.ItemsSource = meusServicos;
        }

        private void btnAlterarStatus_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridMeusServicos.SelectedItem is Servico servico)
            {
                var statusDialog = new AlterarStatusDialog(servico.Status);
                if (statusDialog.ShowDialog() == true)
                {
                    servico.Status = statusDialog.NovoStatus;
                    ServicoRepository.AtualizarStatusServico(servico.Id, servico.Status);
                    CarregarMeusServicos();
                    CarregarServicos();
                }
            }

        }

        private void btnConfirmarPagamento_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridMeusServicos.SelectedItem is Servico servico)
            {
                var pagamentoDialog = new ConfirmarPagamentoDialog(servico.ValorPagamento);
                if (pagamentoDialog.ShowDialog() == true)
                {
                    servico.ValorPagamento = pagamentoDialog.ValorPagamento;
                    ServicoRepository.ConfirmarPagamentoServico(servico.Id, servico.ValorPagamento);
                    CarregarMeusServicos();
                    CarregarServicos();
                }
            }
        }

        private void btnAdicionarServico_Click(object sender, RoutedEventArgs e)
        {
            // Permita o botão "Adicionar Serviço" apenas na tela "Meus Serviços" se o funcionário for um técnico
            if (_funcionario.Cargo == Cargo.Técnico && tabControlServicos.SelectedIndex == 1)
            {
                var adicionarServicoDialog = new AdicionarServicoDialog();
                if (adicionarServicoDialog.ShowDialog() == true)
                {
                    var novoServico = adicionarServicoDialog.NovoServico;
                    novoServico.TecnicoResponsavel = _funcionario;
                    ServicoRepository.AdicionarServico(novoServico);
                    CarregarServicos();
                    CarregarMeusServicos();
                }
            }
        }
    }
}
