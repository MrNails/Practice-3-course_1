using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Geometry_Figures
{
    public abstract class GeometryFigure3D : GeometryFigure
    {
        public GeometryFigure3D()
        {
        }
        public GeometryFigure3D(string name, Canvas paintField, Color penColor, Point currentPoint = default(Point), bool isFilled = false) : base(name, paintField, penColor, currentPoint, isFilled)
        {
        }
    }
}
