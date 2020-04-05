using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Geometry_Figures
{
    public class MyCilyndre : GeometryFigure3D
    {
        private float radius;

        public MyCilyndre() : this (null, 5, 5,null, Colors.Black)
        { }

        public MyCilyndre(string name, float radius, float height, Canvas paintField, Color penColor, Point currentPoint = default(Point))
            : base(name, paintField, penColor, currentPoint, false)
        {
            Radius = radius;
            Height = height;

            width = Radius * 2;

            figure.Width = width * Scale;
            figure.Height = Height * Scale;
        }
        public override float Area { get { return (float)Math.PI * Radius * Height * Scale * Scale; } }

        public float Radius
        {
            get { return radius; }
            set 
            {
                if (value > 0)
                {
                    radius = value;
                } else
                {
                    radius = 1;
                }
                OnPropertyChanged("Radius");
            }
        }
        public float Height
        {
            get { return height; }
            set
            {
                if (value > 0 && value > Radius * Scale * 1.3)
                {
                    height = value;
                    OnPropertyChanged("Height");
                }
                else
                {
                    throw new ArgumentException("Не правильная высота");
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

            PathFigure topArc = new PathFigure(new Point(0, Height * Scale - Radius * Scale * 0.5), new List<PathSegment> {
                new ArcSegment(new Point(Radius * 2 * Scale, Height * Scale - Radius * Scale * 0.5), new Size(10, 5), 0, false, SweepDirection.Clockwise, true)
            }, false);

            PathFigure bottomArc = new PathFigure(new Point(0, Height * Scale - Radius * Scale * 0.5), new List<PathSegment> { 
                new ArcSegment(new Point(Radius * 2 * Scale, Height * Scale - Radius * Scale * 0.5), new Size(10, 5), 0, false, SweepDirection.Counterclockwise, true)
            }, false);

            dashPathGeometry.Figures.Add(topArc);

            pathGeometry.Figures.Add(bottomArc);

            geometryGroup.Children.Add(pathGeometry);

            geometryGroup.Children.Add(new EllipseGeometry(new Point(Radius * Scale, Radius * Scale * 0.5), Radius * Scale, Radius * Scale * 0.5));
            geometryGroup.Children.Add(new LineGeometry(new Point(0, Radius * Scale * 0.5), new Point(0, Height * Scale - Radius * Scale * 0.5)));
            geometryGroup.Children.Add(new LineGeometry(new Point(Radius * 2 * Scale, Radius * Scale * 0.5), new Point(Radius * 2 * Scale, Height * Scale - Radius * Scale * 0.5)));

            path.Data = geometryGroup;
            path.Width = Radius * 2 * Scale;
            path.Height = Height * Scale;
            path.Stroke = new SolidColorBrush(PenColor);
            path.StrokeThickness = 1;

            dashPath.Data = dashPathGeometry;
            dashPath.Width = Radius * 2 * Scale;
            dashPath.Height = Height * Scale;
            dashPath.Stroke = new SolidColorBrush(PenColor);
            dashPath.StrokeThickness = 1;
            dashPath.StrokeDashArray = new DoubleCollection(new List<double> { 6, 6, 6, 6, 6, 6, 6 });

            TextBlock trigger = new TextBlock();
            trigger.Width = width * Scale;
            trigger.Height = height * Scale;

            figure.Children.Add(dashPath);
            figure.Children.Add(path);
            figure.Children.Add(trigger);

            MainField.Children.Add(figure);

            isDrawed = true;
        }
        public override void Scaling(float delta)
        {
            base.Scaling(delta);

            foreach (var child in figure.Children)
            {
                if (child is TextBlock)
                {
                    ((TextBlock)child).Width = width * Scale;
                    ((TextBlock)child).Height = height * Scale;
                }

                if (child is Path)
                {
                    Path path = (Path)child;
                    path.Width = Radius * 2 * Scale;
                    path.Height = Height * Scale;

                    if (path.Data is GeometryGroup)
                    {
                        GeometryGroup geometryGroup = (GeometryGroup)path.Data;
                        PathGeometry pathGeometry = (PathGeometry)geometryGroup.Children[0];
                        EllipseGeometry ellipse = (EllipseGeometry)geometryGroup.Children[1];
                        LineGeometry rightLine = (LineGeometry)geometryGroup.Children[2];
                        LineGeometry leftLine = (LineGeometry)geometryGroup.Children[3];

                        Point startArcPoint = new Point(0, Height * Scale - Radius * Scale * 0.5);
                        Point endArcPoint = new Point(Radius * 2 * Scale, Height * Scale - Radius * Scale * 0.5);
                        Point centerEllipsePoint = new Point(Radius * Scale, Radius * Scale * 0.5);
                        Point startRightLinePoint = new Point(0, Radius * Scale * 0.5);
                        Point endRightLinePoint = new Point(0, Height * Scale - Radius * Scale * 0.5);
                        Point startLeftLinePoint = new Point(Radius * 2 * Scale, Radius * Scale * 0.5);
                        Point endLeftLinePoint = new Point(Radius * 2 * Scale, Height * Scale - Radius * Scale * 0.5);

                        pathGeometry.Figures[0].StartPoint = startArcPoint;
                        ((ArcSegment)pathGeometry.Figures[0].Segments[0]).Point = endArcPoint;

                        ellipse.Center = centerEllipsePoint;
                        ellipse.RadiusX = Radius * Scale;
                        ellipse.RadiusY = Radius * Scale * 0.5;

                        rightLine.StartPoint = startRightLinePoint;
                        rightLine.EndPoint = endRightLinePoint;
                        leftLine.StartPoint = startLeftLinePoint;
                        leftLine.EndPoint = endLeftLinePoint;

                    } else
                    {
                        PathGeometry pathGeometry = (PathGeometry)path.Data;

                        Point startArcPoint = new Point(0, Height * Scale - Radius * Scale * 0.5);
                        Point endArcPoint = new Point(Radius * 2 * Scale, Height * Scale - Radius * Scale * 0.5);

                        pathGeometry.Figures[0].StartPoint = startArcPoint;
                        ((ArcSegment)pathGeometry.Figures[0].Segments[0]).Point = endArcPoint;
                    }

                }
            }

            OnPropertyChanged("Area");
        }
    }
}
