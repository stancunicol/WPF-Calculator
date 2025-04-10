using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace Tema_1
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region Variables
        public bool IsDigitGroupingOn { get; set; } = Properties.Settings.Default.IsDigitGroupingOn;
        public event PropertyChangedEventHandler PropertyChanged;
        private Result _result;
        public string CurrentNumber { get; set; } = "0";
        public string FirstNumber { get; set; } = "0";
        public string SecondNumber { get; set; } = "0";
        public string LastOperator { get; set; } = "";
        public string PreviousNumber { get; set; } = "0";
        public string CurrentOperator { get; set; } = "";
        public bool IsNewEntry { get; set; } = true;
        public Stack<string> _memoryStack = new Stack<string>();
        private string _operation = "";
        private bool hasDecimal = false;
        #endregion

        #region Functions
        public ViewModel()
        {
            Result = new Result();
        }
        public Result Result
        {
            get { return _result; }
            set
            {
                if (_result != value)
                {
                    _result = value;
                    OnPropertyChanged(nameof(Result));
                }
            }
        }
        public string Operation
        {
            get { return _operation; }
            set
            {
                if (_operation != value)
                {
                    _operation = value;
                    OnPropertyChanged(nameof(Operation));
                }
            }
        }
        public string DeleteDots(string number)
        {
            return number.Replace(CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, "");
        }
        public string FormatNumber(string number)
        {
            if (IsDigitGroupingOn == false)
            {
                return number;
            }

            number = DeleteDots(number);

            string decimalSeparator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            int decimalIndex = number.IndexOf(decimalSeparator);
            string integerPart = number;
            string decimalPart = "";

            if (decimalIndex != -1)
            {
                integerPart = number.Substring(0, decimalIndex);
                decimalPart = number.Substring(decimalIndex);
            }

            string result = "";
            int length = integerPart.Length;
            for (int i = 0; i < length; i++)
            {
                if (i > 0 && (length - i) % 3 == 0)
                {
                    result += CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
                }
                result += integerPart[i];
            }

            result += decimalPart;

            return result;
        }
        private string RemoveLeadingZeros(string number)
        {
            if (number.Contains(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator))
            {
                return number;
            }

            number = number.TrimStart('0');
            if (string.IsNullOrEmpty(number))
            {
                number = "0";
            }

            return number;
        }
        public void AddDigitToNumber(int digit)
        {
            if (IsNewEntry)
            {
                CurrentNumber = digit.ToString();
                IsNewEntry = false;
            }
            else
            {
                CurrentNumber += digit.ToString();
            }

            CurrentNumber = RemoveLeadingZeros(CurrentNumber);
            Result.Subtotal = FormatNumber(CurrentNumber);
            NotifyPropertyChanged(nameof(Result));
        }
        public void AddDecimalPoint()
        {
            char separator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];

            if (!CurrentNumber.Contains(separator))
            {
                CurrentNumber += separator;
            }
            Result.Subtotal = FormatNumber(CurrentNumber);
            NotifyPropertyChanged(nameof(Result));
        }
        public void AddOperator(string op)
        {
            if (!string.IsNullOrEmpty(CurrentOperator))
            {
                Calculate();
            }

            PreviousNumber = CurrentNumber;
            CurrentOperator = op;
            IsNewEntry = true;
            hasDecimal = false;

            if (op != "+/-" && op != "1/x" && op != "x^2" && op != "sqrt(x)")
                Operation = $"{PreviousNumber} {CurrentOperator}";
            NotifyPropertyChanged(nameof(Operation));
        }
        public void Calculate()
        {
            double.TryParse(CurrentNumber, out double num);
            double.TryParse(PreviousNumber, out double prevNum);

            switch (CurrentOperator)
            {
                case "+/-":
                    num = -num;
                    FirstNumber = num.ToString();
                    Operation = $"{FirstNumber} = ";
                    break;
                case "+":
                    SecondNumber = num.ToString();
                    num = prevNum + num;
                    FirstNumber = prevNum.ToString();
                    Operation = $"{FirstNumber} {CurrentOperator} {SecondNumber} = ";
                    break;
                case "-":
                    SecondNumber = num.ToString();
                    num = prevNum - num;
                    FirstNumber = prevNum.ToString();
                    Operation = $"{FirstNumber} {CurrentOperator} {SecondNumber} = ";
                    break;
                case "*":
                    SecondNumber = num.ToString();
                    num = prevNum * num;
                    FirstNumber = prevNum.ToString();
                    Operation = $"{FirstNumber} {CurrentOperator} {SecondNumber} = ";
                    break;
                case "/":
                    if (num == 0)
                    {
                        MessageBox.Show("Eroare: Împărțirea la zero nu este permisă!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    SecondNumber = num.ToString();
                    num = prevNum / num;
                    FirstNumber = prevNum.ToString();
                    Operation = $"{FirstNumber} {CurrentOperator} {SecondNumber} = ";
                    break;
                case "1/x":
                    if (num == 0)
                    {
                        MessageBox.Show("Eroare: Nu se poate calcula 1/0!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    SecondNumber = num.ToString();
                    num = 1.0 / num;
                    FirstNumber = prevNum.ToString();
                    Operation = $"1/ ({SecondNumber}) = ";
                    break;
                case "x^2":
                    SecondNumber = num.ToString();
                    num = Math.Pow(num, 2);
                    FirstNumber = prevNum.ToString();
                    Operation = $"({SecondNumber}) ^ 2 = ";
                    break;
                case "sqrt(x)":
                    if (num < 0)
                    {
                        MessageBox.Show("Eroare: Nu se poate calcula radical dintr-un număr negativ!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    SecondNumber = num.ToString();
                    num = Math.Sqrt(num);
                    FirstNumber = prevNum.ToString();
                    Operation = $"sqrt({SecondNumber}) = ";
                    break;
                case "%":
                    num = prevNum % num;
                    break;
            }

            PreviousNumber = CurrentNumber;
            CurrentOperator = "";
            CurrentNumber = FormatNumber(num.ToString());
            Result.Subtotal = CurrentNumber;

            NotifyPropertyChanged(nameof(Result));
            NotifyPropertyChanged(nameof(Operation));
        }
        public void Equals()
        {
            Calculate();
        }
        public void Backspace()
        {
            if (CurrentNumber.Length > 1)
            {
                CurrentNumber = CurrentNumber.Substring(0, CurrentNumber.Length - 1);
            }
            else
            {
                CurrentNumber = "0";
                IsNewEntry = true;
            }

            Result.Subtotal = FormatNumber(CurrentNumber);
            NotifyPropertyChanged(nameof(Result));
        }
        public void ClearLastNumber()
        {
            if (CurrentNumber.Length > 1)
            {
                CurrentNumber = CurrentNumber.Substring(0, CurrentNumber.Length - 1);
            }
            else
            {
                CurrentNumber = "0";
            }
            Result.Subtotal = CurrentNumber;
            IsNewEntry = true;
            NotifyPropertyChanged(nameof(Result));
        }
        public void ClearOperation()
        {
            CurrentNumber = "0";
            PreviousNumber = "0";
            CurrentOperator = "";
            IsNewEntry = true;
            Result.Subtotal = FormatNumber(CurrentNumber);
            NotifyPropertyChanged(nameof(Result));
        }
        public void AddToMemory()
        {
            if (double.TryParse(CurrentNumber, out double currentNumber))
            {
                if (_memoryStack.Count > 0)
                {
                    if (double.TryParse(_memoryStack.Peek(), out double memoryValue))
                    {
                        double result = currentNumber + memoryValue;

                        _memoryStack.Pop();

                        _memoryStack.Push(result.ToString());
                    }
                    else
                    {
                        throw new InvalidOperationException("Valoarea din stivă nu este un număr valid.");
                    }
                }
                else
                {
                    _memoryStack.Push(currentNumber.ToString());
                }
            }
            else
            {
                throw new InvalidOperationException("CurrentNumber nu este un număr valid.");
            }

            NotifyPropertyChanged(nameof(Result));
            NotifyPropertyChanged(nameof(IsMemoryStackNotEmpty));
        }
        public void AddToMemoryStack()
        {
            if (double.TryParse(CurrentNumber, out double num))
            {
                _memoryStack.Push(num.ToString());
            }
            NotifyPropertyChanged(nameof(Result));
            NotifyPropertyChanged(nameof(IsMemoryStackNotEmpty));
        }
        public void SubtractFromMemory()
        {
            if (double.TryParse(CurrentNumber, out double num))
            {
                if (_memoryStack.Count == 0)
                {
                    _memoryStack.Push((-num).ToString());
                }
                else
                {
                    string lastValue = _memoryStack.Pop();
                    if (double.TryParse(lastValue, out double lastNum))
                    {
                        double newValue = lastNum - num;
                        _memoryStack.Push(newValue.ToString());
                    }
                }
            }
            NotifyPropertyChanged(nameof(Result));
            NotifyPropertyChanged(nameof(IsMemoryStackNotEmpty));
        }
        public void DisplayMemoryValue()
        {
            if (_memoryStack.Count > 0)
            {
                CurrentNumber = _memoryStack.Peek();
                Result.Subtotal = CurrentNumber;
                NotifyPropertyChanged(nameof(Result));
            }
        }
        public void DisplayMemoryStack()
        {
            if (_memoryStack.Count > 0)
            {
                string memoryList = string.Join(", ", _memoryStack);
                MessageBox.Show($"Valori stocate: {memoryList}", "Memorie");

            }
        }
        public void Cut()
        {
            if (!string.IsNullOrEmpty(CurrentNumber))
            {
                Clipboard.SetText(CurrentNumber);

                CurrentNumber = string.Empty;
                Result.Subtotal = CurrentNumber;
                NotifyPropertyChanged(nameof(Result));
            }
        }
        public void Copy()
        {
            if (!string.IsNullOrEmpty(CurrentNumber))
            {
                Clipboard.SetText(CurrentNumber);
            }
        }
        public void Paste()
        {
            if (Clipboard.ContainsText())
            {
                string clipboardText = Clipboard.GetText();

                CurrentNumber = clipboardText;
                Result.Subtotal = CurrentNumber;
                NotifyPropertyChanged(nameof(Result));
            }
        }
        public bool IsMemoryStackNotEmpty
        {
            get { return _memoryStack.Count > 0; }
        }
        public void ClearMemory()
        {
            _memoryStack.Clear();
            NotifyPropertyChanged(nameof(Result));
            NotifyPropertyChanged(nameof(IsMemoryStackNotEmpty));
        }
        public void UpdateDigitGrouping(bool isDigitGroupingOn)
        {
            IsDigitGroupingOn = isDigitGroupingOn;

            Properties.Settings.Default.IsDigitGroupingOn = isDigitGroupingOn;
            Properties.Settings.Default.Save();

            if (IsDigitGroupingOn)
            {
                CurrentNumber = FormatNumber(CurrentNumber);
            }
            else
            {
                CurrentNumber = DeleteDots(CurrentNumber);
            }

            Result.Subtotal = CurrentNumber;
            NotifyPropertyChanged(nameof(Result));
        }
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}