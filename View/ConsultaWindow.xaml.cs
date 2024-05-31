using OficinaEletrodomesticos.Models;
using OficinaEletrodomesticos.Data;
using System.Windows;

namespace OficinaEletrodomesticos.View
{
    public partial class ConsultaWindow : Window
    {
        private readonly Pessoa _pessoa;

        public ConsultaWindow(Pessoa pessoa)
        {
            InitializeComponent();
            _pessoa = pessoa;

            if (_pessoa is Cliente cliente)
            {
                ClientesComboBox.Visibility = Visibility.Collapsed;
                LoadClienteData(cliente);
                NomeClienteLabel.Content = cliente.Nome;
                OrcamentosButtonPanel.Visibility = Visibility.Visible;
            }
            else if (_pessoa is Funcionario)
            {
                LoadFuncionariosClientes();
                OrcamentosButtonPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void LoadClienteData(Cliente cliente)
        {
            SolicitacoesListView.ItemsSource = ConsultaRepository.ObterSolicitacoes(cliente.Id);
            OrcamentosListView.ItemsSource = ConsultaRepository.ObterOrcamentos(cliente.Id);
            ServicosListView.ItemsSource = ConsultaRepository.ObterServicos(cliente.Id);
        }

        private void LoadFuncionariosClientes()
        {
            ClientesComboBox.ItemsSource = ConsultaRepository.ObterClientes();
        }

        private void ClientesComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ClientesComboBox.SelectedItem is Cliente selectedCliente)
            {
                LoadClienteData(selectedCliente);
            }
        }

        private void AutorizarOrcamentoButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrcamentosListView.SelectedItem is Orcamento selectedOrcamento)
            {
                if(selectedOrcamento.Autorizado != false)
                {
                    MessageBox.Show("Orçamento já autorizado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                bool success = OrcamentoRepository.AutorizarOrcamento(selectedOrcamento.Id);

                if (success)
                {
                    MessageBox.Show("Orçamento autorizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadClienteData(_pessoa as Cliente);
                }
                else
                {
                    MessageBox.Show("Falha ao autorizar o orçamento.", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Selecione um orçamento para autorizar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
