using System.Windows;
using System.Windows.Controls;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.View
{
    public partial class EstoqueWindow : Window
    {
        private string _cargo; // Armazena o cargo do usuário logado
        private Peca _pecaSelecionada; // Armazena a peça selecionada na lista

        public EstoqueWindow(string cargo)
        {
            InitializeComponent();
            _cargo = cargo;

            // Desabilita os botões de inserir e remover se o usuário não for Gerente ou Administrador
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
                EstoqueRepository.RemoverPeca(_pecaSelecionada);
                MessageBox.Show($"Peça {_pecaSelecionada.Nome} removida do estoque.");
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
