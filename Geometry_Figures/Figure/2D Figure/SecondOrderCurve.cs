using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Geometry_Figures
{
    public abstract class MySecondOrderCurves : GeometryFigure2D
    {
        private float radiusX;
        private float radiusY;

        public MySecondOrderCurves() : this(null, 5, 5, null, Colors.Black)
        {
        }
        protected MySecondOrderCurves(string name, float radiusX, float radiusY, Canvas paintField, Color penColor, Point currentPoint = default(Point), bool isFilled = false)
            : base(name, paintField, penColor, currentPoint, isFilled)
        {
            RadiusX = radiusX;
            RadiusY = radiusY;

            width = radiusX * 2 * Scale;
            height = radiusY * 2 * Scale;

            figure.Width = width * Scale;
            figure.Height = height * Scale;
        }

        public override float Area { get { return (float)Math.PI * RadiusX * RadiusY * Scale * Scale; }  }

        public float RadiusX
        {
            get { return radiusX; }
            set 
            {
                if (value > 0)
                {
                    radiusX = value;
                    OnPropertyChanged("RadiusX");
                }
                else
                {
                    radiusX = 1;
                }
            }
        }
        public float RadiusY
        {
            get { return radiusY; }
            set
            {
                if (value > 0)
                {
                    radiusY = value;
                    OnPropertyChanged("RadiusY");
                }
                else
                {
                    radiusY = 1;
                }
            }
        }

    }

    public class MyCircle : MySecondOrderCurves
    {
        public MyCircle() : this(null, 5, null, Colors.Black)
        {
        }

        public MyCircle(string name, float radius, Canvas paintField, Color penColor, Point currentPoint = default(Point), bool isFilled = false)
            :base(name, radius, radius, paintField, penColor, currentPoint, isFilled)
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

            path.Data = new EllipseGeometry(new Point(RadiusX * Scale, RadiusY * Scale), RadiusX, RadiusY);
            path.Width = RadiusX * 2 * Scale;
            path.Height = RadiusY * 2 * Scale;
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
                    EllipseGeometry ellipse = (EllipseGeometry)path.Data;

                    path.Width = width * Scale;
                    path.Height = height * Scale;

                    ellipse.Center = new Point(RadiusX * Scale, RadiusY * Scale);
                    ellipse.RadiusX = RadiusX * Scale;
                    ellipse.RadiusY = RadiusY * Scale;

                }
            }

            OnPropertyChanged("Area");
        }
    }

    public class MyEllipse : MySecondOrderCurves
    {
        public MyEllipse() : this(null, 5, 5, null, Colors.Black)
        {
        }

        public MyEllipse(string name, float radiusX, float radiusY, Canvas paintField, Color penColor, Point currentPoint = default(Point), bool isFilled = false)
            : base(name, radiusX, radiusY, paintField, penColor, currentPoint, isFilled)
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

            path.Data = new EllipseGeometry(new Point(RadiusX * Scale, RadiusY * Scale), RadiusX, RadiusY);
            path.Width = RadiusX * 2 * Scale;
            path.Height = RadiusY * 2 * Scale;
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
                    EllipseGeometry ellipse = (EllipseGeometry)path.Data;

                    path.Width = width * Scale;
                    path.Height = height * Scale;

                    ellipse.Center = new Point(RadiusX * Scale, RadiusY * Scale);
                    ellipse.RadiusX = RadiusX * Scale;
                    ellipse.RadiusY = RadiusY * Scale;

                }
            }

            OnPropertyChanged("Area");
        }
    }
}
