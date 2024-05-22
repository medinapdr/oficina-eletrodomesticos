using OficinaEletrodomesticos.Data;
using System.Windows;
using System.Windows.Controls;

namespace OficinaEletrodomesticos
{
    public partial class CriarAcessoWindow : Window
    {
        private readonly ConexaoBanco conexaoBanco;

        public CriarAcessoWindow()
        {
            InitializeComponent();
            conexaoBanco = new ConexaoBanco();
        }

        private void CriarUsuario_Click(object sender, RoutedEventArgs e)
        {
            string nomeUsuario = txtNomeUsuario.Text;
            string senha = txtSenha.Password;
            int pessoaId = int.Parse(txtPessoaId.Text);

            bool sucesso = conexaoBanco.CriarUsuario(nomeUsuario, senha, pessoaId);
            if (sucesso)
            {
                MessageBox.Show("Usuário criado com sucesso!");
            }
            else
            {
                MessageBox.Show("Erro ao criar usuário. Por favor, tente novamente.");
            }
        }

        private void CriarPessoa_Click(object sender, RoutedEventArgs e)
        {
            string nome = txtNome.Text;
            string cpf = txtCPF.Text;
            string telefone = txtTelefone.Text;
            string endereco = txtEndereco.Text;
            string tipoPessoa = ((ComboBoxItem)cmbTipoPessoa.SelectedItem).Content.ToString();

            if (tipoPessoa == "Funcionário")
            {
                string cargo = ((ComboBoxItem)cmbCargo.SelectedItem).Content.ToString();
                decimal salario = decimal.Parse(txtSalario.Text);
                string departamento = GetDepartamentoFromCargo(cargo);

                bool sucesso = conexaoBanco.CriarPessoa(nome, cpf, telefone, endereco, tipoPessoa, cargo, salario, departamento);
                if (sucesso)
                {
                    MessageBox.Show("Funcionário criado com sucesso!");
                }
                else
                {
                    MessageBox.Show("Erro ao criar funcionário. Por favor, tente novamente.");
                }
            }
            else
            {
                bool sucesso = conexaoBanco.CriarPessoa(nome, cpf, telefone, endereco, tipoPessoa);
                if (sucesso)
                {
                    MessageBox.Show("Pessoa criada com sucesso!");
                }
                else
                {
                    MessageBox.Show("Erro ao criar pessoa. Por favor, tente novamente.");
                }
            }
        }

        private string GetDepartamentoFromCargo(string cargo)
        {
            switch (cargo)
            {
                case "Vendedor":
                    return "Vendas";
                case "Técnico":
                    return "Serviços";
                case "Gerente":
                    return "Gerência";
                case "Administrador":
                    return "Administração";
                default:
                    return "";
            }
        }

        private void cmbTipoPessoa_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)cmbTipoPessoa.SelectedItem;
            if (selectedItem != null)
            {
                string tipoPessoa = selectedItem.Content.ToString();
                if (tipoPessoa == "Funcionário")
                {
                    cmbCargo.Visibility = Visibility.Visible;
                    txtSalario.Visibility = Visibility.Visible;

                    lblCargo.Visibility = Visibility.Visible;
                    lblSalario.Visibility = Visibility.Visible;

                    txtDepartamento.Visibility = Visibility.Visible;
                    lblDepartamento.Visibility = Visibility.Visible;
                }
                else
                {
                    cmbCargo.Visibility = Visibility.Collapsed;
                    txtSalario.Visibility = Visibility.Collapsed;

                    lblCargo.Visibility = Visibility.Collapsed;
                    lblSalario.Visibility = Visibility.Collapsed;

                    txtDepartamento.Visibility = Visibility.Collapsed;
                    lblDepartamento.Visibility = Visibility.Collapsed;
                }
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
