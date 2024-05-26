using System.Windows;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.View
{
    public partial class AddPecaEstoqueWindow : Window
    {
        private Estoque _estoque;

        public AddPecaEstoqueWindow(ConexaoBanco conexaoBanco)
        {
            InitializeComponent();
            _estoque = new Estoque(conexaoBanco);
        }

        private void AdicionarPeca_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateFields())
            {
                Peca peca = CriarPeca();
                _estoque.AdicionarPeca(peca);
                LimparCampos();
            }
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(txtNomePeca.Text))
            {
                ShowErrorMessage("Por favor, insira um nome válido para a peça.");
                return false;
            }

            if (!decimal.TryParse(txtPreco.Text, out decimal preco) || preco <= 0)
            {
                ShowErrorMessage("Por favor, insira um preço válido.");
                return false;
            }

            if (!decimal.TryParse(txtLargura.Text, out decimal largura) || largura <= 0)
            {
                ShowErrorMessage("Por favor, insira uma largura válida.");
                return false;
            }

            if (!decimal.TryParse(txtAltura.Text, out decimal altura) || altura <= 0)
            {
                ShowErrorMessage("Por favor, insira uma altura válida.");
                return false;
            }

            if (!decimal.TryParse(txtComprimento.Text, out decimal comprimento) || comprimento <= 0)
            {
                ShowErrorMessage("Por favor, insira um comprimento válido.");
                return false;
            }

            if (!decimal.TryParse(txtPeso.Text, out decimal peso) || peso <= 0)
            {
                ShowErrorMessage("Por favor, insira um peso válido.");
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFabricante.Text))
            {
                ShowErrorMessage("Por favor, insira um nome válido para o fabricante.");
                return false;
            }

            if (!int.TryParse(txtQuantidade.Text, out int quantidade) || quantidade <= 0)
            {
                ShowErrorMessage("Por favor, insira uma quantidade válida.");
                return false;
            }

            return true;
        }

        private void LimparCampos_Click(object sender, RoutedEventArgs e)
        {
            LimparCampos();
        }

        private Peca CriarPeca()
        {
            return new Peca(
                nome: txtNomePeca.Text,
                largura: decimal.Parse(txtLargura.Text),
                altura: decimal.Parse(txtAltura.Text),
                comprimento: decimal.Parse(txtComprimento.Text),
                peso: decimal.Parse(txtPeso.Text),
                fabricante: txtFabricante.Text,
                preco: decimal.Parse(txtPreco.Text),
                quantidade: int.Parse(txtQuantidade.Text)
            );
        }

        private void LimparCampos()
        {
            txtNomePeca.Clear();
            txtPreco.Clear();
            txtLargura.Clear();
            txtAltura.Clear();
            txtComprimento.Clear();
            txtPeso.Clear();
            txtFabricante.Clear();
            txtQuantidade.Clear();
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
