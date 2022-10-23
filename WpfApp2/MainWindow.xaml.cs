using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            lbl_squareRoot.Content = ((char)0x221A).ToString();
            lbl_squareOfX.Content = $"x\u00B2";
        }


        private double _result;
        bool anySymbol = false;


        private void symbol_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (!anySymbol)
                    _result = double.Parse(lbl_result.Text);

                double yoxlamaResult = _result;
                _result = btn.Name switch
                {
                    "faiz" => _result / 100,
                    "musbetMenfi" => _result * -1,
                    "birBolX" => 1 / _result,
                    "lbl_squareOfX" => Math.Pow(_result, 2),
                    "lbl_squareRoot" => Math.Sqrt(_result),
                    _ => _result
                };

                if (yoxlamaResult != _result)
                {
                    labelAdjustment(_result.ToString(), true);
                    anySymbol = false;
                    return;
                }

                if (btn.Name.Equals("beraber"))
                {
                    if (!char.IsDigit(lbl_result.Text.Last()))
                    {
                        MessageBox.Show("No number in front of symbol...",
                                        "Caution",
                                        MessageBoxButton.OK,
                                        MessageBoxImage.Warning);
                        return;
                    }

                    findAnswer();
                    labelAdjustment(_result.ToString(), true);
                    anySymbol = false;
                }

                string symbolStr = btn.Name switch
                {
                    "ustegel" => "+",
                    "cix" => "-",
                    "vur" => "*",
                    "bol" => "/",
                    "vergul" => ".",
                    _ => string.Empty
                };

                labelAdjustment(symbolStr);
                anySymbol = true;

            }
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (btn.Content.Equals("C") || btn.Content.Equals("CE"))
                {
                    labelAdjustment("0", true);
                    anySymbol = false;
                    return;
                }
                if (lbl_result.Text.Length == 1)
                {
                    labelAdjustment("0", true);
                    anySymbol= false;
                }
                else
                    labelAdjustment(lbl_result.Text.Remove(lbl_result.Text.Length - 1), true);
            }
        }

        private void number_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn)
            {
                if (lbl_result.Text.Equals("0"))
                {
                    labelAdjustment(btn.Content.ToString()!, true);
                    return;
                }
                labelAdjustment(btn.Content.ToString()!);
            }
        }


        private void labelAdjustment(string text, bool fullyReplace = false)
        {
            if (string.IsNullOrEmpty(text))
                return;


            if (fullyReplace)
                lbl_result.Text = text;
            else
                lbl_result.Text += text;

            if (lbl_result.Text.Length > 15)
                lbl_result.FontSize = 30;
            else if (lbl_result.Text.Length > 5)
                lbl_result.FontSize = 50;
            else
                lbl_result.FontSize = 80;
        }

        private void findAnswer()
        {
            try
            {
                _result = double.Parse(new DataTable().Compute(lbl_result.Text, null).ToString()!);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message,
                                "Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
                _result = 0;
            }
        }

    }
}
