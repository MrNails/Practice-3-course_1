using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Geometry_Figures
{
    public abstract class MyQuadrangles : GeometryFigure2D
    {

        public MyQuadrangles()
        {
        }
        protected MyQuadrangles(string name, float width, float height, Canvas paintField, Color penColor, Point currentPoint = default(Point), bool isFilled = false) 
            : base(name, paintField, penColor, currentPoint, isFilled)
        {
            Width = width;
            Height = height;

            figure.Width = Width * Scale;
            figure.Height = Height * Scale;
        }

        public float Width
        {
            get { return width; }
            set
            {
                if (value > 0)
                {
                    width = value;
                    OnPropertyChanged("Width");
                }
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
                    OnPropertyChanged("Height");
                }
            }
        }

    }

    public class MyRectangle : MyQuadrangles
    {
        public MyRectangle() : this(null, 50, 50, null, Colors.Black)
        {
        }
        public MyRectangle(string name, float width, float height, Canvas paintField, Color penColor, Point currentPoint = default(Point), bool isFilled = false)
            : base(name, width, height, paintField, penColor, currentPoint, isFilled)
        {
        }

        public override float Area
        {
            get { return Width * Scale * Height * Scale; }
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

            figure.Children.Add(path);

            TextBlock trigger = new TextBlock();
            trigger.Width = width * Scale;
            trigger.Height = height * Scale;
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

    public class MyRhombus : MyQuadrangles
    {
        public MyRhombus() : this(null, 50, 50, null, Colors.Black)
        {
        }
        public MyRhombus(string name, float width, float height, Canvas paintField, Color penColor, Point currentPoint = default(Point), bool isFilled = false)
            : base(name, width, height, paintField, penColor, currentPoint, isFilled)
        {
        }

        public override float Area
        {
            get { return (Width * Scale * Height * Scale) / 2; }
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
            PathGeometry pathGeometry = new PathGeometry();

            PathFigure topTriangle = new PathFigure(new Point((Width * Scale) / 2, 0), new List<PathSegment> {
                    new LineSegment(new Point(Width * Scale, (Height * Scale) / 2), true),
                    new LineSegment(new Point((Width * Scale) / 2, Height * Scale), true),
                    new LineSegment(new Point(0, (Height * Scale) / 2), true)
            }, true);


            pathGeometry.Figures.Add(topTriangle);

            path.Data = pathGeometry;
            path.Width = Width * Scale;
            path.Height = Height * Scale;
            path.Stroke = new SolidColorBrush(PenColor);
            path.StrokeThickness = 1;


            if (IsFilled)
            {
                path.Fill = new SolidColorBrush(PenColor);
            }

            TextBlock trigger = new TextBlock();
            trigger.Width = width * Scale;
            trigger.Height = height * Scale;
            figure.Children.Add(trigger);

            figure.Children.Add(path);

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
                    PathGeometry pathGeometry = (PathGeometry)path.Data;

                    Point startPoint = new Point((Width * Scale) / 2, 0);
                    Point point1 = new Point(Width * Scale, (Height * Scale) / 2);
                    Point point2 = new Point((Width * Scale) / 2, Height * Scale);
                    Point endPoint = new Point(0, (Height * Scale) / 2);

                    path.Width = Width * Scale;
                    path.Height = Height * Scale;

                    pathGeometry.Figures[0].StartPoint = startPoint;

                    ((LineSegment)pathGeometry.Figures[0].Segments[0]).Point = point1;
                    ((LineSegment)pathGeometry.Figures[0].Segments[1]).Point = point2;
                    ((LineSegment)pathGeometry.Figures[0].Segments[2]).Point = endPoint;
                }
            }

            OnPropertyChanged("Area");
        }
    }

    public class MyTrapeze : MyQuadrangles
    {
        private float topLineWidth;

        public MyTrapeze() : this(null, 50, 50, 50, null, Colors.Black)
        {
        }
        public MyTrapeze(string name, float topLineWidth, float bottomLineWidth, float height,
                        Canvas paintField, Color penColor, Point currentPoint = default(Point), bool isFilled = false)
            : base(name, bottomLineWidth, height, paintField, penColor, currentPoint, isFilled)
        {
            TopLineWidth = topLineWidth;
        }

        public float TopLineWidth
        {
            get { return topLineWidth; }
            set
            {
                if (value > 0 && value <= Width)
                {
                    topLineWidth = value;
                    OnPropertyChanged("TopLineWidth");
                } else
                {
                    topLineWidth = Width;
                }
               
            }
        }

        public override float Area
        {
            get { return ((Width * Scale + TopLineWidth * Scale) / 2) * Height; }
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
            PathGeometry pathGeometry = new PathGeometry();

            float topLineX = (Width * Scale - TopLineWidth * Scale) / 2;

            PathFigure trapeze = new PathFigure(new Point(topLineX, 0), new List<PathSegment> {
                    new LineSegment(new Point(topLineX + TopLineWidth * Scale, 0), true),
                    new LineSegment(new Point(Width * Scale, Height * Scale), true),
                    new LineSegment(new Point(0, Height * Scale), true),
            }, true);

            pathGeometry.Figures.Add(trapeze);

            path.Data = pathGeometry;
            path.Width = Width * Scale;
            path.Height = Height * Scale;
            path.Stroke = new SolidColorBrush(PenColor);
            path.StrokeThickness = 1;


            if (IsFilled)
            {
                path.Fill = new SolidColorBrush(PenColor);
            }

            TextBlock trigger = new TextBlock();
            trigger.Width = width * Scale;
            trigger.Height = height * Scale;
            figure.Children.Add(trigger);

            figure.Children.Add(path);

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
                    PathGeometry pathGeometry = (PathGeometry)path.Data;
                    float topLineX = (Width * Scale - TopLineWidth * Scale) / 2;

                    Point startPoint = new Point(topLineX, 0);
                    Point point1 = new Point(topLineX + TopLineWidth * Scale, 0);
                    Point point2 = new Point(Width * Scale, Height * Scale);
                    Point endPoint = new Point(0, Height * Scale);

                    path.Width = Width * Scale;
                    path.Height = Height * Scale;

                    pathGeometry.Figures[0].StartPoint = startPoint;

                    ((LineSegment)pathGeometry.Figures[0].Segments[0]).Point = point1;
                    ((LineSegment)pathGeometry.Figures[0].Segments[1]).Point = point2;
                    ((LineSegment)pathGeometry.Figures[0].Segments[2]).Point = endPoint;
                }
            }

            OnPropertyChanged("Area");
        }
    }
}
