using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Geometry_Figures
{
    public static class MyCommand
    {
        static MyCommand()
        {
            DeleteFigure = new RoutedCommand("DeleteFigure", typeof(MainWindow));
            AddFigure = new RoutedCommand("AddFigure", typeof(MainWindow));
            UpFigure = new RoutedCommand("UpFigure", typeof(MainWindow));
            DownFigure = new RoutedCommand("DownFigure", typeof(MainWindow));
            RightFigure = new RoutedCommand("RightFigure", typeof(MainWindow));
            LeftFigure = new RoutedCommand("LeftFigure", typeof(MainWindow));
        }

        public static RoutedCommand UpFigure { get; set; }
        public static RoutedCommand DownFigure { get; set; }
        public static RoutedCommand RightFigure { get; set; }
        public static RoutedCommand LeftFigure { get; set; }
        public static RoutedCommand DeleteFigure { get; set; }
        public static RoutedCommand AddFigure { get; set; }
    }
}
