using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Geometry_Figures
{
    public abstract class GeometryFigure : INotifyPropertyChanged
    {
        private float scale;
        private Point currentPoint;

        protected Canvas figure;
        protected float width;
        protected float height;
        protected bool isDrawed;
        protected bool isFilled;

        public GeometryFigure() : this(null, null, Colors.Black)
        {}
        protected GeometryFigure(string name, Canvas paintField, Color penColor, Point currentPoint = default(Point), bool isFilled = false)
        {
            Name = name;
            PenColor = penColor;
            scale = 1;
            this.isFilled = isFilled;

            MainField = paintField;

            figure = new Canvas();

            this.currentPoint = currentPoint;

            figure.DataContext = this;
        }

        public Canvas Figure { get { return figure; } }
        public Point CurrentPoint { get { return currentPoint; } }
        public bool IsFilled { get { return isFilled; } }

        public string Name { get; set; }
        public Canvas MainField { get; set; }
        public Color PenColor { get; set; }

        public float Scale
        {
            get { return scale; }
            set
            {
                if (value > 0.01)
                {
                    scale = value;
                    
                } else
                {
                    scale = 0.1f;
                }
                OnPropertyChanged("Scale");
            }
        }

        public abstract float Area { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        public abstract void Draw();

        public virtual void Scaling(float delta)
        {
            Scale += delta;

            figure.Width = width * Scale;
            figure.Height = height * Scale;
        }

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public void Move(Point newPoint)
        {

            if (newPoint.Y < 0)
            {
                newPoint.Y = 0;
            }
            else if (newPoint.Y + height > MainField.ActualHeight)
            {
                newPoint.Y = MainField.ActualHeight - height;
            }

            if (newPoint.X < 0)
            {
                newPoint.X = 0;
            }
            else if (newPoint.X + width > MainField.ActualWidth)
            {
                newPoint.X = MainField.ActualWidth - width;
            }

            currentPoint = newPoint;

            Canvas.SetTop(figure, newPoint.Y);
            Canvas.SetLeft(figure, newPoint.X);
        }
    }
}
