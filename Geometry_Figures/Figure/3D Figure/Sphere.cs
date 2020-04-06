using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Geometry_Figures
{
    [Serializable]
    public class MySphere : GeometryFigure3D
    {
        private float radius;

        public MySphere() : this(null, 5, null, Colors.Black)
        {
        }
        public MySphere(string name, float radius, Canvas paintField, Color penColor, Point currentPoint = default(Point))
            : base(name, paintField, penColor, currentPoint, false)
        {
            Radius = radius;

            width = radius * 2 * Scale;
            height = radius * 2 * Scale;

            figure.Width = width * Scale;
            figure.Height = height * Scale;
        }

        public override float Area { get { return 4 * (float)Math.PI * Radius * Radius * Scale * Scale; } }

        public float Radius
        {
            get { return radius; }
            set
            {
                if (value > 0)
                {
                    radius = value;
                    width = value * 2;
                    height = value * 2;
                    OnPropertyChanged("Radius");
                }
                else
                {
                    radius = 1;
                    width = 1 * 2;
                    height = 1 * 2;
                }
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

            Path path = new Path();
            Path dashPath = new Path();
            PathGeometry pathGeometry = new PathGeometry();
            PathGeometry dashPathGeometry = new PathGeometry();
            GeometryGroup geometryGroup = new GeometryGroup();

            PathFigure topArc = new PathFigure(new Point(0, Radius * Scale), new List<PathSegment> {
                new ArcSegment(new Point(Radius * 2 * Scale, Radius * Scale), new Size(10, 5), 0, false, SweepDirection.Clockwise, true)
            }, false);

            PathFigure bottomArc = new PathFigure(new Point(0, Radius * Scale), new List<PathSegment> {
                new ArcSegment(new Point(Radius * 2 * Scale, Radius * Scale), new Size(10, 5), 0, false, SweepDirection.Counterclockwise, true)
            }, false);

            dashPathGeometry.Figures.Add(topArc);

            pathGeometry.Figures.Add(bottomArc);

            geometryGroup.Children.Add(pathGeometry);

            geometryGroup.Children.Add(new EllipseGeometry(new Point(Radius * Scale, Radius * Scale), Radius * Scale, Radius * Scale));

            path.Data = geometryGroup;
            path.Width = Radius * 2 * Scale;
            path.Height = Radius * 2 * Scale;
            path.Stroke = new SolidColorBrush(PenColor);
            path.StrokeThickness = 1;

            dashPath.Data = dashPathGeometry;
            dashPath.Width = Radius * 2 * Scale;
            dashPath.Height = Radius * 2 * Scale;
            dashPath.Stroke = new SolidColorBrush(PenColor);
            dashPath.StrokeThickness = 1;
            dashPath.StrokeDashArray = new DoubleCollection(new List<double> { 6, 6, 6, 6, 6, 6, 6 });

            Button trigger = new Button();
            trigger.Width = width * Scale;
            trigger.Height = height * Scale;
            trigger.Opacity = 1;
            trigger.Clip = new EllipseGeometry(new Point(Radius * Scale, Radius * Scale), Radius * Scale, Radius * Scale);

            figure.Children.Add(dashPath);
            figure.Children.Add(path);
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
                    ((Button)child).Clip = new EllipseGeometry(new Point(Radius * Scale, Radius * Scale), Radius * Scale, Radius * Scale);
                }

                if (child is Path)
                {
                    Path path = (Path)child;
                    path.Width = Radius * 2 * Scale;
                    path.Height = Radius * 2 * Scale;

                    if (path.Data is GeometryGroup)
                    {
                        GeometryGroup geometryGroup = (GeometryGroup)path.Data;
                        PathGeometry pathGeometry = (PathGeometry)geometryGroup.Children[0];
                        EllipseGeometry ellipse = (EllipseGeometry)geometryGroup.Children[1];

                        Point startArcPoint = new Point(0, Radius * Scale);
                        Point endArcPoint = new Point(Radius * 2 * Scale, Radius * Scale);
                        Point centerEllipsePoint = new Point(Radius * Scale, Radius * Scale);

                        pathGeometry.Figures[0].StartPoint = startArcPoint;
                        ((ArcSegment)pathGeometry.Figures[0].Segments[0]).Point = endArcPoint;

                        ellipse.Center = centerEllipsePoint;
                        ellipse.RadiusX = Radius * Scale;
                        ellipse.RadiusY = Radius * Scale;

                    }
                    else
                    {
                        PathGeometry pathGeometry = (PathGeometry)path.Data;

                        Point startArcPoint = new Point(0, Radius * Scale);
                        Point endArcPoint = new Point(Radius * 2 * Scale, Radius * Scale);

                        pathGeometry.Figures[0].StartPoint = startArcPoint;
                        ((ArcSegment)pathGeometry.Figures[0].Segments[0]).Point = endArcPoint;
                    }
                }
            }

            OnPropertyChanged("Area");
        }
    }
}
