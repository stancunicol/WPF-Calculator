using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System.Diagnostics;
using System.Windows.Media;
using Tema_1.Themes;
using System.Windows.Media.Imaging;

namespace Tema_1
{
    public partial class Programmer
    {
        #region Variables
        bool isOn = Properties.Settings.Default.IsDigitGroupingOn;
        #endregion

        #region Functions
        public Programmer()
        {
            InitializeComponent();
            InitializeThemeMode();

            this.Loaded += Programmer_Loaded;

            this.PreviewKeyDown += MainWindow_PreviewKeyDown;

            var viewModel = new ProgrammerFunctions();
            DataContext = viewModel;

            this.Icon = new BitmapImage(new Uri("Imagini/cartoon-calculator-icon-png.png", UriKind.Relative));

            if (viewModel != null)
            {
                viewModel.IsHexOn = Properties.Settings.Default.IsHexOn;
                viewModel.IsDecOn = Properties.Settings.Default.IsDecOn;
                viewModel.IsOctOn = Properties.Settings.Default.IsOctOn;
                viewModel.IsBinOn = Properties.Settings.Default.IsBinOn;

                viewModel.NotifyPropertyChanged(nameof(viewModel.HexBox));
                viewModel.NotifyPropertyChanged(nameof(viewModel.DecBox));
                viewModel.NotifyPropertyChanged(nameof(viewModel.OctBox));
                viewModel.NotifyPropertyChanged(nameof(viewModel.BinBox));
                viewModel.NotifyPropertyChanged(nameof(viewModel.Result));
            }
        }
        private void Programmer_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
        }
        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = false;
        }
        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = (DataContext as ProgrammerFunctions);

            if (viewModel == null) return;

            if ((e.Key == Key.D8 && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))))
            {
                viewModel.AddOperator("*");
            }

            else if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                string digit = e.Key.ToString().Replace("D", "");
                viewModel.AddDigitToNumber(digit);
            }
            else if (e.Key >= Key.A && e.Key <= Key.F)
            {
                string digit = e.Key.ToString();
                viewModel.AddDigitToNumber(digit);
            }
            else if (e.Key == Key.Add || e.Key == Key.OemPlus)
            {
                viewModel.AddOperator("+");
            }
            else if (e.Key == Key.Subtract || e.Key == Key.OemMinus)
            {
                viewModel.AddOperator("-");
            }
            else if (e.Key == Key.Divide || e.Key == Key.OemQuestion)
            {
                viewModel.AddOperator("/");
            }
            else if (e.Key == Key.Enter)
            {
                viewModel.Equals();
            }
            else if (e.Key == Key.Back)
            {
                viewModel.Backspace();
            }
            else if (e.Key == Key.Escape)
            {
                viewModel.ClearOperation();
            }

            e.Handled = true;
        }

        #region Operations
        private void Zero_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.AddDigitToNumber("0");
        private void One_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.AddDigitToNumber("1");
        private void Two_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.AddDigitToNumber("2");
        private void Three_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.AddDigitToNumber("3");
        private void Four_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.AddDigitToNumber("4");
        private void Five_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.AddDigitToNumber("5");
        private void Six_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.AddDigitToNumber("6");
        private void Seven_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.AddDigitToNumber("7");
        private void Eight_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.AddDigitToNumber("8");
        private void Nine_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.AddDigitToNumber("9");
        private void A_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.AddDigitToNumber("A");
        private void B_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.AddDigitToNumber("B");
        private void C_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.AddDigitToNumber("C");
        private void D_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.AddDigitToNumber("D");
        private void E_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.AddDigitToNumber("E");
        private void F_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.AddDigitToNumber("F");
        private void Plus_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.AddOperator("+");
        private void Minus_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.AddOperator("-");
        private void Multiply_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.AddOperator("*");
        private void Divide_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.AddOperator("/");
        private void Equals_Click(object sender, RoutedEventArgs e) => (DataContext as ProgrammerFunctions)?.Equals();
        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as ProgrammerFunctions);
            viewModel?.Backspace();
        }
        private void CE_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as ProgrammerFunctions);
            viewModel?.ClearOperation();
        }
        private void Modulo_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as ProgrammerFunctions);
            viewModel?.AddOperator("%");
        }
        private void Opposite_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as ProgrammerFunctions);
            viewModel?.AddOperator("+/-");
        }
        #endregion

        #region Cut, copy, paste
        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProgrammerFunctions viewModel)
            {
                viewModel.Cut();
            }
        }
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProgrammerFunctions viewModel)
            {
                viewModel.Copy();
            }
        }
        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProgrammerFunctions viewModel)
            {
                viewModel.Paste();
            }
        }
        #endregion

        #region Settings
        private void MeniuClick(object sender, RoutedEventArgs e)
        {
            MeniuContext.PlacementTarget = sender as Button;
            MeniuContext.IsOpen = true;
        }
        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Stancu Nicol-Alexia din Grupa 10LF234.");
        }
        private void DigitGrouping_Click(object sender, RoutedEventArgs e)
        {
            DigitGroupingWindow dialog = new DigitGroupingWindow(this);
            if (dialog.ShowDialog() == true)
            {
                isOn = dialog.IsDigitGroupingOn ?? false;

                var viewModel = DataContext as ProgrammerFunctions;
                if (viewModel != null)
                {
                    viewModel.UpdateDigitGrouping(isOn);
                    viewModel.NotifyPropertyChanged(nameof(viewModel.DecBox));
                }
            }
        }
        private void Mode_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.Header.ToString() == "Standard")
            {

                Tema_1.Properties.Settings.Default.IsStandardOn = true; // sau true

                Tema_1.Properties.Settings.Default.Save();
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                this.Close();
            }
        }

        #endregion

        #region Base operations
        private void HEX_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ProgrammerFunctions;
            Properties.Settings.Default.IsHexOn = true;
            Properties.Settings.Default.IsDecOn = false;
            Properties.Settings.Default.IsOctOn = false;
            Properties.Settings.Default.IsBinOn = false;
            Properties.Settings.Default.Save();
            viewModel.IsHexOn = true;
            viewModel.IsDecOn = false;
            viewModel.IsOctOn = false;
            viewModel.IsBinOn = false;
            viewModel.CurrentNumber = "0";
            viewModel.PreviousNumber = "0";
            viewModel.Result.Subtotal = "0";

            viewModel.NotifyPropertyChanged(nameof(viewModel.HexBox));
            viewModel.NotifyPropertyChanged(nameof(viewModel.DecBox));
            viewModel.NotifyPropertyChanged(nameof(viewModel.OctBox));
            viewModel.NotifyPropertyChanged(nameof(viewModel.BinBox));
            viewModel.NotifyPropertyChanged(nameof(viewModel.Result));
        }

        private void DEC_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ProgrammerFunctions;
            Properties.Settings.Default.IsHexOn = false;
            Properties.Settings.Default.IsDecOn = true;
            Properties.Settings.Default.IsOctOn = false;
            Properties.Settings.Default.IsBinOn = false;
            Properties.Settings.Default.Save();
            viewModel.IsHexOn = false;
            viewModel.IsDecOn = true;
            viewModel.IsOctOn = false;
            viewModel.IsBinOn = false;
            viewModel.CurrentNumber = "0";
            viewModel.PreviousNumber = "0";
            viewModel.Result.Subtotal = "0";

            viewModel.NotifyPropertyChanged(nameof(viewModel.HexBox));
            viewModel.NotifyPropertyChanged(nameof(viewModel.DecBox));
            viewModel.NotifyPropertyChanged(nameof(viewModel.OctBox));
            viewModel.NotifyPropertyChanged(nameof(viewModel.BinBox));
            viewModel.NotifyPropertyChanged(nameof(viewModel.Result));
        }

        private void OCT_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ProgrammerFunctions;
            Properties.Settings.Default.IsHexOn = false;
            Properties.Settings.Default.IsDecOn = false;
            Properties.Settings.Default.IsOctOn = true;
            Properties.Settings.Default.IsBinOn = false;
            Properties.Settings.Default.Save();
            viewModel.IsHexOn = false;
            viewModel.IsDecOn = false;
            viewModel.IsOctOn = true;
            viewModel.IsBinOn = false;
            viewModel.CurrentNumber = "0";
            viewModel.PreviousNumber = "0";
            viewModel.Result.Subtotal = "0";

            viewModel.NotifyPropertyChanged(nameof(viewModel.HexBox));
            viewModel.NotifyPropertyChanged(nameof(viewModel.DecBox));
            viewModel.NotifyPropertyChanged(nameof(viewModel.OctBox));
            viewModel.NotifyPropertyChanged(nameof(viewModel.BinBox));
            viewModel.NotifyPropertyChanged(nameof(viewModel.Result));
        }

        private void BIN_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ProgrammerFunctions;
            Properties.Settings.Default.IsHexOn = false;
            Properties.Settings.Default.IsDecOn = false;
            Properties.Settings.Default.IsOctOn = false;
            Properties.Settings.Default.IsBinOn = true;
            Properties.Settings.Default.Save();
            viewModel.IsHexOn = false;
            viewModel.IsDecOn = false;
            viewModel.IsOctOn = false;
            viewModel.IsBinOn = true;
            viewModel.CurrentNumber = "0";
            viewModel.PreviousNumber = "0";
            viewModel.Result.Subtotal = "0";

            viewModel.NotifyPropertyChanged(nameof(viewModel.HexBox));
            viewModel.NotifyPropertyChanged(nameof(viewModel.DecBox));
            viewModel.NotifyPropertyChanged(nameof(viewModel.OctBox));
            viewModel.NotifyPropertyChanged(nameof(viewModel.BinBox));
            viewModel.NotifyPropertyChanged(nameof(viewModel.Result));
        }
        #endregion

        #region Stack operations 
        private void MS_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as ProgrammerFunctions);
            viewModel?.AddToMemoryStack();
        }
        private void MDown_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as ProgrammerFunctions);
            viewModel?.DisplayMemoryStack();
            if (viewModel._memoryStack.Count == 0)
            {
                MessageBox.Show("Memory stack is empty.");
                return;
            }
        }
        #endregion

        #region Theme Mode
        private void ThemeModeSelector(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem == null) return;

            string themeName = menuItem.Header.ToString();

            switch (themeName)
            {
                case "Lime Green":
                    ApplyTheme(LimeGreen.Instance);
                    Properties.Settings.Default.Theme = "LimeGreen";
                    break;
                case "Pale Pink":
                    ApplyTheme(PalePink.Instance);
                    Properties.Settings.Default.Theme = "PalePink";
                    break;
                case "Lavender Violet":
                    ApplyTheme(LavenderViolet.Instance);
                    Properties.Settings.Default.Theme = "LavenderViolet";
                    break;
                default:
                    break;
            }
            Properties.Settings.Default.Save();
        }

        private void ApplyTheme(ThemeMode theme)
        {
            Debug.WriteLine($"Applying theme: {theme.GetType().Name}");
            Debug.WriteLine($"MenuBackgroundColor: {theme.MenuBackgroundColor}");
            Debug.WriteLine($"BackgroundColor: {theme.BackgroundColor}");

            UpdateResource("MenuBackgroundColor", theme.MenuBackgroundColor);
            UpdateResource("MenuItemBackgroundColor", theme.MenuItemBackgroundColor);
            UpdateResource("MenuItemForegroundColor", theme.MenuItemForegroundColor);
            UpdateResource("BackgroundColor", theme.BackgroundColor);
            UpdateResource("CanvasBackgroundColor", theme.CanvasBackgroundColor);
            UpdateResource("CanvasBorderBrush", theme.CanvasBorderBrush);
            UpdateResource("TextForeground", theme.TextForeground);
            UpdateResource("ButtonBackgroundColor", theme.ButtonBackgroundColor);
            UpdateResource("ButtonForegroundColor", theme.ButtonForegroundColor);
            UpdateResource("ButtonBorderBrush", theme.ButtonBorderBrush);
        }
        private void InitializeThemeMode()
        {
            string savedTheme = Properties.Settings.Default.Theme;

            if (string.IsNullOrEmpty(savedTheme))
            {
                savedTheme = "LavenderViolet";
                Properties.Settings.Default.Theme = savedTheme;
                Properties.Settings.Default.Save();
            }

            switch (savedTheme)
            {
                case "LimeGreen":
                    ApplyTheme(LimeGreen.Instance);
                    break;
                case "PalePink":
                    ApplyTheme(PalePink.Instance);
                    break;
                case "LavenderViolet":
                    ApplyTheme(LavenderViolet.Instance);
                    break;
                default:
                    ApplyTheme(LavenderViolet.Instance);
                    break;
            }
            Properties.Settings.Default.Save();
        }
        private void UpdateResource(string resourceKey, string colorValue)
        {
            if (string.IsNullOrEmpty(colorValue))
            {
                MessageBox.Show($"Color value for {resourceKey} is null or empty.");
                return;
            }

            try
            {
                var color = (Color)ColorConverter.ConvertFromString(colorValue);
                var brush = new SolidColorBrush(color);

                this.Resources[resourceKey] = brush;
                Application.Current.Resources[resourceKey] = brush;

                Debug.WriteLine($"Updated {resourceKey} to {colorValue}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update resource {resourceKey}: {ex.Message}");
            }
        }
        #endregion

        #endregion
    }
}