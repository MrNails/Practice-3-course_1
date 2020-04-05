using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Geometry_Figures
{
    /*
     * FirstSide - левая сторона
     * SecondSide - основа
     * ThirdSide - правая сторона
     */
    public abstract class MyTriangle : GeometryFigure2D
    {
        private float firstSide;
        private float secondSide;
        private float thirdSide;

        public MyTriangle() : this(null, 40, 40, 40, null, Colors.Black)
        {
        }
        protected MyTriangle(string name, float firstSide, float secondSide, float thirdSide, 
            Canvas paintField, Color penColor, Point currentPoint = default(Point), bool isFilled = false)
            : base(name, paintField, penColor, currentPoint, isFilled)
        {
            FirstSide = firstSide;
            SecondSide = secondSide;
            ThirdSide = thirdSide;

            float perimetr = (FirstSide + SecondSide + ThirdSide) / 2;

            Height = (float)(2 * (Math.Sqrt(perimetr * (perimetr - FirstSide) * (perimetr - SecondSide) * (perimetr - ThirdSide)) / FirstSide));
            width = SecondSide;

            figure.Width = SecondSide * Scale;
            figure.Height = Height * Scale;
        }

        public override void Scaling(float delta)
        {
            base.Scaling(delta);

            float perimetr = (FirstSide + SecondSide + ThirdSide) / 2;

            Height = (float)(2 * (Math.Sqrt(perimetr * (perimetr - FirstSide) * (perimetr - SecondSide) * (perimetr - ThirdSide)) / FirstSide));
        }

        public override float Area { get { return (FirstSide * Scale * Height * Scale) / 2; } }

        public float FirstSide
        {
            get { return firstSide; }
            set 
            {
                if (value > 0)
                {
                    firstSide = value;
                    OnPropertyChanged("FirstSide");
                } 
                else
                {
                    throw new ArgumentException("Не правильный ввод первой стороны");
                }

            }
        }
        public float SecondSide
        {
            get { return secondSide; }
            set
            {
                if (value > 0)
                {
                    secondSide = value;
                    OnPropertyChanged("SecondSide");
                }
                else
                {
                    throw new ArgumentException("Не правильный ввод второй стороны");
                }

            }
        }
        public float ThirdSide
        {
            get { return thirdSide; }
            set
            {
                if (value > 0)
                {
                    thirdSide = value;
                    OnPropertyChanged("ThirdSide");
                }
                else
                {
                    throw new ArgumentException("Не правильный ввод третьей стороны");
                }

            }
        }
        public float Height
        {
            get { return height; }
            private set
            {
                if (value > 0)
                {
                    height = value;
                    OnPropertyChanged("Height");
                }
                else
                {
                    throw new ArgumentException("Невозможно посчитать высоту");
                }

            }
        }
    }

    public class MyEquilateralTriangle : MyTriangle
    {
        public MyEquilateralTriangle() : this(null, 40, null, Colors.Black)
        {
        }

        public MyEquilateralTriangle(string name, float side,
            Canvas paintField, Color penColor, Point currentPoint = default(Point), bool isFilled = false) 
            : base(name, side, side, side, paintField, penColor, currentPoint, isFilled)
        { }

        public override float Area { get { return (FirstSide * Scale * Height * Scale) / 2; } }

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

            PathFigure triangle = new PathFigure(new Point((SecondSide * Scale) / 2, 0), new List<PathSegment> {
                    new LineSegment(new Point(0, Height * Scale), true),
                    new LineSegment(new Point(SecondSide * Scale, Height * Scale), true)
            }, true);

            pathGeometry.Figures.Add(triangle);

            path.Data = pathGeometry;
            path.Width = SecondSide * Scale;
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

                    Point startPoint = new Point((SecondSide * Scale) / 2, 0);
                    Point point1 = new Point(0, Height * Scale);
                    Point endPoint = new Point(SecondSide * Scale, Height * Scale);

                    path.Width = SecondSide * Scale;
                    path.Height = Height * Scale;

                    pathGeometry.Figures[0].StartPoint = startPoint;

                    ((LineSegment)pathGeometry.Figures[0].Segments[0]).Point = point1;
                    ((LineSegment)pathGeometry.Figures[0].Segments[1]).Point = endPoint;
                }
            }

            OnPropertyChanged("Area");
        }
    }

    public class MyRightTriangle : MyTriangle
    {
        public MyRightTriangle() : this(null, 40, 40, 40, null, Colors.Black)
        {
        }

        public MyRightTriangle(string name, float firstSide, float secondSide, float thirdSide,
            Canvas paintField, Color penColor, Point currentPoint = default(Point), bool isFilled = false)
            : base(name, firstSide, secondSide, thirdSide, paintField, penColor, currentPoint, isFilled)
        { }

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

            PathFigure triangle = new PathFigure(new Point(0, 0), new List<PathSegment> {
                    new LineSegment(new Point(0, Height * Scale), true),
                    new LineSegment(new Point(SecondSide * Scale, Height * Scale), true)
            }, true);

            pathGeometry.Figures.Add(triangle);

            path.Data = pathGeometry;
            path.Width = SecondSide * Scale;
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

                    Point startPoint = new Point(0, 0);
                    Point point1 = new Point(0, Height * Scale);
                    Point endPoint = new Point(SecondSide * Scale, Height * Scale);

                    path.Width = SecondSide * Scale;
                    path.Height = Height * Scale;

                    pathGeometry.Figures[0].StartPoint = startPoint;

                    ((LineSegment)pathGeometry.Figures[0].Segments[0]).Point = point1;
                    ((LineSegment)pathGeometry.Figures[0].Segments[1]).Point = endPoint;
                }
            }

            OnPropertyChanged("Area");
        }
    }

    public class MyIsoscelesTriangle : MyTriangle
    {
        public MyIsoscelesTriangle() : this(null, 40, 40, null, Colors.Black)
        {
        }

        public MyIsoscelesTriangle(string name, float side, float baseSide,
            Canvas paintField, Color penColor, Point currentPoint = default(Point), bool isFilled = false)
            : base(name, side, baseSide, side, paintField, penColor, currentPoint, isFilled)
        { }

        public override float Area { get { return (FirstSide * Scale * Height * Scale) / 2; } }

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

            PathFigure triangle = new PathFigure(new Point((SecondSide * Scale) / 2, 0), new List<PathSegment> {
                    new LineSegment(new Point(0, Height * Scale), true),
                    new LineSegment(new Point(SecondSide * Scale, Height * Scale), true)
            }, true);

            pathGeometry.Figures.Add(triangle);

            path.Data = pathGeometry;
            path.Width = SecondSide * Scale;
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

                    Point startPoint = new Point((SecondSide * Scale) / 2, 0);
                    Point point1 = new Point(0, Height * Scale);
                    Point endPoint = new Point(SecondSide * Scale, Height * Scale);

                    path.Width = SecondSide * Scale;
                    path.Height = Height * Scale;

                    pathGeometry.Figures[0].StartPoint = startPoint;

                    ((LineSegment)pathGeometry.Figures[0].Segments[0]).Point = point1;
                    ((LineSegment)pathGeometry.Figures[0].Segments[1]).Point = endPoint;
                }
            }

            OnPropertyChanged("Area");
        }
    }
}
