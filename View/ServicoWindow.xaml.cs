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

            // Adicionando manipulador de evento para o evento SelectionChanged do TabControl
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

            // Configurar visibilidade dos botões
            ConfigureButtons();
        }

        private void CarregarServicos()
        {
            listViewServicos.ItemsSource = ServicoRepository.ObterTodosServicos();
        }

        private void CarregarMeusServicos()
        {
            listViewMeusServicos.ItemsSource = ServicoRepository.ObterServicosPorTecnico(_funcionario.Id);
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
            bool isMeusServicosTabSelected = tabControlServicos.SelectedIndex == 1;
            bool isTecnico = _funcionario.Cargo == Cargo.Técnico;

            // Adicionar um novo serviço somente se o funcionário for um técnico na guia "Meus Serviços" ou se não for técnico na guia "Serviços"
            if ((isTecnico && isMeusServicosTabSelected) || (!isTecnico && !isMeusServicosTabSelected))
            {
                var adicionarServicoDialog = new AdicionarServicoDialog(_funcionario);
                if (adicionarServicoDialog.ShowDialog() == true)
                {
                    var novoServico = adicionarServicoDialog.NovoServico;
                    if (isTecnico)
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

        // Método para configurar a visibilidade dos botões com base na guia selecionada e no cargo do funcionário
        private void ConfigureButtons()
        {
            bool isMeusServicosTabSelected = tabControlServicos.SelectedIndex == 1;
            bool isTecnico = _funcionario.Cargo == Cargo.Técnico;

            // O botão "Adicionar Serviço" está disponível apenas para técnicos na guia "Meus Serviços" e para outros cargos na guia "Serviços"
            btnAdicionarServico.IsEnabled = (isTecnico && isMeusServicosTabSelected) || (!isTecnico && !isMeusServicosTabSelected);

            // O botão "Alterar Status" está disponível apenas para técnicos naguia "Meus Serviços", e para administradores e gerentes
            btnAlterarStatus.IsEnabled = isMeusServicosTabSelected && (isTecnico || _funcionario.Cargo == Cargo.Administrador || _funcionario.Cargo == Cargo.Gerente);

            // O botão "Confirmar Pagamento" está disponível para todos os cargos, exceto técnicos
            btnConfirmarPagamento.IsEnabled = _funcionario.Cargo != Cargo.Técnico;
        }
    }
}
