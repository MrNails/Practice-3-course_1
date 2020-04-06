using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Geometry_Figures
{
    public abstract class GeometryFigure2D : GeometryFigure
    {
        public GeometryFigure2D() 
        {
        }
        public GeometryFigure2D(string name, Canvas paintField, Color penColor, Point currentPoint = default(Point), bool isFilled = false)
            : base(name, paintField, penColor, currentPoint, isFilled)
        {
        }
    }

}
