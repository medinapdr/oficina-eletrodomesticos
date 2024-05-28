using System.Collections.Generic;
using System.Windows;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.View
{
    public partial class AdicionarServicoDialog : Window
    {
        public Servico NovoServico { get; private set; }

        public AdicionarServicoDialog(Funcionario funcionario)
        {
            InitializeComponent();
            CarregarTecnicos(funcionario);
        }

        private void CarregarTecnicos(Funcionario funcionario)
        {
            if (funcionario.Cargo == Cargo.Técnico)
            {
                cmbTecnico.Items.Add(funcionario.Nome);
                cmbTecnico.SelectedIndex = 0;
                cmbTecnico.IsEnabled = false;
            }
            else
            {
                var tecnicos = ServicoRepository.ObterTecnicos();
                cmbTecnico.ItemsSource = tecnicos;
                cmbTecnico.DisplayMemberPath = "Nome";
                cmbTecnico.SelectedValuePath = "Id";
            }
        }

        private void Adicionar_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txtOrcamentoId.Text, out int orcamentoId) &&
                !string.IsNullOrWhiteSpace(txtDescricao.Text) &&
                cmbTecnico.SelectedItem != null)
            {
                var tecnicoId = (cmbTecnico.SelectedItem as Funcionario)?.Id ?? 0;
                NovoServico = new Servico
                {
                    Orcamento = new Orcamento { Id = orcamentoId },
                    Descricao = txtDescricao.Text,
                    Status = StatusServico.Parado,
                    TecnicoResponsavel = new Funcionario { Id = tecnicoId }
                };
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Por favor, insira todos os dados necessários.");
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
