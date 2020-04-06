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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml.Serialization;

namespace Geometry_Figures
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<GeometryFigure> geometryFigures;
        private bool isMouseDown;
        private float deltaX;
        private float deltaY;

        public MainWindow()
        {
            InitializeComponent();
            geometryFigures = new List<GeometryFigure>();
            isMouseDown = false;

            DataContext = null;

            this.MouseEnter += (enterObj, enterArg) =>
            {
                if (enterArg.RightButton == MouseButtonState.Pressed || enterArg.LeftButton == MouseButtonState.Pressed)
                {
                    isMouseDown = true;
                }
            };
            this.MouseLeave += (leaveObj, leaveArg) =>
            {
                isMouseDown = false;
            };
            this.PreviewMouseDown += (downObj, downArd) =>
            {
                isMouseDown = true;
            };
            this.PreviewMouseUp += (upObj, upArd) =>
            {
                isMouseDown = false;
            };
            this.PreviewMouseWheel += (wheelObj, wheelArg) =>
            {
                if (DataContext == null)
                {
                    return;
                }

                float delta = 0;
                if (wheelArg.Delta > 0)
                {
                    delta = 0.1f;
                }
                else if (wheelArg.Delta < 0)
                {
                    delta = -0.1f;
                }

                if (SelectAllCheckBox.IsChecked == true)
                {
                    foreach (var figure in geometryFigures)
                    {
                        figure.Scaling(delta);
                    }


                    TotalAreaTextBlock.Text = CalculcateTotalArea(geometryFigures).ToString();
                }
                else
                {
                    SelectAllCheckBox.IsChecked = false;
                    ((GeometryFigure)DataContext).Scaling(delta);

                }
            };
            this.PreviewMouseMove += (moveObj, moveArg) =>
            {
                if (!isMouseDown || SelectAllCheckBox.IsChecked == true)
                {
                    return;
                }

                if (DataContext != null && ((GeometryFigure)DataContext).Figure.IsMouseOver)
                {
                    Point point = moveArg.GetPosition(MainPaintField);
                    
                    if (point.X < 0)
                    {
                        point.X = 0;
                    } else if (point.X + ((GeometryFigure)DataContext).Figure.Width > MainPaintField.ActualWidth)
                    {
                        point.X = MainPaintField.ActualWidth - ((GeometryFigure)DataContext).Figure.Width;
                    }

                    if (point.Y < 0)
                    {
                        point.Y = 0;
                    } else if (point.Y + ((GeometryFigure)DataContext).Figure.Height > MainPaintField.ActualHeight)
                    {
                        point.Y = MainPaintField.ActualHeight - ((GeometryFigure)DataContext).Figure.Height;
                    }

                    ((GeometryFigure)DataContext).CurrentPoint = point;
                }
            };
        }

        private void DeleteFigureExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectAllCheckBox.IsChecked == true)
            {
                geometryFigures.Clear();
                MainPaintField.Children.Clear();
                DataContext = null;
            }

            if (DataContext != null)
            {
                MainPaintField.Children.Remove(((GeometryFigure)DataContext).Figure);
                geometryFigures.Remove((GeometryFigure)DataContext);
                DataContext = null;
            } 
        }

        private void AddFigureExecute(object sender, ExecutedRoutedEventArgs e)
        {
            AddFigureWindow addFigureWindow = new AddFigureWindow();
            if (addFigureWindow.ShowDialog() == true)
            {
                addFigureWindow.Figure.Figure.PreviewMouseUp += (obj, arg) =>
                {
                    DataContext = addFigureWindow.Figure;

                    if (SelectAllCheckBox.IsChecked == false)
                    {
                        foreach (var figure in geometryFigures)
                        {
                            Border border = null;
                            foreach (var child in figure.Figure.Children)
                            {
                                if (child is Border)
                                {
                                    border = (Border)child;
                                }
                            }

                            if (border != null)
                            {
                                figure.Figure.Children.Remove(border);
                            }
                        }

                        Border selectBorder = new Border();
                        selectBorder.BorderThickness = new Thickness(3);
                        selectBorder.BorderBrush = new SolidColorBrush(Colors.Orange);
                        selectBorder.Width = addFigureWindow.Figure.Figure.Width;
                        selectBorder.Height = addFigureWindow.Figure.Figure.Height;

                        Binding widthBind = new Binding();
                        widthBind.Source = addFigureWindow.Figure;
                        widthBind.Path = new PropertyPath("Figure.Width");
                        widthBind.Mode = BindingMode.TwoWay;
                        selectBorder.SetBinding(Border.WidthProperty, widthBind);

                        Binding heightBind = new Binding();
                        heightBind.Source = addFigureWindow.Figure;
                        heightBind.Path = new PropertyPath("Figure.Height");
                        heightBind.Mode = BindingMode.TwoWay;
                        selectBorder.SetBinding(Border.HeightProperty, heightBind);

                        addFigureWindow.Figure.Figure.Children.Add(selectBorder);
                    }
                };

                addFigureWindow.Figure.MainField = MainPaintField;
                MainPaintField.Children.Add(addFigureWindow.Figure.Figure);

                geometryFigures.Add(addFigureWindow.Figure);
                DataContext = addFigureWindow.Figure;
            }
        }

        private void UpFigureExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectAllCheckBox.IsChecked == true)
            {
                DirectionButtonClick(UpButton, new RoutedEventArgs());
            }
        }

        private void DownFigureExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectAllCheckBox.IsChecked == true)
            {
                DirectionButtonClick(DownButton, new RoutedEventArgs());
            }
        }

        private void RightFigureExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectAllCheckBox.IsChecked == true)
            {
                DirectionButtonClick(RigthButton, new RoutedEventArgs());
            }
        }

        private void LeftFigureExecute(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectAllCheckBox.IsChecked == true)
            {
                DirectionButtonClick(LeftButton, new RoutedEventArgs());
            }
        }

        private void DirectionButtonClick(object sender, RoutedEventArgs e)
        {
            if (sender is Button) {
                Button button = (Button)sender;

                switch ((string)button.Tag)
                {
                    case "0":
                        deltaY = -3;
                        break;
                    case "1":
                        deltaY = 3;
                        break;
                    case "2":
                        deltaX = -3;
                        break;
                    case "3":
                        deltaX = 3;
                        break;
                    default:
                        break;
                }

                foreach (GeometryFigure figure in geometryFigures)
                {
                    figure.CurrentPoint = new Point(figure.CurrentPoint.X + deltaX, figure.CurrentPoint.Y + deltaY);
                }

                deltaY = 0;
                deltaX = 0;
            }
        }

        private void SelectAllClick(object sender, RoutedEventArgs e)
        {
            if (SelectAllCheckBox.IsChecked == true)
            {
                UpButton.Visibility = Visibility.Visible;
                DownButton.Visibility = Visibility.Visible;
                LeftButton.Visibility = Visibility.Visible;
                RigthButton.Visibility = Visibility.Visible;

                TotalAreaTextBlock.Text = CalculcateTotalArea(geometryFigures).ToString();

                foreach (var figure in geometryFigures)
                {
                    Border selectBorder = new Border();
                    selectBorder.BorderThickness = new Thickness(3);
                    selectBorder.BorderBrush = new SolidColorBrush(Colors.Orange);
                    selectBorder.Width = figure.Figure.Width;
                    selectBorder.Height = figure.Figure.Height;

                    figure.Figure.Children.Add(selectBorder);
                }
            } 
            else
            {
                UpButton.Visibility = Visibility.Hidden;
                DownButton.Visibility = Visibility.Hidden;
                LeftButton.Visibility = Visibility.Hidden;
                RigthButton.Visibility = Visibility.Hidden;

                TotalAreaTextBlock.Text = "-";

                foreach (var figure in geometryFigures)
                {
                    Border border = null;
                    foreach (var child in figure.Figure.Children)
                    {
                        if (child is Border)
                        {
                            border = (Border)child;
                        }
                    }

                    if (border != null)
                    {
                        figure.Figure.Children.Remove(border);
                    }
                }
            } 
        }

        private float CalculcateTotalArea(List<GeometryFigure> figures)
        {
            float totalArea = 0;

            foreach (var figure in geometryFigures)
            {
                totalArea += figure.Area;
            }

            return totalArea;
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog())
            {
                try
                {
                    saveFileDialog.Filter = "XML file (*.xml)|*.xml";
                    if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<GeometryFigure>));
                        using (Stream fs = new FileStream(saveFileDialog.FileName, FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            xmlSerializer.Serialize(fs, geometryFigures);
                        }
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void LoadClick(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                try
                {
                    openFileDialog.Filter = "XML file (*.xml)|*.xml";
                    if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<GeometryFigure>));

                        using (Stream fs = new FileStream(openFileDialog.FileName, FileMode.Open))
                        {
                            geometryFigures = (List<GeometryFigure>)xmlSerializer.Deserialize(fs);
                        }

                        foreach (var item in geometryFigures)
                        {
                            item.MainField = MainPaintField;
                            item.Figure.PreviewMouseUp += (obj, arg) =>
                            {
                                DataContext = item;

                                if (SelectAllCheckBox.IsChecked == false)
                                {
                                    foreach (var figure in geometryFigures)
                                    {
                                        Border border = null;
                                        foreach (var child in figure.Figure.Children)
                                        {
                                            if (child is Border)
                                            {
                                                border = (Border)child;
                                            }
                                        }

                                        if (border != null)
                                        {
                                            figure.Figure.Children.Remove(border);
                                        }
                                    }

                                    Border selectBorder = new Border();
                                    selectBorder.BorderThickness = new Thickness(3);
                                    selectBorder.BorderBrush = new SolidColorBrush(Colors.Orange);
                                    selectBorder.Width = item.Figure.Width;
                                    selectBorder.Height = item.Figure.Height;

                                    Binding widthBind = new Binding();
                                    widthBind.Source = item;
                                    widthBind.Path = new PropertyPath("Figure.Width");
                                    selectBorder.SetBinding(Border.WidthProperty, widthBind);

                                    Binding heightBind = new Binding();
                                    heightBind.Source = item;
                                    heightBind.Path = new PropertyPath("Figure.Height");
                                    selectBorder.SetBinding(Border.HeightProperty, heightBind);

                                    item.Figure.Children.Add(selectBorder);

                                }
                            };
                            item.Draw();
                            item.Scaling(0);

                        }
                    }
                } catch
                {
                    MessageBox.Show("Не удалось открыть файл. Проверьте целосность файла", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

        }

    }
}
