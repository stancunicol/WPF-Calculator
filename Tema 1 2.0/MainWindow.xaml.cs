using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tema_1.Themes;

namespace Tema_1
{
    public partial class MainWindow : Window
    {
        #region Variables
        bool isOn = Properties.Settings.Default.IsDigitGroupingOn;
        #endregion

        #region Functions
        public MainWindow()
        {
            InitializeComponent();
            InitializeThemeMode();

            this.Icon = new BitmapImage(new Uri("Imagini/cartoon-calculator-icon-png.png", UriKind.Relative));
            this.WindowStyle = WindowStyle.SingleBorderWindow;

            this.DataContext = new ViewModel();

            this.Loaded += Programmer_Loaded;

            this.PreviewKeyDown += MainWindow_PreviewKeyDown;
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
            var viewModel = (DataContext as ViewModel);

            if (viewModel == null) return;

            if ((e.Key == Key.D8 && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))))
            {
                viewModel.AddOperator("*");
            }

            if ((e.Key == Key.D5 && (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))))
            {
                viewModel.AddOperator("%");
            }

            else if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                string digit = e.Key.ToString().Replace("D", "");
                viewModel.AddDigitToNumber(int.Parse(digit));
            }
            else if (e.Key >= Key.A && e.Key <= Key.F)
            {
                string digit = e.Key.ToString();
                viewModel.AddDigitToNumber(int.Parse(digit));
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
        private void Zero_Click(object sender, RoutedEventArgs e) => (DataContext as ViewModel)?.AddDigitToNumber(0);
        private void One_Click(object sender, RoutedEventArgs e) => (DataContext as ViewModel)?.AddDigitToNumber(1);
        private void Two_Click(object sender, RoutedEventArgs e) => (DataContext as ViewModel)?.AddDigitToNumber(2);
        private void Three_Click(object sender, RoutedEventArgs e) => (DataContext as ViewModel)?.AddDigitToNumber(3);
        private void Four_Click(object sender, RoutedEventArgs e) => (DataContext as ViewModel)?.AddDigitToNumber(4);
        private void Five_Click(object sender, RoutedEventArgs e) => (DataContext as ViewModel)?.AddDigitToNumber(5);
        private void Six_Click(object sender, RoutedEventArgs e) => (DataContext as ViewModel)?.AddDigitToNumber(6);
        private void Seven_Click(object sender, RoutedEventArgs e) => (DataContext as ViewModel)?.AddDigitToNumber(7);
        private void Eight_Click(object sender, RoutedEventArgs e) => (DataContext as ViewModel)?.AddDigitToNumber(8);
        private void Nine_Click(object sender, RoutedEventArgs e) => (DataContext as ViewModel)?.AddDigitToNumber(9);
        private void Plus_Click(object sender, RoutedEventArgs e) => (DataContext as ViewModel)?.AddOperator("+");
        private void Minus_Click(object sender, RoutedEventArgs e) => (DataContext as ViewModel)?.AddOperator("-");
        private void Multiply_Click(object sender, RoutedEventArgs e) => (DataContext as ViewModel)?.AddOperator("*");
        private void Divide_Click(object sender, RoutedEventArgs e) => (DataContext as ViewModel)?.AddOperator("/");
        private void Equals_Click(object sender, RoutedEventArgs e) => (DataContext as ViewModel)?.Equals();
        private void Fraction_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as ViewModel);
            if (viewModel != null)
            {
                viewModel.AddOperator("1/x");
            }
        }
        private void XSquare_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as ViewModel);
            if (viewModel != null)
            {
                viewModel.AddOperator("x^2");
            }
        }
        private void SqrtX_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as ViewModel);
            if (viewModel != null)
            {
                viewModel.AddOperator("sqrt(x)");
            }
        }
        private void Modulo_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as ViewModel);
            viewModel?.AddOperator("%");
        }
        private void Opposite_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as ViewModel);
            viewModel?.AddOperator("+/-");
        }
        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as ViewModel);
            viewModel?.Backspace();
        }
        private void CE_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as ViewModel);
            viewModel?.ClearLastNumber();
        }
        private void C_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as ViewModel);
            viewModel?.ClearOperation();
        }
        #endregion

        #region Point

        private void DigitGrouping_Click(object sender, RoutedEventArgs e)
        {
            DigitGroupingWindow dialog = new DigitGroupingWindow(this);
            if (dialog.ShowDialog() == true)
            {
                isOn = dialog.IsDigitGroupingOn ?? false;

                var viewModel = DataContext as ViewModel;
                if (viewModel != null)
                {
                    viewModel.UpdateDigitGrouping(isOn);
                }
            }
        }

        private void CommaButton_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModel viewModel)
            {
                viewModel.AddDecimalPoint();
            }
        }

        #endregion

        #region Settings

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Stancu Nicol-Alexia din Grupa 10LF234.");
        }

        private void MeniuClick(object sender, RoutedEventArgs e)
        {
            MeniuContext.PlacementTarget = sender as Button;
            MeniuContext.IsOpen = true;
        }
        private void Mode_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.Header.ToString() == "Programmer")
            {
                Tema_1.Properties.Settings.Default.IsStandardOn = false;

                Tema_1.Properties.Settings.Default.Save();
                Programmer programmerWindow = new Programmer();
                programmerWindow.Show();

                this.Close();
            }
        }
        #endregion

        #region Stack operations

        private void MPlus_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as ViewModel);
            viewModel?.AddToMemory();
        }
        private void MMinus_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as ViewModel);
            viewModel?.SubtractFromMemory();
        }
        private void MR_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as ViewModel);
            viewModel?.DisplayMemoryValue();
        }
        private void MDown_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as ViewModel);
            viewModel?.DisplayMemoryStack();
            if (viewModel._memoryStack.Count == 0)
            {
                MessageBox.Show("Memory stack is empty.");
                return;
            }
            DialogStack dialog = new DialogStack(viewModel._memoryStack)
            {
                DataContext = viewModel
            };
            dialog.ShowDialog();
        }
        private void MS_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as ViewModel);
            viewModel?.AddToMemoryStack();
        }
        private void MC_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = (DataContext as ViewModel);
            viewModel?.ClearMemory();
        }
        #endregion

        #region Cut, copy, paste
        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModel viewModel)
            {
                viewModel.Paste();
            }
        }
        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModel viewModel)
            {
                viewModel.Copy();
            }
        }
        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ViewModel viewModel)
            {
                viewModel.Cut();
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
                    Properties.Settings.Default.Theme = "LimeGreen";
                    break;
                case "PalePink":
                    ApplyTheme(PalePink.Instance);
                    Properties.Settings.Default.Theme = "PalePink";
                    break;
                case "LavenderViolet":
                    ApplyTheme(LavenderViolet.Instance);
                    break;
                default:
                    ApplyTheme(LavenderViolet.Instance);
                    Properties.Settings.Default.Theme = "LavenderViolet";
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
                Color color = (Color)ColorConverter.ConvertFromString(colorValue);

                Application.Current.Dispatcher.Invoke(() =>
                {
                    if (Application.Current.Resources.Contains(resourceKey))
                    {
                        Application.Current.Resources[resourceKey] = new SolidColorBrush(color);
                    }
                    else
                    {
                        MessageBox.Show($"Resource {resourceKey} not found.");
                    }
                });
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