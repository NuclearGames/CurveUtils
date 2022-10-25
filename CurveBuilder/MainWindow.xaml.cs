using BezierCurveLib;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Numerics;

namespace CurveBuilder {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        private Bezier? _Curve;
        
        private List<Vector2> _newPoints = new List<Vector2>();
        private List<Vector2> curvePoints = new List<Vector2>();
        private int _cellSize = 10;

        private Ellipse? prevPoint;

        public MainWindow() {
            InitializeComponent();
            DrawGrid();
        }
        private List<Vector2> CalculateCurve(List<Vector2> points) {

            Bezier curve = new Bezier(points);
            curvePoints = curve.GetBezierCurve(0.001f);

            return curvePoints;
        }

        #region Draw

        public Ellipse DrawPoint(Vector2 point, SolidColorBrush color) {

            Ellipse ellipse = new Ellipse();
            ellipse.Width = 6;
            ellipse.Height = 6;
            ellipse.StrokeThickness = 4;
            ellipse.Stroke = color;
            ellipse.Margin = new Thickness(point.X - 2, point.Y - 2, 0, 0);

            CanvasXY.Children.Add(ellipse);

            return ellipse;
        }

        public Line DrawLine(Vector2 from, Vector2 to, SolidColorBrush color) {

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

        private void DrawCurve(List<Vector2> curve) {

            for (int i = 0; i < curve.Count - 1; i++) {
                DrawLine(curve[i], curve[i + 1], Brushes.Red);
            }

        }

        #region Grid
        public void DrawGrid() {

            for (int coordinateX = 0; coordinateX < CanvasXY.Width / _cellSize; coordinateX++) {
                Line HLine = new Line();//hLine=highLine-линии высокие 
                HLine.X1 = coordinateX * _cellSize;
                HLine.Y1 = 0;
                HLine.X2 = coordinateX * _cellSize;
                HLine.Y2 = CanvasXY.Height;
                HLine.Stroke = Brushes.Black;//Красим линии чёрным


                HLine.StrokeThickness = 0.15;//Толщина промежуточных линий
                CanvasXY.Children.Add(HLine);//Переносим линии на convas


            }

            for (int coordinateY = 0; coordinateY < CanvasXY.Height / _cellSize; coordinateY++) {

                Line Wline = new Line();//- широкие линии
                Wline.X1 = 0;
                Wline.Y1 = coordinateY * _cellSize;
                Wline.X2 = CanvasXY.Width;
                Wline.Y2 = coordinateY * _cellSize;
                Wline.Stroke = Brushes.Black;//Красим линии чёрным
                Wline.StrokeThickness = 0.15;//Толщина основных линий
                CanvasXY.Children.Add(Wline);//Переносим линии на convas

            }
        }
        #endregion

        #endregion

        #region Events
        private void CanvasXY_MouseDown(object sender, MouseButtonEventArgs e) {

            Vector2 point = MousePositionNormalize(e.GetPosition(CanvasXY));

            DrawPoint(point, Brushes.Black);
            _newPoints.Add(point);
        }

        private void DrawCurve_Click(object sender, RoutedEventArgs e) {
            _Curve = new Bezier(_newPoints);
            DrawCurve(CalculateCurve(_Curve.Points));

        }

        private void CanvasXY_MouseMove(object sender, MouseEventArgs e) {

            Vector2 point = MousePositionNormalize(e.GetPosition(CanvasXY));

            labelX.Content = "X: " + point.X;
            labelY.Content = "Y: " + (CanvasXY.Height - point.Y);

            if (_Curve == null) {
                return;
            }

            if(_Curve.Points.Count <= 1) {
                return;
            }

            float Y = _Curve.GetY(point.X, curvePoints);

            LabelOutputY.Content = "OutputY: " + (CanvasXY.Height - Y);

            CanvasXY.Children.Remove(prevPoint);
            prevPoint = DrawPoint(new Vector2(point.X, Y), Brushes.Blue);
        }


        private void CanvasXY_MouseRightButtonDown(object sender, MouseButtonEventArgs e) {

            Vector2 point = MousePositionNormalize(e.GetPosition(CanvasXY));

            _newPoints.Remove(new Vector2((float)point.X, (float)point.Y));
            HitTestResult result = VisualTreeHelper.HitTest(CanvasXY, new System.Windows.Point(point.X, point.Y));

            if(result == null) {
                return;
            }

            CanvasXY.Children.Remove(result.VisualHit as UIElement);
        }
        private void ClearGrid_Click(object sender, RoutedEventArgs e) {

            CanvasXY.Children.Clear();
            _newPoints.Clear();
            DrawGrid();
  
        }

        private Vector2 MousePositionNormalize(System.Windows.Point point) {

            point.X = Math.Round(point.X / _cellSize) * _cellSize;
            point.Y = Math.Round(point.Y / _cellSize) * _cellSize;

            return new Vector2((float)point.X, (float)point.Y);
        }

        private async void Deserialize_Click(object sender, RoutedEventArgs e) {

            Bezier curve = await CurveConverter.Deserialize();
            foreach (var point in curve.Points) {
                DrawPoint(point, Brushes.Black);
            }
            DrawCurve(CalculateCurve(curve.Points));

        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            Bezier curve = new Bezier(_newPoints);
            CurveConverter.Serialize(curve);
        }
    }
    #endregion
}

