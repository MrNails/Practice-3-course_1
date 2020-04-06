using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Geometry_Figures
{
    /// <summary>
    /// Логика взаимодействия для AddFigureWindow.xaml
    /// </summary>
    public partial class AddFigureWindow : Window
    {
        private int currentIndex;

        public Color PenColor { get; set; }
        public GeometryFigure Figure { get; set; }

        public AddFigureWindow()
        {
            InitializeComponent();
            FigureTypeComboBox.IsEnabled = false;
            currentIndex = 0;
            Figure = null;
        }

        private void NextTabClick(object sender, RoutedEventArgs e)
        {
            if (FigureTypeComboBox.SelectedIndex == -1 && FigureComboBox.SelectedIndex == -1 || ColorComboBox.SelectedIndex == -1)
            {
                return;
            }

            if (currentIndex < FigureTabs.Items.Count)
            {
                currentIndex++;
            }

            ((TabItem)FigureTabs.Items[currentIndex]).IsSelected = true;

            CreateInstace(100, 150, 150);

        }

        private void CreateInstace(float firstParam, float secondParam, float thirdParam)
        {
            if (ExampleCanvas.Children.Count > 0)
            {
                ExampleCanvas.Children.Clear();
                Figure = null;
            }

            bool fill;

            if (FillCheckBox.IsChecked.HasValue)
            {
                fill = FillCheckBox.IsChecked.Value;
            }
            else
            {
                fill = false;
            }

            switch ((String)FigureTypeComboBox.SelectedItem)
            {
                case "Прямоугольник":
                    FillCheckBox.IsEnabled = true;
                    TextBlock1.Text = "Ширина";
                    TextBlock2.Text = "Длина";
                    Figure = new MyRectangle("Прямоугольник", firstParam, secondParam, ExampleCanvas, PenColor, isFilled: fill);
                    break;
                case "Ромб":
                    FillCheckBox.IsEnabled = true;
                    TextBlock1.Text = "Ширина";
                    TextBlock2.Text = "Длина";
                    Figure = new MyRhombus("Ромб", firstParam, secondParam, ExampleCanvas, PenColor, isFilled: fill);
                    break;
                case "Трапеция":
                    FillCheckBox.IsEnabled = true;
                    TextBlock1.Text = "Нижняя граница";
                    TextBlock2.Text = "Верхняя граница";
                    TextBlock3.Text = "Высота";
                    Figure = new MyTrapeze("Трапеция", firstParam, secondParam, thirdParam, ExampleCanvas, PenColor, isFilled: fill);
                    TextBlock3.Visibility = Visibility.Visible;
                    TextBox3.Visibility = Visibility.Visible;
                    break;
                case "Равнобедренный треугольник":
                    FillCheckBox.IsEnabled = true;
                    TextBlock1.Text = "Боковые стороны";
                    TextBlock2.Text = "Основа";
                    Figure = new MyIsoscelesTriangle("Равнобедренный треугольник", firstParam, secondParam, ExampleCanvas, PenColor, isFilled: fill);
                    break;
                case "Равносторонний треугольник":
                    FillCheckBox.IsEnabled = true;
                    TextBlock1.Text = "Сторона";
                    Figure = new MyEquilateralTriangle("Равносторонний треугольник", firstParam, ExampleCanvas, PenColor, isFilled: fill);
                    TextBlock2.Visibility = Visibility.Hidden;
                    TextBox2.Visibility = Visibility.Hidden;
                    break;
                case "Прямоугольный треугольник":
                    FillCheckBox.IsEnabled = true;
                    TextBlock1.Text = "Левая сторона";
                    TextBlock2.Text = "Основа";
                    Figure = new MyRightTriangle("Прямоугольный треугольник", firstParam, secondParam, thirdParam, ExampleCanvas, PenColor, isFilled: fill);
                    break;
                case "Круг":
                    FillCheckBox.IsEnabled = true;
                    TextBlock1.Text = "Радиус";
                    Figure = new MyCircle("Круг", firstParam, ExampleCanvas, PenColor, isFilled: fill);
                    TextBlock2.Visibility = Visibility.Hidden;
                    TextBox2.Visibility = Visibility.Hidden;
                    break;
                case "Элипс":
                    TextBlock1.Text = "Радиус по X";
                    TextBlock2.Text = "Радиус по Y";
                    Figure = new MyEllipse("Элипс", firstParam, secondParam, ExampleCanvas, PenColor, isFilled: fill);
                    break;
                default:
                    switch (((ComboBoxItem)FigureComboBox.SelectedItem).Content)
                    {
                        case "Цилиндр":
                            TextBlock1.Text = "Радиус";
                            TextBlock2.Text = "Высота";
                            Figure = new MyCilyndre("Цилиндр", firstParam, secondParam, ExampleCanvas, PenColor);
                            break;
                        case "Пирамида":
                            TextBlock1.Text = "Нижнияя сторона";
                            TextBlock2.Text = "Высота";
                            Figure = new MyPyramid("Пирамида", firstParam, secondParam, ExampleCanvas, PenColor);
                            break;
                        case "Сфера":
                            TextBlock1.Text = "Радиус";
                            TextBlock2.IsEnabled = false;
                            TextBox2.IsEnabled = false;
                            Figure = new MySphere("Сфера", firstParam, ExampleCanvas, PenColor);
                            break;
                        default:
                            break;
                    }
                    break;
            }

            Figure.Draw();
        }

        private void RefuseClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void AcceptClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            ExampleCanvas.Children.Clear();
        }

        private void BackTabClick(object sender, RoutedEventArgs e)
        {
            if (currentIndex >= 0)
            {
                currentIndex--;
            }

            ((TabItem)FigureTabs.Items[currentIndex]).IsSelected = true;

            TextBlock2.Visibility = Visibility.Visible;
            TextBox2.Visibility = Visibility.Visible;
            TextBlock3.Visibility = Visibility.Hidden;
            TextBox3.Visibility = Visibility.Hidden;
            ExampleCanvas.Children.Clear();
            Figure = null;
        }

        private void ChangeColor(object sender, SelectionChangedEventArgs e)
        {
            if (ColorComboBox != null && ColorExample != null)
            {
                switch (((ComboBoxItem)ColorComboBox.SelectedItem).Content)
                {
                    case "Red":
                        PenColor = Colors.Red;
                        break;
                    case "Green":
                        PenColor = Colors.Green;
                        break;
                    case "Blue":
                        PenColor = Colors.Blue;
                        break;
                    case "Black":
                        PenColor = Colors.Black;
                        break;
                    default:
                        PenColor = Colors.Black;
                        break;
                }
                ColorExample.Background = new SolidColorBrush(PenColor);
            }
        }

        private void ChangeFigure(object sender, SelectionChangedEventArgs e)
        {
            if (FigureComboBox == null || FigureTypeComboBox == null)
            {
                return;
            }

            switch (((ComboBoxItem)FigureComboBox.SelectedItem).Content)
            {
                case "Четырёхугольник":
                    FigureTypeComboBox.IsEnabled = true;
                    FigureTypeComboBox.ItemsSource = new string[] { "Прямоугольник", "Ромб", "Трапеция" };
                    FigureTypeComboBox.SelectedIndex = 0;
                    FillCheckBox.IsEnabled = true;
                    break;
                case "Треугольник":
                    FigureTypeComboBox.IsEnabled = true;
                    FigureTypeComboBox.ItemsSource = new string[] { "Равнобедренный треугольник", "Равносторонний треугольник", "Прямоугольный треугольник" };
                    FigureTypeComboBox.SelectedIndex = 0;
                    FillCheckBox.IsEnabled = true;
                    break;
                case "Кривые второго порядка":
                    FigureTypeComboBox.IsEnabled = true;
                    FigureTypeComboBox.ItemsSource = new string[] { "Круг", "Элипс" };
                    FigureTypeComboBox.SelectedIndex = 0;
                    FillCheckBox.IsEnabled = true;
                    break;
                case "Цилиндр":
                    FigureTypeComboBox.IsEnabled = false;
                    FigureTypeComboBox.ItemsSource = null;
                    FillCheckBox.IsEnabled = false;
                    FillCheckBox.IsChecked = false;
                    break;
                case "Пирамида":
                    FigureTypeComboBox.IsEnabled = false;
                    FigureTypeComboBox.ItemsSource = null;
                    FillCheckBox.IsEnabled = false;
                    FillCheckBox.IsChecked = false;
                    break;
                case "Сфера":
                    FigureTypeComboBox.IsEnabled = false;
                    FigureTypeComboBox.ItemsSource = null;
                    FillCheckBox.IsEnabled = false;
                    FillCheckBox.IsChecked = false;
                    break;
                default:
                    break;
            }
        }

        private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            float firstParametr;
            float secondParametr;
            float thirdParametr;

            if (!TextBox1.IsEnabled || !Single.TryParse(TextBox1.Text, out firstParametr)) {
                firstParametr = 100;
            }

            if (!TextBox2.IsEnabled || !Single.TryParse(TextBox2.Text, out secondParametr))
            {
                secondParametr = 100;
            }

            if (!TextBox3.IsEnabled || !Single.TryParse(TextBox3.Text, out thirdParametr))
            {
                thirdParametr = 100;
            }

            try
            {
                CreateInstace(firstParametr, secondParametr, thirdParametr);
            } catch
            { }
        }
    }
}
