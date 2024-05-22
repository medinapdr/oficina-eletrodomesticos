using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace OficinaEletrodomesticos
{
    public partial class EstoqueWindow : Window
    {
        private readonly Estoque _estoque;
        private List<Peca> _pecasEstoque;
        private Peca _pecaSelecionada;

        public EstoqueWindow()
        {
            InitializeComponent();
            _estoque = new Estoque();
            AtualizarListaEstoque();
        }

        private void AtualizarListaEstoque()
        {
            _pecasEstoque = _estoque.ConsultarEstoque();
            dataGridEstoque.ItemsSource = _pecasEstoque;
            dataGridEstoque.Items.Refresh();
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (_pecaSelecionada != null)
            {
                MessageBox.Show($"Editar pe�a: {_pecaSelecionada.Nome}");
            }
            else
            {
                MessageBox.Show("Selecione uma pe�a para editar.");
            }
        }

        private void Remover_Click(object sender, RoutedEventArgs e)
        {
            if (_pecaSelecionada != null)
            {
                MessageBoxResult resultado = MessageBox.Show($"Tem certeza que deseja remover a pe�a '{_pecaSelecionada.Nome}'?", "Remover Pe�a", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (resultado == MessageBoxResult.Yes)
                {
                    // Remove the part from the inventory here
                    _estoque.RemoverPeca(_pecaSelecionada);
                    AtualizarListaEstoque();
                }
            }
            else
            {
                MessageBox.Show("Selecione uma pe�a para remover.");
            }
        }

        private void InserirPe�as_Click(object sender, RoutedEventArgs e)
        {
            AdicionarPecaAoEstoqueWindow adicionarPecaWindow = new AdicionarPecaAoEstoqueWindow();
            adicionarPecaWindow.ShowDialog();
            AtualizarListaEstoque();
        }

        private void DataGridEstoque_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridEstoque.SelectedItem != null)
            {
                _pecaSelecionada = (Peca)dataGridEstoque.SelectedItem;
            }
        }
    }
}
