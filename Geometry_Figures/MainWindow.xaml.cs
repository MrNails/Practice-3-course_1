using System;
using System.Collections.ObjectModel;
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

namespace Geometry_Figures
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<GeometryFigure> geometryFigures;
        private bool isMouseDown;
        private float deltaX;
        private float deltaY;

        public MainWindow()
        {
            InitializeComponent();
            geometryFigures = new ObservableCollection<GeometryFigure>();
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
                    foreach (var item in geometryFigures)
                    {
                        item.Scaling(delta);
                    }

                    if (SelectAllCheckBox.IsChecked == true)
                    {
                        TotalAreaTextBlock.Text = CalculcateTotalArea(geometryFigures).ToString();
                    }
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

                if (DataContext != null)
                {
                    ((GeometryFigure)DataContext).Move(moveArg.GetPosition(MainPaintField));
                }
            };
        }

        private void DeleteFigureExecute(object sender, ExecutedRoutedEventArgs e)
        {
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
                    MessageBox.Show("+");
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
                    figure.Move(new Point(figure.CurrentPoint.X + deltaX, figure.CurrentPoint.Y + deltaY));
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
            } 
            else
            {
                UpButton.Visibility = Visibility.Hidden;
                DownButton.Visibility = Visibility.Hidden;
                LeftButton.Visibility = Visibility.Hidden;
                RigthButton.Visibility = Visibility.Hidden;

                TotalAreaTextBlock.Text = "-";
            } 
        }

        private float CalculcateTotalArea(Collection<GeometryFigure> figures)
        {
            float totalArea = 0;

            foreach (var figure in geometryFigures)
            {
                totalArea += figure.Area;
            }

            return totalArea;
        }
    }
}
