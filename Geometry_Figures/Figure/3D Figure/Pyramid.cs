using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Geometry_Figures
{
    /*
     * Коефициент 0.65 был взят для построения пирамида на поле 100х100, 
     * где 0.65 - конец нижней линии примадиты и начало боковой
     * Коефициент 0.40 был взят как верхняя точка пирамиды на поле 100х100
     * Основа пирамиды - квадратная
     */
    [Serializable]
    public class MyPyramid : GeometryFigure3D
    {
        private float bottomLine;

        public MyPyramid() : this(null, 5, 5, null, Colors.Black)
        { }

        public MyPyramid(string name, float bottomLine, float height, Canvas paintField, Color penColor, Point currentPoint = default(Point))
            : base(name, paintField, penColor, currentPoint, false)
        {
            BottomLine = bottomLine;
            Height = height;

            width = BottomLine / 0.65f;

            figure.Width = width * Scale;
            figure.Height = Height * Scale;
        }
        public override float Area { get { return (float)(Math.Pow(BottomLine  * Scale, 2) + 2 * BottomLine * Scale * 
                    Math.Sqrt((Math.Pow(BottomLine * Scale, 2) + 2 *Math.Pow(Height * Scale, 2)) / 2 - Math.Pow(BottomLine * Scale, 2) / 4)); } }

        public float BottomLine
        {
            get { return bottomLine; }
            set
            {
                if (value > 0)
                {
                    bottomLine = value;
                    width = value / 0.65f;
                }
                else
                {
                    bottomLine = 1;
                    width = bottomLine / 0.65f;
                }
                OnPropertyChanged("Radius");
            }
        }
        public float Height
        {
            get { return height; }
            set
            {
                if (value > 0)
                {
                    height = value;
                }
                else
                {
                    height = 1;
                }
                OnPropertyChanged("Height");
            }
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

            Path dashPath = new Path();
            Path visiblePath = new Path();
            GeometryGroup visibleGeometryGroup = new GeometryGroup();
            GeometryGroup dashGeometryGroup = new GeometryGroup();

            dashGeometryGroup.Children.Add(new LineGeometry(new Point(width * 0.35, Height * Scale * 0.65), new Point(width * 0.4, 0)));
            dashGeometryGroup.Children.Add(new LineGeometry(new Point(width * 0.35, Height * Scale * 0.65), new Point(width, Height * Scale * 0.65)));
            dashGeometryGroup.Children.Add(new LineGeometry(new Point(0, Height * Scale), new Point(width * 0.35, Height * Scale * 0.65)));

            visibleGeometryGroup.Children.Add(new LineGeometry(new Point(width, Height * Scale * 0.65), new Point(width * 0.4, 0)));
            visibleGeometryGroup.Children.Add(new LineGeometry(new Point(BottomLine, Height * Scale), new Point(width * 0.4, 0)));
            visibleGeometryGroup.Children.Add(new LineGeometry(new Point(0, Height * Scale), new Point(width * 0.4, 0)));
            visibleGeometryGroup.Children.Add(new LineGeometry(new Point(BottomLine, Height * Scale), new Point(width, Height * Scale * 0.65)));
            visibleGeometryGroup.Children.Add(new LineGeometry(new Point(0, Height * Scale), new Point(BottomLine, Height * Scale)));

            visiblePath.Data = visibleGeometryGroup;
            visiblePath.Width = width * Scale;
            visiblePath.Height = Height * Scale;
            visiblePath.Stroke = new SolidColorBrush(PenColor);
            visiblePath.StrokeThickness = 1;

            dashPath.Data = dashGeometryGroup;
            dashPath.Width = width * Scale;
            dashPath.Height = Height * Scale;
            dashPath.Stroke = new SolidColorBrush(PenColor);
            dashPath.StrokeThickness = 1;
            dashPath.StrokeDashArray = new DoubleCollection(new List<double> { 6, 6, 6, 6, 6, 6, 6 });

            Button trigger = new Button();
            trigger.Width = width * Scale;
            trigger.Height = height * Scale;
            trigger.Opacity = 0;

            figure.Children.Add(dashPath);
            figure.Children.Add(visiblePath);
            figure.Children.Add(trigger);

            MainField?.Children.Add(figure);

            isDrawed = true;
        }
        public override void Scaling(float delta)
        {
            base.Scaling(delta);

            foreach (var child in figure.Children)
            {
                if (child is Button)
                {
                    ((Button)child).Width = width * Scale;
                    ((Button)child).Height = height * Scale;
                }

                if (child is Path)
                {
                    Path path = (Path)child;
                    GeometryGroup geometryGroup = (GeometryGroup)path.Data;
                    path.Width = width * Scale;
                    path.Height = Height * Scale;

                    if (geometryGroup.Children.Count > 3)
                    {
                        LineGeometry line1 = (LineGeometry)geometryGroup.Children[0];
                        LineGeometry line2 = (LineGeometry)geometryGroup.Children[1];
                        LineGeometry line3 = (LineGeometry)geometryGroup.Children[2];
                        LineGeometry line4 = (LineGeometry)geometryGroup.Children[3];
                        LineGeometry line5 = (LineGeometry)geometryGroup.Children[4];

                        Point startLinePoint1 = new Point(width * Scale, Height * Scale * 0.65);
                        Point endLinePoint1 = new Point(width * 0.4 * Scale, 0);
                        Point startLinePoint2 = new Point(BottomLine * Scale, Height * Scale);
                        Point endLinePoint2 = new Point(width * 0.4 * Scale, 0);
                        Point startLinePoint3 = new Point(0, Height * Scale);
                        Point endLinePoint3 = new Point(width * 0.4 * Scale, 0);
                        Point startLinePoint4 = new Point(BottomLine * Scale, Height * Scale);
                        Point endLinePoint4 = new Point(width * Scale, Height * Scale * 0.65);
                        Point startLinePoint5 = new Point(0, Height * Scale);
                        Point endLinePoint5 = new Point(BottomLine * Scale, Height * Scale);

                        line1.StartPoint = startLinePoint1;
                        line1.EndPoint = endLinePoint1;
                        line2.StartPoint = startLinePoint2;
                        line2.EndPoint = endLinePoint2;
                        line3.StartPoint = startLinePoint3;
                        line3.EndPoint = endLinePoint3;
                        line4.StartPoint = startLinePoint4;
                        line4.EndPoint = endLinePoint4;
                        line5.StartPoint = startLinePoint5;
                        line5.EndPoint = endLinePoint5;
                    } else
                    {
                        LineGeometry line1 = (LineGeometry)geometryGroup.Children[0];
                        LineGeometry line2 = (LineGeometry)geometryGroup.Children[1];
                        LineGeometry line3 = (LineGeometry)geometryGroup.Children[2];

                        Point startLinePoint1 = new Point(width * 0.35 * Scale, Height * Scale * 0.65);
                        Point endLinePoint1 = new Point(width * 0.4 * Scale, 0);
                        Point startLinePoint2 = new Point(width * 0.35 * Scale, Height * Scale * 0.65);
                        Point endLinePoint2 = new Point(width * Scale, Height * Scale * 0.65);
                        Point startLinePoint3 = new Point(width * 0.35 * Scale, Height * Scale * 0.65);
                        Point endLinePoint3 = new Point(0, Height * Scale);

                        line1.StartPoint = startLinePoint1;
                        line1.EndPoint = endLinePoint1;
                        line2.StartPoint = startLinePoint2;
                        line2.EndPoint = endLinePoint2;
                        line3.StartPoint = startLinePoint3;
                        line3.EndPoint = endLinePoint3;
                    }

                }
            }

            OnPropertyChanged("Area");
        }
    }
}
