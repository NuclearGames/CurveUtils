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
            ellipse.Margin = new Thickness(point.X - 2, point.Y - 2, 0, 0);

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

        public static void DrawCurve(List<Vector2> curve, Canvas CanvasXY) {

            for (int i = 0; i < curve.Count - 1; i++) {
                DrawLine(curve[i], curve[i + 1], Brushes.Red, CanvasXY);
            }
            onCurveDrawn?.Invoke(true);
        }

        #endregion
    }
}
