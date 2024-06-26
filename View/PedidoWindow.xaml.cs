﻿using System.Windows;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.View
{
    public partial class PedidoWindow : Window
    {
        public PedidoWindow()
        {
            InitializeComponent();
            DataContext = this;

            lvPedidos.ItemsSource = PedidoRepository.ConsultarPedidos();
            cmbPeca.ItemsSource = EstoqueRepository.ConsultarEstoque();

            // Vincular eventos de alteração de texto para atualizar o valor total quando os campos de valor unitário ou quantidade forem modificados
            txtQuantidade.TextChanged += (s, e) => AtualizarValorTotal();
            txtValorUnitario.TextChanged += (s, e) => AtualizarValorTotal();
        }

        // Método para calcular e atualizar o valor total com base na quantidade e no valor unitário
        private void AtualizarValorTotal()
        {
            if (decimal.TryParse(txtValorUnitario.Text, out decimal valorUnitario) && int.TryParse(txtQuantidade.Text, out int quantidade))
            {
                txtValorTotal.Text = (valorUnitario * quantidade).ToString("F2");
            }
            else
            {
                txtValorTotal.Clear();
            }
        }

        private void AdicionarPedido_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidarCampos())
            {
                MessageBox.Show("Por favor, preencha todos os campos corretamente.");
                return;
            }

            var peca = cmbPeca.SelectedItem as Peca;
            var quantidade = int.Parse(txtQuantidade.Text);
            var valorUnitario = decimal.Parse(txtValorUnitario.Text);
            var valorTotal = decimal.Parse(txtValorTotal.Text);
            var fornecedor = txtFornecedor.Text;
            var dataCriacao = DateTime.Now;

            var novoPedido = new Pedido
            {
                Peca = new Peca
                {
                    Id = peca.Id,
                    Nome = peca.Nome,
                    Fabricante = peca.Fabricante,
                    Quantidade = quantidade,
                },
                Quantidade = quantidade,
                ValorUnitario = valorUnitario,
                ValorTotal = valorTotal,
                Fornecedor = fornecedor,
                DataCriacao = dataCriacao
            };

            PedidoRepository.AdicionarPedido(novoPedido);
            AtualizarDataGridPedidos();
        }

        private bool ValidarCampos()
        {
            return cmbPeca.SelectedItem is Peca peca &&
                   !string.IsNullOrWhiteSpace(txtQuantidade.Text) &&
                   int.TryParse(txtQuantidade.Text, out _) &&
                   !string.IsNullOrWhiteSpace(txtValorUnitario.Text) &&
                   decimal.TryParse(txtValorUnitario.Text, out _) &&
                   !string.IsNullOrWhiteSpace(txtValorTotal.Text) &&
                   decimal.TryParse(txtValorTotal.Text, out _) &&
                   !string.IsNullOrWhiteSpace(txtFornecedor.Text);
        }

        private void AtualizarDataGridPedidos()
        {
            lvPedidos.ItemsSource = PedidoRepository.ConsultarPedidos();
        }

        private void ConfirmarRecebimento_Click(object sender, RoutedEventArgs e)
        {
            if (lvPedidos.SelectedItem is Pedido pedido)
            {
                if (PedidoRepository.ConfirmarRecebimentoPedido(pedido.Id, DateTime.Now))
                {
                    AtualizarDataGridPedidos();
                }
                else
                {
                    MessageBox.Show("Esse pedido já foi recebido.");
                }
            }
            else
            {
                MessageBox.Show("Selecione um pedido para confirmar o recebimento.");
            }
        }
    }
}
