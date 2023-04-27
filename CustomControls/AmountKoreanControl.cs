using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MVVMToolkitTest
{
    [TemplatePart(Name = textBlockName, Type = typeof(TextBlock)) ]
    [TemplatePart(Name = textBoxName, Type = typeof(TextBox)) ]
    public class AmountKoreanControl : Control
    {
        // 해당 Control에서 제어할 Control Names
        private const string textBlockName = "PART_KoreanDisplay";
        private const string textBoxName = "PART_Amount";

        // 해당 Control에서 제어할 Controls
        private TextBlock koreanDisplayTextBlock;
        private TextBox amountTextBox;
        private bool isWork;

        static AmountKoreanControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AmountKoreanControl), new FrameworkPropertyMetadata(typeof(AmountKoreanControl)));
        }

        public override void OnApplyTemplate()
        {
            // Custom Control 각 PART를 내부에서 사용할 수 있도록 가져옴
            koreanDisplayTextBlock = GetTemplateChild(textBlockName) as TextBlock;
            amountTextBox = GetTemplateChild(textBoxName) as TextBox;

            if ( amountTextBox == null || koreanDisplayTextBlock == null )
            {
                Console.WriteLine("Control의 PART를 찾을 수 없음");
                return;
            }

            //Amount 초기값을 설정
            DecimalToFormatString(Amount);

            amountTextBox.TextChanged += AmountTextBox_TextChanged;
            amountTextBox.PreviewKeyDown += AmountTextBox_PreviewKeyDown;
        }

        // Key Down Event Handler
        private void AmountTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            bool isDigit = false;
            switch ((int)e.Key)
            {
                // Ascii Value 0~9, (NumPad 0~9 is 74~83)
                case int n when ((34 <= n && 43 >= n) || (74 <= n && 83 >= n)):
                    isDigit = true;
                    break;
            }
            if (!(isDigit || e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right))
            {
                e.Handled = true;
            }
        }

        // Text Changed Event Handler
        private void AmountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // 중복 실행에 대한 처리
            if (isWork)
            {
                return;
            }
            isWork = true;

            var numberTextOnly = amountTextBox.Text.Trim().Replace(",", "");
            if ( decimal.TryParse(numberTextOnly, out decimal decimalValue))
            {
                DecimalToFormatString(decimalValue);
            }
            else
            {
                Amount = 0;
                koreanDisplayTextBlock.Text = "";
            }
            isWork = false;
        }

        /// <summary>
        /// Decimal Value -> String 변환
        /// </summary>
        /// <param name="amount">decimal value</param>
        private void DecimalToFormatString(decimal amount)
        {
            if (amountTextBox != null && koreanDisplayTextBlock != null)
            {
                // Check Control object exist
                amountTextBox.Text = amount.ToString();

                // Carot 맨 뒤로
                amountTextBox.SelectionStart = amountTextBox.Text.Length;

                // Dependency Property에 값 입력
                this.Amount = amount;
                var korean = Number2Hangle(Amount);
                koreanDisplayTextBlock.Text = korean;
            }
        }

        private string Number2Hangle(decimal num)
        {
            string sign = "";
            string[] numberChar = new string[] { "", "일", "이", "삼", "사", "오", "육", "칠", "팔", "구" };
            string[] levelChar = new string[] { "", "십", "백", "천" };
            string[] decimalChar = new string[] { "", "만", "억", "조", "경" };

            string strValue = string.Format("{0}", num);
            string numToKorea = sign;
            bool useDecimal = false;

            for ( int i = 0; i < strValue.Length; i++)
            {
                int Level = strValue.Length - i;
                
                // 검사 자릿 수가 0이 아닌 경우에 대해서
                if ( strValue.Substring(i, 1) != "0")
                {
                    useDecimal = true;
                    if (((Level - 1) % 4) == 0)
                    {
                        numToKorea = numToKorea + numberChar[int.Parse(strValue.Substring(i, 1))] + decimalChar[(Level - 1) / 4];
                        useDecimal = false;
                    }

                    else
                    {
                        if (strValue.Substring(i, 1) == "1")
                        {
                            numToKorea += levelChar[(Level - 1) % 4];
                        }
                        else
                        {
                            numToKorea = numToKorea + numberChar[int.Parse(strValue.Substring(i, 1))] + levelChar[(Level - 1) % 4];
                        }
                    }
                }

                else
                {
                    if ((Level % 4 == 0) && useDecimal)
                    {
                        numToKorea = numToKorea + decimalChar[Level / 4];
                        useDecimal = false;
                    }
                }
            }
            return numToKorea;
        }

        #region Dependency Property
        public decimal Amount { get { return (decimal)GetValue(AmountProperty);  } set { SetValue(AmountProperty, value); } }

        public static readonly DependencyProperty AmountProperty =
            DependencyProperty.Register(nameof(Amount), typeof(decimal), typeof(AmountKoreanControl), new PropertyMetadata(decimal.Zero, AmountChanged));

        private static void AmountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (AmountKoreanControl)d;
            control.SetAmount();
        }

        private void SetAmount()
        {
            if (isWork)
            {
                return;
            }
            isWork = true;
            DecimalToFormatString(Amount);
            Console.WriteLine("SetAmount() :: Amount = " + Amount);
            isWork = false;
        }
        #endregion  
    }
}
