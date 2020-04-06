using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Geometry_Figures
{
    [Serializable]
    [XmlInclude(typeof(GeometryFigure2D))]
    [XmlInclude(typeof(GeometryFigure3D))]
    [XmlInclude(typeof(MyQuadrangles))]
    [XmlInclude(typeof(MyTriangle))]
    [XmlInclude(typeof(MySecondOrderCurves))]
    [XmlInclude(typeof(MySphere))]
    [XmlInclude(typeof(MyPyramid))]
    [XmlInclude(typeof(MyCilyndre))]
    [XmlInclude(typeof(MyIsoscelesTriangle))]
    [XmlInclude(typeof(MyEquilateralTriangle))]
    [XmlInclude(typeof(MyEllipse))]
    [XmlInclude(typeof(MyCircle))]
    [XmlInclude(typeof(MyRightTriangle))]
    [XmlInclude(typeof(MyEquilateralTriangle))]
    [XmlInclude(typeof(MyRhombus))]
    [XmlInclude(typeof(MyRectangle))]
    [XmlInclude(typeof(MyTrapeze))]
    public abstract class GeometryFigure : INotifyPropertyChanged
    {
        private float scale;
        private Point currentPoint;

        [XmlIgnore]
        [NonSerialized]
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
        public Point CurrentPoint { 
            get { return currentPoint; }
            set
            {
                currentPoint = value;

                Canvas.SetTop(figure, value.Y);
                Canvas.SetLeft(figure, value.X);
            }
        }
        public bool IsFilled {
            get { return isFilled; } 
            set
            {
                if (!isDrawed) 
                {
                    isFilled = true;
                }
            } 
        }

        public string Name { get; set; }

        [XmlIgnore]
        [field: NonSerialized]
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
        public float FigureOriginalBaseWidth
        {
            get { return width; }
            set
            {
                if (value > 0)
                {
                    width = value;

                }
                else
                {
                    width = 1f;
                }
                Scaling(0);
                OnPropertyChanged("FigureOriginalBaseWidth");
            }
        }
        public float FigureOriginalBaseHeight
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
                    height = 1f;
                }
                Scaling(0);
                OnPropertyChanged("FigureOriginalBaseHeight");
            }
        }

        public abstract float Area { get; }

        [field: XmlIgnore]
        [field: NonSerialized]
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
    }
}
