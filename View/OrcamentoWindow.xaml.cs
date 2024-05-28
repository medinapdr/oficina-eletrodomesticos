using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.View
{
    public partial class OrcamentoWindow : Window
    {
        private Orcamento orcamentoAtual;
        private Dictionary<int, (Peca, int)> pecasSelecionadas;

        public OrcamentoWindow()
        {
            InitializeComponent();
            CarregarDadosIniciais();
            pecasSelecionadas = new Dictionary<int, (Peca, int)>();
        }

        private void CarregarDadosIniciais()
        {
            ClienteComboBox.ItemsSource = OrcamentoRepository.ObterClientes();
            SolicitacaoComboBox.ItemsSource = OrcamentoRepository.ObterSolicitacoes();
            PecasListBox.ItemsSource = OrcamentoRepository.ObterPecas();

            AtualizarListaSolicitacoes();
            AtualizarListaOrcamentos();
        }

        private void AtualizarListaSolicitacoes()
        {
            SolicitacoesListView.ItemsSource = OrcamentoRepository.ObterSolicitacoes();
        }

        private void AtualizarListaOrcamentos()
        {
            OrcamentosListView.ItemsSource = OrcamentoRepository.ObterOrcamentos();
        }

        private void AtualizarValorTotalEPrazo()
        {
            ValorTotalTextBox.Text = orcamentoAtual.ValorTotal.ToString();
            PrazoTextBox.Text = orcamentoAtual.PrazoEntrega.ToString();
        }

        private void PecasListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                foreach (var item in e.AddedItems)
                {
                    if (item is Peca peca)
                    {
                        var container = (ListBoxItem)PecasListBox.ItemContainerGenerator.ContainerFromItem(item);
                        if (container != null)
                        {
                            var stackPanel = FindVisualChild<StackPanel>(container);
                            if (stackPanel != null)
                            {
                                var textBox = stackPanel.Children.OfType<TextBox>().FirstOrDefault();
                                if (textBox != null)
                                {
                                    textBox.IsEnabled = true;
                                }
                            }
                        }
                    }
                }

                foreach (var item in e.RemovedItems)
                {
                    if (item is Peca peca)
                    {
                        var container = (ListBoxItem)PecasListBox.ItemContainerGenerator.ContainerFromItem(item);
                        if (container != null)
                        {
                            var stackPanel = FindVisualChild<StackPanel>(container);
                            if (stackPanel != null)
                            {
                                var textBox = stackPanel.Children.OfType<TextBox>().FirstOrDefault();
                                if (textBox != null)
                                {
                                    textBox.IsEnabled = false;
                                    textBox.Text = "0";
                                }
                            }
                        }

                        pecasSelecionadas.Remove(peca.Id);
                    }
                }

                AtualizarValorPecas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao selecionar a peça: {ex.Message}");
            }
        }

        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    return (T)child;
                }
                else
                {
                    var childOfChild = FindVisualChild<T>(child);
                    if (childOfChild != null)
                    {
                        return childOfChild;
                    }
                }
            }
            return null;
        }



        private void QuantidadeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var textBox = sender as TextBox;
                if (textBox != null)
                {
                    var stackPanel = textBox.Parent as StackPanel;
                    if (stackPanel != null)
                    {
                        var textBlock = stackPanel.Children.OfType<TextBlock>().FirstOrDefault();
                        if (textBlock != null)
                        {
                            var pecaNome = textBlock.Text;
                            var peca = ((List<Peca>)PecasListBox.ItemsSource).FirstOrDefault(p => p.Nome == pecaNome);
                            if (peca != null && int.TryParse(textBox.Text, out int quantidade))
                            {
                                if (quantidade > 0)
                                {
                                    pecasSelecionadas[peca.Id] = (peca, quantidade);
                                }
                                else
                                {
                                    pecasSelecionadas.Remove(peca.Id);
                                }
                            }
                        }
                    }

                    AtualizarValorPecas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao atualizar a quantidade da peça: {ex.Message}");
            }
        }



        private void AtualizarValorPecas()
        {
            try
            {
                decimal valorPecas = 0;
                foreach (var (peca, quantidade) in pecasSelecionadas.Values)
                {
                    valorPecas += peca.Preco * quantidade;
                }
                ValorPecasTextBox.Text = valorPecas.ToString("F2");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao atualizar o valor das peças: {ex.Message}");
            }
        }


        private void CriarOrcamentoButton_Click(object sender, RoutedEventArgs e)
        {
            if (SolicitacaoComboBox.SelectedItem != null && !string.IsNullOrWhiteSpace(ValorTotalTextBox.Text) && !string.IsNullOrWhiteSpace(PrazoTextBox.Text))
            {
                int solicitacaoId = (SolicitacaoComboBox.SelectedItem as SolicitacaoOrcamento).Id;
                decimal valorTotal = decimal.Parse(ValorTotalTextBox.Text);

                // Convert days input to a DateTime representing the deadline
                if (int.TryParse(PrazoTextBox.Text, out int prazoDias))
                {
                    DateTime prazoEntrega = DateTime.Now.AddDays(prazoDias);
                    bool autorizado = AutorizarCheckBox.IsChecked ?? false;

                    var pecasQuantidade = pecasSelecionadas.Values.ToList();

                    bool criacaoSucesso = OrcamentoRepository.CriarOrcamento(solicitacaoId, valorTotal, prazoEntrega, autorizado, pecasQuantidade);

                    if (criacaoSucesso)
                    {
                        MessageBox.Show("Orçamento criado com sucesso!");
                        AtualizarListaOrcamentos();
                    }
                    else
                    {
                        MessageBox.Show("Erro ao criar o orçamento. Por favor, tente novamente.");
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, insira um prazo válido em dias.");
                }
            }
        }



    }
}
