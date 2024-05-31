using System.Windows;
using OficinaEletrodomesticos.Data;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.View
{
    public partial class AddPecaEstoqueWindow : Window
    {
        private readonly Estoque _estoque;

        public AddPecaEstoqueWindow()
        {
            InitializeComponent();
            _estoque = new Estoque();
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

            if (!string.IsNullOrWhiteSpace(txtLargura.Text) && (!decimal.TryParse(txtLargura.Text, out decimal largura) || largura <= 0))
            {
                ShowErrorMessage("Por favor, insira uma largura válida.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtAltura.Text) && (!decimal.TryParse(txtAltura.Text, out decimal altura) || altura <= 0))
            {
                ShowErrorMessage("Por favor, insira uma altura válida.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtComprimento.Text) && (!decimal.TryParse(txtComprimento.Text, out decimal comprimento) || comprimento <= 0))
            {
                ShowErrorMessage("Por favor, insira um comprimento válido.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtPeso.Text) && (!decimal.TryParse(txtPeso.Text, out decimal peso) || peso <= 0))
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
            decimal? largura = string.IsNullOrWhiteSpace(txtLargura.Text) ? (decimal?)null : decimal.Parse(txtLargura.Text);
            decimal? altura = string.IsNullOrWhiteSpace(txtAltura.Text) ? (decimal?)null : decimal.Parse(txtAltura.Text);
            decimal? comprimento = string.IsNullOrWhiteSpace(txtComprimento.Text) ? (decimal?)null : decimal.Parse(txtComprimento.Text);
            decimal? peso = string.IsNullOrWhiteSpace(txtPeso.Text) ? (decimal?)null : decimal.Parse(txtPeso.Text);

            return new Peca
            {
                Nome = txtNomePeca.Text,
                Largura = largura,
                Altura = altura,
                Comprimento = comprimento,
                Peso = peso,
                Fabricante = txtFabricante.Text,
                Preco = decimal.Parse(txtPreco.Text),
                Quantidade = int.Parse(txtQuantidade.Text)
            };
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
