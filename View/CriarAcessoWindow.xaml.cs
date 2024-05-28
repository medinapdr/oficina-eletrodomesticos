using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.View
{
    public partial class CriarAcessoWindow : Window
    {
        public CriarAcessoWindow()
        {
            InitializeComponent();
            CarregarPessoas();
        }

        private void CarregarPessoas()
        {
            List<Pessoa> pessoas = UsuarioRepository.ObterPessoas();
            cmbPessoa.ItemsSource = pessoas;
        }

        private void CriarUsuario_Click(object sender, RoutedEventArgs e)
        {
            string nomeUsuario = txtNomeUsuario.Text;
            string senha = txtSenha.Password;
            int pessoaId = (int)cmbPessoa.SelectedValue;

            bool sucesso = UsuarioRepository.CriarUsuario(nomeUsuario, senha, pessoaId);
            CarregarPessoas();
            MessageBox.Show(sucesso ? "Usuário criado com sucesso!" : "Erro ao criar usuário. Por favor, tente novamente.");
        }

        private void CriarPessoa_Click(object sender, RoutedEventArgs e)
        {
            string nome = txtNome.Text;
            string cpf = txtCPF.Text;
            string telefone = txtTelefone.Text;
            string endereco = txtEndereco.Text;
            string tipoPessoa = ((ComboBoxItem)cmbTipoPessoa.SelectedItem).Content.ToString();

            bool sucesso;
            if (tipoPessoa == "Funcionário")
            {
                string cargo = ((ComboBoxItem)cmbCargo.SelectedItem).Content.ToString();
                decimal salario = decimal.Parse(txtSalario.Text);
                string departamento = GetDepartamentoFromCargo(cargo);

                sucesso = UsuarioRepository.CriarPessoa(nome, cpf, telefone, endereco, tipoPessoa, cargo, salario, departamento);
            }
            else
            {
                sucesso = UsuarioRepository.CriarPessoa(nome, cpf, telefone, endereco, tipoPessoa);
            }

            MessageBox.Show(sucesso ? $"Pessoa {tipoPessoa.ToLower()} criada com sucesso!" : $"Erro ao criar {tipoPessoa.ToLower()}. Por favor, tente novamente.");
        }

        private string GetDepartamentoFromCargo(string cargo)
        {
            return cargo switch
            {
                "Vendedor" => "Vendas",
                "Técnico" => "Serviços",
                "Gerente" or "Administrador" => "Gerência",
                _ => ""
            };
        }

        private void cmbTipoPessoa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)cmbTipoPessoa.SelectedItem;
            if (selectedItem != null)
            {
                string tipoPessoa = selectedItem.Content.ToString();
                bool isFuncionario = tipoPessoa == "Funcionário";

                cmbCargo.Visibility = isFuncionario ? Visibility.Visible : Visibility.Collapsed;
                txtSalario.Visibility = isFuncionario ? Visibility.Visible : Visibility.Collapsed;

                lblCargo.Visibility = isFuncionario ? Visibility.Visible : Visibility.Collapsed;
                lblSalario.Visibility = isFuncionario ? Visibility.Visible : Visibility.Collapsed;

                txtDepartamento.Visibility = isFuncionario ? Visibility.Visible : Visibility.Collapsed;
                lblDepartamento.Visibility = isFuncionario ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void cmbCargo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)cmbCargo.SelectedItem;
            if (selectedItem != null)
            {
                string cargo = selectedItem.Content.ToString();
                if (cargo == "Técnico" || cargo == "Vendedor" || cargo == "Gerente" || cargo == "Administrador")
                {
                    txtDepartamento.Text = GetDepartamentoFromCargo(cargo);
                }
            }
        }
    }
}
