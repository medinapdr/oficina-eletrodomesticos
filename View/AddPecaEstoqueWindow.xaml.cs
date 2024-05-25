using System.Windows;
using OficinaEletrodomesticos.Models;

namespace OficinaEletrodomesticos.View
{
    public partial class AddPecaEstoqueWindow : Window
    {
        private Estoque _estoque;

        public AddPecaEstoqueWindow()
        {
            InitializeComponent();
            _estoque = new Estoque();
        }

        private void AdicionarPeca_Click(object sender, RoutedEventArgs e)
        {
            string nomePeca = txtNomePeca.Text;
            string fabricante = txtFabricante.Text;

            if (string.IsNullOrWhiteSpace(nomePeca))
            {
                MessageBox.Show("Por favor, insira um nome válido para a peça.");
                return;
            }

            if (!decimal.TryParse(txtPreco.Text, out decimal preco) || preco <= 0)
            {
                MessageBox.Show("Por favor, insira um preço válido.");
                return;
            }

            if (!decimal.TryParse(txtLargura.Text, out decimal largura) || largura <= 0)
            {
                MessageBox.Show("Por favor, insira uma largura válida.");
                return;
            }

            if (!decimal.TryParse(txtAltura.Text, out decimal altura) || altura <= 0)
            {
                MessageBox.Show("Por favor, insira uma altura válida.");
                return;
            }

            if (!decimal.TryParse(txtComprimento.Text, out decimal comprimento) || comprimento <= 0)
            {
                MessageBox.Show("Por favor, insira um comprimento válido.");
                return;
            }

            if (!decimal.TryParse(txtPeso.Text, out decimal peso) || peso <= 0)
            {
                MessageBox.Show("Por favor, insira um peso válido.");
                return;
            }

            if (string.IsNullOrWhiteSpace(fabricante))
            {
                MessageBox.Show("Por favor, insira um nome válido para o fabricante.");
                return;
            }

            if (!int.TryParse(txtQuantidade.Text, out int quantidade) || quantidade <= 0)
            {
                MessageBox.Show("Por favor, insira uma quantidade válida.");
                return;
            }

            Peca peca = new Peca(
                nome: nomePeca,
                largura: largura,
                altura: altura,
                comprimento: comprimento,
                peso: peso,
                fabricante: fabricante,
                preco: preco,
                quantidade: quantidade
            );

            _estoque.AdicionarPeca(peca);
        }

        private void LimparCampos_Click(object sender, RoutedEventArgs e)
        {
            txtNomePeca.Text = "";
            txtPreco.Text = "";
            txtLargura.Text = "";
            txtAltura.Text = "";
            txtComprimento.Text = "";
            txtPeso.Text = "";
            txtFabricante.Text = "";
            txtQuantidade.Text = "";
        }

    }
}
