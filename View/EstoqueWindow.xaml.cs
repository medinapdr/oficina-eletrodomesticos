using System.Windows;
using System.Windows.Controls;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.View
{
    public partial class EstoqueWindow : Window
    {
        private string _cargo; // Armazena o cargo do usu�rio logado
        private Peca _pecaSelecionada; // Armazena a pe�a selecionada na lista

        public EstoqueWindow(string cargo)
        {
            InitializeComponent();
            _cargo = cargo;

            // Desabilita os bot�es de inserir e remover se o usu�rio n�o for Gerente ou Administrador
            if (cargo != "Gerente" && cargo != "Administrador")
            {
                btnInserir.IsEnabled = false;
                btnRemover.IsEnabled = false;
            }

            // Atualiza a lista de estoque ao abrir a janela
            AtualizarListaEstoque();
        }

        private void AtualizarListaEstoque()
        {
            listViewEstoque.ItemsSource = EstoqueRepository.ConsultarEstoque();
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
                EstoqueRepository.RemoverPeca(_pecaSelecionada);
                MessageBox.Show($"Pe�a {_pecaSelecionada.Nome} removida do estoque.");
                AtualizarListaEstoque();
            }
        }

        private void InserirPecas_Click(object sender, RoutedEventArgs e)
        {
            new AdicionarPecaDialog().ShowDialog();
            AtualizarListaEstoque();
        }

        private void ListViewEstoque_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _pecaSelecionada = listViewEstoque.SelectedItem as Peca;
        }
    }
}
