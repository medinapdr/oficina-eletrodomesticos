using System.Windows;
using System.Windows.Controls;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.View
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
            if (_pecaSelecionada == null)
            {
                MessageBox.Show("Selecione uma peça para editar.");
                return;
            }
            MessageBox.Show($"Editar peça: {_pecaSelecionada.Nome}");
        }

        private void Remover_Click(object sender, RoutedEventArgs e)
        {
            if (_pecaSelecionada == null)
            {
                MessageBox.Show("Selecione uma peça para remover.");
                return;
            }

            MessageBoxResult resultado = MessageBox.Show(
                $"Tem certeza que deseja remover a peça '{_pecaSelecionada.Nome}'?", 
                "Remover Peça", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Question
            );

            if (resultado == MessageBoxResult.Yes)
            {
                _estoque.RemoverPeca(_pecaSelecionada);
                AtualizarListaEstoque();
            }
        }

        private void InserirPecas_Click(object sender, RoutedEventArgs e)
        {
            new AddPecaEstoqueWindow().ShowDialog();
            AtualizarListaEstoque();
        }

        private void DataGridEstoque_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _pecaSelecionada = dataGridEstoque.SelectedItem as Peca;
        }
    }
}
