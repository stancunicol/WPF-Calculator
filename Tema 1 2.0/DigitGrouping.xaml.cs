using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tema_1
{
    public partial class DigitGroupingWindow : Window
    {
        #region Variabiles
        public bool? IsDigitGroupingOn { get; private set; }
        #endregion

        #region Functions
        public DigitGroupingWindow(Window owner)
        {
            InitializeComponent();

            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = owner.Left + (owner.Width - this.Width) / 2;
            this.Top = owner.Top + (owner.Height - this.Height) / 2;

            if (IsDigitGroupingOn == true)
            {
                HighlightButton(OnButton);
                UnhighlightButton(OffButton);
            }
            else
            {
                HighlightButton(OffButton);
                UnhighlightButton(OnButton);
            }
        }

        private void OnButton_Click(object sender, RoutedEventArgs e)
        {
            IsDigitGroupingOn = true;
            this.DialogResult = true;

            this.Close();
        }

        private void OffButton_Click(object sender, RoutedEventArgs e)
        {
            IsDigitGroupingOn = false;
            this.DialogResult = true;

            this.Close();
        }
        private void HighlightButton(Button button)
        {
            button.Background = Brushes.LightBlue;
            button.FontWeight = FontWeights.Bold;
        }

        private void UnhighlightButton(Button button)
        {
            button.Background = Brushes.LightGray;
            button.FontWeight = FontWeights.Normal;
        }
        #endregion
    }
}