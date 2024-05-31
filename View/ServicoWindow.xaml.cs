using System.Linq;
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

            tabControlServicos.SelectionChanged += TabControlServicos_SelectionChanged;

            if (_funcionario.Cargo != Cargo.Técnico)
            {
                var tabItemMeusServicos = tabControlServicos.Items.Cast<TabItem>().FirstOrDefault(item => item.Header.ToString() == "Meus Servicos");
                if (tabItemMeusServicos != null)
                {
                    tabItemMeusServicos.IsEnabled = false;
                }
            }

            if (_funcionario.Cargo == Cargo.Vendedor)
            {
                btnAlterarStatus.IsEnabled = false;
            }

            ConfigureButtons();
        }

        private void CarregarServicos()
        {
            List<Servico> servicos = ServicoRepository.ObterTodosServicos();
            listViewServicos.ItemsSource = servicos;
        }

        private void CarregarMeusServicos()
        {
            List<Servico> meusServicos = ServicoRepository.ObterServicosPorTecnico(_funcionario.Id);
            listViewMeusServicos.ItemsSource = meusServicos;
        }

        private void btnAlterarStatus_Click(object sender, RoutedEventArgs e)
        {
            ListView listView = tabControlServicos.SelectedIndex == 1 ? listViewMeusServicos : listViewServicos;
            if (listView.SelectedItem is Servico servico)
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
            ListView listView = tabControlServicos.SelectedIndex == 1 ? listViewMeusServicos : listViewServicos;
            if (listView.SelectedItem is Servico servico)
            {
                if (servico.ValorPagamento != null)
                {
                    MessageBoxResult result = MessageBox.Show("Este serviço já foi pago. Tem certeza que deseja alterar o pagamento?", "Confirmar Alteração de Pagamento", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result != MessageBoxResult.Yes)
                    {
                        return;
                    }
                }

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
            if ((_funcionario.Cargo == Cargo.Técnico && tabControlServicos.SelectedIndex == 1) ||
                (_funcionario.Cargo != Cargo.Técnico && tabControlServicos.SelectedIndex == 0))
            {
                var adicionarServicoDialog = new AdicionarServicoDialog(_funcionario);
                if (adicionarServicoDialog.ShowDialog() == true)
                {
                    var novoServico = adicionarServicoDialog.NovoServico;
                    if (_funcionario.Cargo == Cargo.Técnico)
                    {
                        novoServico.TecnicoResponsavel = _funcionario;
                    }
                    ServicoRepository.AdicionarServico(novoServico);
                    CarregarServicos();
                    CarregarMeusServicos();
                }
            }
        }

        private void TabControlServicos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ConfigureButtons();
        }

        private void ConfigureButtons()
        {
            bool isMeusServicosTabSelected = tabControlServicos.SelectedIndex == 1;
            btnAdicionarServico.IsEnabled = (_funcionario.Cargo == Cargo.Técnico && isMeusServicosTabSelected) ||
                                            (_funcionario.Cargo != Cargo.Técnico && !isMeusServicosTabSelected);
            btnAlterarStatus.IsEnabled = isMeusServicosTabSelected && (_funcionario.Cargo == Cargo.Técnico ||
                                            _funcionario.Cargo == Cargo.Administrador || _funcionario.Cargo == Cargo.Gerente);
            btnConfirmarPagamento.IsEnabled = _funcionario.Cargo != Cargo.Técnico;
        }
    }
}
