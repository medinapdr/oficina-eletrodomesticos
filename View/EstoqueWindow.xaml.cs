using System.Windows;
using System.Windows.Controls;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.View
{
    public partial class EstoqueWindow : Window
    {
        private readonly Estoque _estoque;
        private string _cargo;
        private List<Peca> _pecasEstoque;
        private Peca _pecaSelecionada;

        public EstoqueWindow(string cargo)
        {
            InitializeComponent();
            _estoque = new Estoque();
            _cargo = cargo;

            if (cargo != "Gerente" && cargo != "Administrador")
            {
                btnInserir.IsEnabled = false;
                btnRemover.IsEnabled = false;
            }
            AtualizarListaEstoque();
        }

        private void AtualizarListaEstoque()
        {
            _pecasEstoque = _estoque.ConsultarEstoque();
            listViewEstoque.ItemsSource = _pecasEstoque;
            listViewEstoque.Items.Refresh();
        }

        private void Editar_Click(object sender, RoutedEventArgs e)
        {
            if (_pecaSelecionada == null)
            {
                MessageBox.Show("Selecione uma pe�a para editar.");
                return;
            }
            MessageBox.Show($"Editar pe�a: {_pecaSelecionada.Nome}");
        }

        private void Remover_Click(object sender, RoutedEventArgs e)
        {
            if (_pecaSelecionada == null)
            {
                MessageBox.Show("Selecione uma pe�a para remover.");
                return;
            }

            MessageBoxResult resultado = MessageBox.Show(
                $"Tem certeza que deseja remover a pe�a '{_pecaSelecionada.Nome}'?", 
                "Remover Pe�a", 
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

        private void ListViewEstoque_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _pecaSelecionada = listViewEstoque.SelectedItem as Peca;
        }
    }
}
