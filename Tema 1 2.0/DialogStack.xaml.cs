using System.Diagnostics;
using System.Windows.Controls;
using System.Windows;

namespace Tema_1
{
    public partial class DialogStack : Window
    {
        #region Variabiles
        public string SelectedNumber { get; private set; }
        private Stack<string> _memoryStack;
        private int SelectedNumberIndex { get; set; } = -1;
        #endregion

        #region Functions
        public DialogStack(Stack<string> memoryStack)
        {
            InitializeComponent();
            _memoryStack = memoryStack;
            LoadMemory();
        }
        private void LoadMemory()
        {
            memoryList.ItemsSource = _memoryStack.ToList();

            Debug.WriteLine("Memory loaded:");
            foreach (var item in _memoryStack)
            {
                Debug.WriteLine(item);
            }
        }
        private void RemoveNumberFromStack(string numberToRemove)
        {
            var stackList = _memoryStack.ToList();

            int indexToRemove = stackList.IndexOf(numberToRemove);

            if (indexToRemove != -1)
            {
                stackList.RemoveAt(indexToRemove);

                if (SelectedNumberIndex != -1)
                {
                    if (indexToRemove < SelectedNumberIndex)
                    {
                        SelectedNumberIndex--;
                    }
                }

                _memoryStack.Clear();
                foreach (var item in stackList)
                {
                    _memoryStack.Push(item);
                }

                Debug.WriteLine("Stack after removal:");
                foreach (var item in _memoryStack)
                {
                    Debug.WriteLine(item);
                }
            }
            else
            {
                MessageBox.Show("Number to remove not found in memory stack.");
            }
        }
        private void RemoveNumber_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SelectedNumber))
            {
                MessageBox.Show("Please select a number first.");
                return;
            }

            RemoveNumberFromStack(SelectedNumber);

            LoadMemory();
            MessageBox.Show($"Number {SelectedNumber} removed from memory stack.");
        }
        private void MemoryAdd_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
            if (viewModel == null)
            {
                MessageBox.Show("ViewModel is not available.");
                return;
            }

            if (string.IsNullOrEmpty(SelectedNumber) || SelectedNumberIndex == -1)
            {
                MessageBox.Show("Please select a number first.");
                return;
            }

            if (double.TryParse(viewModel.CurrentNumber, out double currentNum) &&
                double.TryParse(SelectedNumber, out double selectedNum))
            {
                double result = currentNum + selectedNum;

                ReplaceNumberInStack(SelectedNumberIndex, result.ToString());

                SelectedNumber = result.ToString();

                LoadMemory();
                MessageBox.Show($"Added {selectedNum} to {currentNum}. Result: {result}");
            }
            else
            {
                MessageBox.Show("Invalid number in ViewModel or selected number.");
            }
        }
        private void MemorySubtract_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
            if (viewModel == null)
            {
                MessageBox.Show("ViewModel is not available.");
                return;
            }

            if (string.IsNullOrEmpty(SelectedNumber) || SelectedNumberIndex == -1)
            {
                MessageBox.Show("Please select a number first.");
                return;
            }

            if (double.TryParse(viewModel.CurrentNumber, out double currentNum) &&
                double.TryParse(SelectedNumber, out double selectedNum))
            {
                double result = selectedNum - currentNum;

                ReplaceNumberInStack(SelectedNumberIndex, result.ToString());

                SelectedNumber = result.ToString();

                LoadMemory();
                MessageBox.Show($"Subtracted {selectedNum} from {currentNum}. Result: {result}");
            }
            else
            {
                MessageBox.Show("Invalid number in ViewModel or selected number.");
            }
        }
        private void ReplaceNumberInStack(int index, string newNumber)
        {
            var stackList = _memoryStack.ToList();

            if (index >= 0 && index < stackList.Count)
            {
                stackList[index] = newNumber;
            }

            _memoryStack.Clear();
            for (int i = stackList.Count - 1; i >= 0; i--)
            {
                _memoryStack.Push(stackList[i]);
            }

            Debug.WriteLine("Stack after replacement:");
            foreach (var item in _memoryStack)
            {
                Debug.WriteLine(item);
            }
        }
        private void MemoryList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (memoryList.SelectedItem != null)
            {
                string selectedNumber = memoryList.SelectedItem.ToString();

                SelectedNumber = selectedNumber;

                var stackList = _memoryStack.ToList();
                SelectedNumberIndex = stackList.IndexOf(selectedNumber);

                MessageBox.Show($"Number {selectedNumber} selected. You can now perform operations.");
            }
        }
        #endregion
    }
}