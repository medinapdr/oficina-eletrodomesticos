using OficinaEletrodomesticos.Models;
using System.Windows;
using System.Windows.Controls;

namespace OficinaEletrodomesticos.View
{
    public partial class AlterarStatusDialog : Window
    {
        public StatusServico NovoStatus { get; private set; }

        public AlterarStatusDialog(StatusServico statusAtual)
        {
            InitializeComponent();
            foreach (ComboBoxItem item in cbStatus.Items)
            {
                if (item.Tag.ToString() == ((int)statusAtual).ToString())
                {
                    cbStatus.SelectedItem = item;
                    break;
                }
            }
        }

        private void Confirmar_Click(object sender, RoutedEventArgs e)
        {
            if (cbStatus.SelectedItem != null)
            {
                int statusId = int.Parse(((ComboBoxItem)cbStatus.SelectedItem).Tag.ToString());
                if (Enum.IsDefined(typeof(StatusServico), statusId))
                {
                    NovoStatus = (StatusServico)statusId;
                    DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Status inválido.");
                }
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }

}
