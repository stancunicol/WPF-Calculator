using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;

namespace Tema_1
{
    public class ProgrammerFunctions : INotifyPropertyChanged
    {
        #region Variables
        public bool IsDigitGroupingOn { get; set; } = Properties.Settings.Default.IsDigitGroupingOn;
        public event PropertyChangedEventHandler PropertyChanged;
        private Result _result;
        private string _operation = "";
        public string CurrentNumber { get; set; } = "0";
        public string PreviousNumber { get; set; } = "0";
        public string CurrentOperator { get; set; } = "";
        public string FirstNumber { get; set; } = "";
        public string SecondNumber { get; set; } = "";
        public bool IsNewEntry { get; set; } = true;
        public Stack<string> _memoryStack = new Stack<string>();
        private bool _isHexOn;
        private bool _isDecOn;
        private bool _isOctOn;
        private bool _isBinOn;
        private bool _shouldResetNumber = false;
        #endregion

        #region Functions
        public ProgrammerFunctions()
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
                    OnPropertyChanged(nameof(HexBox));
                    OnPropertyChanged(nameof(DecBox));
                    OnPropertyChanged(nameof(OctBox));
                    OnPropertyChanged(nameof(BinBox));
                }
            }
        }
        public bool IsHexOn
        {
            get { return _isHexOn; }
            set
            {
                if (_isHexOn != value)
                {
                    _isHexOn = value;
                    CurrentNumber = "0";
                    PreviousNumber = "0";
                    Result.Subtotal = "0";
                    NotifyPropertyChanged(nameof(Result));
                    OnPropertyChanged(nameof(IsHexOn));
                }
            }
        }
        public bool IsDecOn
        {
            get { return _isDecOn; }
            set
            {
                if (_isDecOn != value)
                {
                    _isDecOn = value;
                    CurrentNumber = "0";
                    PreviousNumber = "0";
                    Result.Subtotal = "0";
                    NotifyPropertyChanged(nameof(Result));
                    OnPropertyChanged(nameof(IsDecOn));
                }
            }
        }
        public bool IsOctOn
        {
            get { return _isOctOn; }
            set
            {
                if (_isOctOn != value)
                {
                    _isOctOn = value;
                    CurrentNumber = "0";
                    PreviousNumber = "0";
                    Result.Subtotal = "0";
                    NotifyPropertyChanged(nameof(Result));
                    OnPropertyChanged(nameof(IsOctOn));
                }
            }
        }
        public bool IsBinOn
        {
            get { return _isBinOn; }
            set
            {
                if (_isBinOn != value)
                {
                    _isBinOn = value;
                    CurrentNumber = "0";
                    PreviousNumber = "0";
                    Result.Subtotal = "0";
                    NotifyPropertyChanged(nameof(Result));
                    OnPropertyChanged(nameof(IsBinOn));
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
        public void Equals()
        {
            Calculate();

            PreviousNumber = CurrentNumber;
            CurrentNumber = string.Empty;

            NotifyPropertyChanged(nameof(Operation));

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
        public string FormatNumber(string number)
        {
            if (IsDigitGroupingOn == false)
            {
                return number;
            }

            number = DeleteDots(number);

            string integerPart = number;

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

            return result;
        }
        private bool IsValidDigit(string digit)
        {
            if (IsHexOn)
            {
                return "0123456789ABCDEF".Contains(digit.ToUpper());
            }
            return "0123456789".Contains(digit);
        }
        public void AddDigitToNumber(string digit)
        {
            if (!IsValidDigit(digit))
                return;

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

            if (IsDecOn)
            {
                Result.Subtotal = FormatNumber(CurrentNumber);
            }
            else
            {
                Result.Subtotal = CurrentNumber;
            }

            NotifyPropertyChanged(nameof(Result));
            NotifyPropertyChanged(nameof(HexBox));
            NotifyPropertyChanged(nameof(DecBox));
            NotifyPropertyChanged(nameof(OctBox));
            NotifyPropertyChanged(nameof(BinBox));
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
            if (op != "+/-")
                Operation = $"{PreviousNumber} {CurrentOperator}";
            NotifyPropertyChanged(nameof(Operation));
        }
        public void Calculate()
        {
            string num = CurrentNumber;
            string prevNum = PreviousNumber;

            switch (CurrentOperator)
            {
                case "+/-":
                    num = (long.Parse(num) * -1).ToString();
                    FirstNumber = num.ToString();
                    Operation = $"{FirstNumber} = ";
                    break;
                case "+":
                    SecondNumber = num.ToString();
                    num = AddNumbers(prevNum, num);
                    FirstNumber = prevNum.ToString();
                    Operation = $"{FirstNumber} {CurrentOperator} {SecondNumber} = ";
                    break;
                case "-":
                    SecondNumber = num.ToString();
                    num = SubtractNumbers(prevNum, num);
                    FirstNumber = prevNum.ToString();
                    Operation = $"{FirstNumber} {CurrentOperator} {SecondNumber} = ";
                    break;
                case "*":
                    SecondNumber = num.ToString();
                    num = MultiplyNumbers(prevNum, num);
                    FirstNumber = prevNum.ToString();
                    Operation = $"{FirstNumber} {CurrentOperator} {SecondNumber} = ";
                    break;
                case "/":
                    if (num == "0")
                    {
                        MessageBox.Show("Eroare: Împărțirea la zero nu este permisă!", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    SecondNumber = num.ToString();
                    num = DivideNumbers(prevNum, num);
                    FirstNumber = prevNum.ToString();
                    Operation = $"{FirstNumber} {CurrentOperator} {SecondNumber} = ";
                    break;
                case "%":
                    SecondNumber = num.ToString();
                    num = ModuloNumbers(prevNum, num);
                    FirstNumber = prevNum.ToString();
                    Operation = $"{FirstNumber} {CurrentOperator} {SecondNumber} = ";
                    break;
            }

            PreviousNumber = CurrentNumber;
            CurrentOperator = "";
            CurrentNumber = num;
            Result.Subtotal = CurrentNumber;

            NotifyPropertyChanged(nameof(Result));
            NotifyPropertyChanged(nameof(HexBox));
            NotifyPropertyChanged(nameof(DecBox));
            NotifyPropertyChanged(nameof(OctBox));
            NotifyPropertyChanged(nameof(BinBox));
            NotifyPropertyChanged(nameof(Operation));
        }
        private string AddNumbers(string num1, string num2)
        {
            if (IsHexOn)
            {
                long n1 = Convert.ToInt64(num1, 16);
                long n2 = Convert.ToInt64(num2, 16);
                return Convert.ToString(n1 + n2, 16).ToUpper();
            }
            else if (IsOctOn)
            {
                long n1 = Convert.ToInt64(num1, 8);
                long n2 = Convert.ToInt64(num2, 8);
                return Convert.ToString(n1 + n2, 8);
            }
            else if (IsBinOn)
            {
                long n1 = Convert.ToInt64(num1, 2);
                long n2 = Convert.ToInt64(num2, 2);
                return Convert.ToString(n1 + n2, 2);
            }
            else
            {
                return FormatNumber((long.Parse(num1) + long.Parse(num2)).ToString());
            }
        }
        private string SubtractNumbers(string num1, string num2)
        {
            if (string.IsNullOrWhiteSpace(num1) || string.IsNullOrWhiteSpace(num2))
            {
                throw new ArgumentException("Input numbers cannot be empty.");
            }

            if (IsHexOn)
            {
                long n1 = Convert.ToInt64(num1, 16);
                long n2 = Convert.ToInt64(num2, 16);
                return Convert.ToString(n1 - n2, 16).ToUpper();
            }
            else if (IsOctOn)
            {
                long n1 = Convert.ToInt64(num1, 8);
                long n2 = Convert.ToInt64(num2, 8);
                return Convert.ToString(n1 - n2, 8);
            }
            else if (IsBinOn)
            {
                long n1 = Convert.ToInt64(num1, 2);
                long n2 = Convert.ToInt64(num2, 2);
                return Convert.ToString(n1 - n2, 2);
            }
            else
            {
                return FormatNumber((long.Parse(num1) - long.Parse(num2)).ToString());
            }
        }
        private string MultiplyNumbers(string num1, string num2)
        {
            if (IsHexOn)
            {
                long n1 = Convert.ToInt64(num1, 16);
                long n2 = Convert.ToInt64(num2, 16);
                return Convert.ToString(n1 * n2, 16).ToUpper();
            }
            else if (IsOctOn)
            {
                long n1 = Convert.ToInt64(num1, 8);
                long n2 = Convert.ToInt64(num2, 8);
                return Convert.ToString(n1 * n2, 8);
            }
            else if (IsBinOn)
            {
                long n1 = Convert.ToInt64(num1, 2);
                long n2 = Convert.ToInt64(num2, 2);
                return Convert.ToString(n1 * n2, 2);
            }
            else
            {
                return FormatNumber((long.Parse(num1) * long.Parse(num2)).ToString());
            }
        }
        private string DivideNumbers(string num1, string num2)
        {
            if (IsHexOn)
            {
                long n1 = Convert.ToInt64(num1, 16);
                long n2 = Convert.ToInt64(num2, 16);
                return Convert.ToString(n1 / n2, 16).ToUpper();
            }
            else if (IsOctOn)
            {
                long n1 = Convert.ToInt64(num1, 8);
                long n2 = Convert.ToInt64(num2, 8);
                return Convert.ToString(n1 / n2, 8);
            }
            else if (IsBinOn)
            {
                long n1 = Convert.ToInt64(num1, 2);
                long n2 = Convert.ToInt64(num2, 2);
                return Convert.ToString(n1 / n2, 2);
            }
            else
            {
                return FormatNumber((long.Parse(num1) / long.Parse(num2)).ToString());
            }
        }
        private string ModuloNumbers(string num1, string num2)
        {
            if (IsHexOn)
            {
                long n1 = Convert.ToInt64(num1, 16);
                long n2 = Convert.ToInt64(num2, 16);
                return Convert.ToString(n1 % n2, 16).ToUpper();
            }
            else if (IsOctOn)
            {
                long n1 = Convert.ToInt64(num1, 8);
                long n2 = Convert.ToInt64(num2, 8);
                return Convert.ToString(n1 % n2, 8);
            }
            else if (IsBinOn)
            {
                long n1 = Convert.ToInt64(num1, 2);
                long n2 = Convert.ToInt64(num2, 2);
                return Convert.ToString(n1 % n2, 2);
            }
            else
            {
                return FormatNumber((long.Parse(num1) % long.Parse(num2)).ToString());
            }
        }
        public string HexBox
        {
            get
            {
                if (IsHexOn)
                    return Result.Subtotal;
                else
                    return ConvertFromDecimalToHex(ConvertFromCurrentBaseToDecimal(Result.Subtotal));
            }
        }
        private string ConvertFromDecimalToHex(string decimalNumber)
        {
            decimalNumber = DeleteDots(decimalNumber);
            if (long.TryParse(decimalNumber, out long number))
            {
                return Convert.ToString(number, 16).ToUpper();
            }
            return "0";
        }
        public string OctBox
        {
            get
            {
                if (IsOctOn)
                    return Result.Subtotal;
                else
                    return ConvertFromDecimalToOctal(ConvertFromCurrentBaseToDecimal(Result.Subtotal));
            }
        }
        private string ConvertFromDecimalToOctal(string decimalNumber)
        {
            decimalNumber = DeleteDots(decimalNumber);
            if (long.TryParse(decimalNumber, out long number))
            {
                return Convert.ToString(number, 8);
            }
            return "0";
        }
        public string DecBox
        {
            get
            {
                if (IsDecOn)
                    return FormatNumber(Result.Subtotal);
                else
                    return FormatNumber(ConvertFromCurrentBaseToDecimal(Result.Subtotal));
            }
        }
        private string ConvertFromCurrentBaseToDecimal(string number)
        {
            number = DeleteDots(number);
            if (IsHexOn)
            {
                return Convert.ToInt64(number, 16).ToString();
            }
            else if (IsOctOn)
            {
                return Convert.ToInt64(number, 8).ToString();
            }
            else if (IsBinOn)
            {
                return Convert.ToInt64(number, 2).ToString();
            }
            else
            {
                return number;
            }
        }
        public string BinBox
        {
            get
            {
                if (IsBinOn)
                    return Result.Subtotal;
                else
                    return ConvertFromDecimalToBinary(ConvertFromCurrentBaseToDecimal(Result.Subtotal));
            }
        }
        private string ConvertFromDecimalToBinary(string decimalNumber)
        {
            decimalNumber = DeleteDots(decimalNumber);
            if (long.TryParse(decimalNumber, out long number))
            {
                return Convert.ToString(number, 2);
            }
            return "0";
        }
        public bool IsMemoryStackNotEmpty
        {
            get { return _memoryStack.Count > 0; }
        }
        public void AddToMemoryStack()
        {
            _memoryStack.Push(CurrentNumber);
            NotifyPropertyChanged(nameof(Result));
            NotifyPropertyChanged(nameof(IsMemoryStackNotEmpty));
        }
        public void DisplayMemoryStack()
        {
            if (_memoryStack.Count > 0)
            {
                string memoryList = string.Join(", ", _memoryStack);
                MessageBox.Show($"Valori stocate: {memoryList}", "Memorie");

            }
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
        public void ClearOperation()
        {
            CurrentNumber = "0";
            PreviousNumber = "0";
            CurrentOperator = "";
            IsNewEntry = true;
            Result.Subtotal = FormatNumber(CurrentNumber);
            NotifyPropertyChanged(nameof(Result));
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