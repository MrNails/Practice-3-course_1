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
    public abstract class GeometryFigures2D : GeometryFigure
    {
        public GeometryFigures2D() 
        {
        }
        public GeometryFigures2D(string name, float width, float height, Canvas paintField, Color penColor) : base(name, width, height, paintField, penColor)
        {
        }
    }

    public abstract class MyQuadrangles : GeometryFigures2D
    {
        protected readonly bool IsFilled;

        public MyQuadrangles()
        {
        }
        public MyQuadrangles(string name, float width, float height, Canvas paintField, Color penColor, bool isFilled = false) : base(name, width, height, paintField, penColor)
        {
            IsFilled = isFilled;
        }

        public override float Area
        {
            get { return Width * Scale * Height * Scale; }
        }
    }

    public class MyRectangle : MyQuadrangles
    {
        public MyRectangle()
        {
        }
        public MyRectangle(string name, float width, float height, Canvas paintField, Color penColor, bool isFilled = false) 
            : base(name, width, height, paintField, penColor, isFilled)
        {
        }

        public override void Draw()
        {
            if (figure == null)
            {
                MessageBox.Show("Не возможно нарисовать телефон");
                return;
            }

            if (isDrawed)
            {
                return;
            }

            Path path = new Path();
            path.Data = new RectangleGeometry(new Rect(0, 0, Width * Scale, Height * Scale));
            path.Width = Width * Scale;
            path.Height = Height * Scale;
            path.Stroke = new SolidColorBrush(PenColor);
            path.StrokeThickness = 1;

            if (IsFilled)
            {
                path.Fill = new SolidColorBrush(PenColor);
            }

            Button trigger = new Button();
            trigger.Width = Width * Scale;
            trigger.Height = Height * Scale;
            trigger.Opacity = 0;

            figure.Children.Add(path);
            figure.Children.Add(trigger);

            MainField.Children.Add(figure);

            isDrawed = true;
        }
        public override void Scaling(float delta)
        {
            Scale += delta;

            foreach (var child in figure.Children)
            {
                if (child is Button)
                {
                    ((Button)child).Width = Width * Scale;
                    ((Button)child).Height = Height * Scale;
                }

                if (child is Path)
                {
                    Path path = (Path)child;
                    RectangleGeometry rectangleGeometry = (RectangleGeometry)path.Data;
                    Rect rect = rectangleGeometry.Rect;

                    path.Width = Width * Scale;
                    path.Height = Height * Scale;

                    rect.Width = Width * Scale;
                    rect.Height = Height * Scale;

                    rectangleGeometry.Rect = rect;
                }
            }

            OnPropertyChanged("Area");
        }
    }
}
