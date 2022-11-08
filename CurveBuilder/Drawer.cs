using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;
using BezierCurveLib;

namespace CurveBuilder {
    internal static class Drawer {

        public static event Action<bool> onCurveDrawn;

        #region Draw

        public static Ellipse DrawPoint(Vector2 point, SolidColorBrush color, Canvas CanvasXY) {
            
            Ellipse ellipse = new Ellipse();
            ellipse.Width = 6;
            ellipse.Height = 6;
            ellipse.StrokeThickness = 4;
            ellipse.Stroke = color;
            ellipse.Margin = new Thickness(point.X * (CanvasXY.Width), point.Y * (CanvasXY.Height), 0, 0);

            CanvasXY.Children.Add(ellipse);

            return ellipse;
        }

        public static void DrawPoints(List<Vector2> points, SolidColorBrush color, Canvas CanvasXY) {
            foreach (var point in points) {
                DrawPoint(point, color, CanvasXY);
            }
        }

        public static Line DrawLine(Vector2 from, Vector2 to, SolidColorBrush color, Canvas CanvasXY) {

            Line line = new Line();

            line.StrokeThickness = 4;
            line.Stroke = color;

            line.X1 = from.X;
            line.Y1 = from.Y;

            line.X2 = to.X;
            line.Y2 = to.Y;

            CanvasXY.Children.Add(line);

            return line;

        }

        public static void DrawCurve(BezierCurve curve, BezierCurveSourceModel curveModel , Canvas CanvasXY) {

            for (int i = 0; i < curve.Points.Count - 1; i++) {
                DrawLine( new Vector2(curve.Points[i].X * (float)CanvasXY.Width, curve.Points[i].Y * (float)CanvasXY.Height) , 
                          new Vector2(curve.Points[i+1].X * (float)CanvasXY.Width, curve.Points[i+1].Y * (float)CanvasXY.Height), Brushes.Red, CanvasXY);
            }
            onCurveDrawn?.Invoke(true);
        }

        #endregion
    }
}
