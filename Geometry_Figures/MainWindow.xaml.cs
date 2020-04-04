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

namespace Geometry_Figures
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<GeometryFigure> geometryFigures;
        public MainWindow()
        {
            InitializeComponent();
            geometryFigures = new List<GeometryFigure>();

            GeometryFigure figure1 = new MyRectangle("text", 200, 200, MainPaintField, Colors.Yellow, true);
            figure1.Draw();
            GeometryFigure figure2 = new MyRectangle("text1", 200, 200, MainPaintField, Colors.Green, true);
            figure2.Draw();
            DataContext = figure1;
            geometryFigures.AddRange(new List<GeometryFigure> { figure1, figure2 } );

            figure1.Figure.PreviewMouseUp += (obj, arg) =>
            {
                DataContext = figure1;
            };
            figure2.Figure.PreviewMouseUp += (obj, arg) =>
            {
                DataContext = figure2;
            };

            this.MouseWheel += (wheelObj, wheelArg) =>
            {
                float delta = 0;
                if (wheelArg.Delta > 0)
                {
                    delta = 0.1f;
                } else if (wheelArg.Delta < 0)
                {
                    delta = -0.1f;
                }

                if (SelectAllCheckBox.IsChecked == true)
                {
                    foreach (var item in geometryFigures)
                    {
                        item.Scaling(delta);
                    }
                } else
                {
                    SelectAllCheckBox.IsChecked = false;
                    ((GeometryFigure)DataContext).Scaling(delta);
                }
            };
        }
    }
}
