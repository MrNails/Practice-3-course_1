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
using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace Geometry_Figures
{
    public abstract class GeometryFigure : INotifyPropertyChanged
    {
        private float width;
        private float height;
        private float scale;

        protected Canvas figure;
        protected bool mouseIsDown;
        protected bool isDrawed;

        public GeometryFigure() : this (null, 0, 0, null, Colors.Black)
        {}
        public GeometryFigure(string name, float width, float height, Canvas paintField, Color penColor)
        {
            Name = name;
            Width = width;
            Height = height;
            PenColor = penColor;
            scale = 1;

            MainField = paintField;

            figure = new Canvas();

            figure.PreviewMouseDown += (downObj, downArg) =>
            {
                mouseIsDown = true;
            };
            figure.PreviewMouseUp += (upObj, upArg) =>
            {
                mouseIsDown = false;
            };
            figure.PreviewMouseMove += (moveObj, moveArg) =>
            {
                int index = MainField.Children.IndexOf(figure);

                if (index >= 0 && mouseIsDown && figure.IsMouseOver)
                {
                    figure = (Canvas)MainField.Children[index];

                    Move(moveArg.GetPosition(MainField));
                }
            };

            figure.DataContext = this;
        }

        public Canvas Figure { get { return figure; } }

        public string Name { get; set; }
        public Canvas MainField { get; set; }
        public Color PenColor { get; set; }

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

        public event PropertyChangedEventHandler PropertyChanged;

        public abstract void Draw();
        public abstract void Scaling(float delta);
        public abstract float Area { get; }

        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public void Move(Point newPoint)
        {
            if (newPoint.Y - (Height * Scale) / 2 < 0)
            {
                newPoint.Y = 0;
            }
            else if (newPoint.Y + (Height * Scale) / 2 > MainField.ActualHeight)
            {
                newPoint.Y = MainField.ActualHeight - Height * Scale;
            }
            else
            {
                newPoint.Y -= (Height * Scale) / 2;
            }

            if (newPoint.X - (Width * Scale) / 2 < 0)
            {
                newPoint.X = 0;
            }
            else if (newPoint.X + (Width * Scale) / 2 > MainField.ActualWidth)
            {
                newPoint.X = MainField.ActualWidth - Width * Scale;
            }
            else
            {
                newPoint.X -= (Width * Scale) / 2;
            }

            Canvas.SetTop(figure, newPoint.Y);
            Canvas.SetLeft(figure, newPoint.X);
        }
    }
}
