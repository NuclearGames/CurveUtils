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
        private Ellipse? prevPoint;

        private List<Vector2> _newPoints = new List<Vector2>();

        private readonly float _accuracy = 0.001f;
        private readonly int _cellSize = 10;

        private bool isExistCurve = false;

        public MainWindow() {
            InitializeComponent();
            Grid.DrawGrid(CanvasXY);
            Subscribe();
        }

        private void Subscribe() {
            Drawer.onCurveDrawn += CurveExistHandler;
        }

        // а как? 
        public static void Discribe() {
            
        }

        private void CurveExistHandler(bool value) {
            isExistCurve = value;
        }

        #region Events
        private void CanvasXY_MouseDown(object sender, MouseButtonEventArgs e) {

            Vector2 point = MousePositionNormalize(e.GetPosition(CanvasXY));

            Drawer.DrawPoint(point, Brushes.Black, CanvasXY);
            _newPoints.Add(point);
        }

        private void DrawCurve_Click(object sender, RoutedEventArgs e) {
            
            BezierNode bezierNode = new BezierNode();

            bezierNode.Nodes.AddRange(_newPoints);

            _Curve = new Bezier(bezierNode, _accuracy);
            Drawer.DrawCurve(_Curve.GetBezierCurve(), CanvasXY);

        }

        private void CanvasXY_MouseMove(object sender, MouseEventArgs e) {

            Vector2 point = MousePositionNormalize(e.GetPosition(CanvasXY));

            labelX.Content = "X: " + point.X;
            labelY.Content = "Y: " + (CanvasXY.Height - point.Y);

            if (_Curve == null) {
                return;
            }

            if(_Curve.GetBezierCurve().Count <= 1) {
                return;
            }

            if (!isExistCurve) {
                return;
            }

            float Y = _Curve.Evaluate(point.X);

            LabelOutputY.Content = "OutputY: " + (CanvasXY.Height - Y);

            CanvasXY.Children.Remove(prevPoint);
            prevPoint = Drawer.DrawPoint(new Vector2(point.X, Y), Brushes.Blue, CanvasXY);
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
            Grid.DrawGrid(CanvasXY);

            CurveExistHandler(false);
  
        }

        private Vector2 MousePositionNormalize(System.Windows.Point point) {

            point.X = Math.Round(point.X / _cellSize) * _cellSize;
            point.Y = Math.Round(point.Y / _cellSize) * _cellSize;

            return new Vector2((float)point.X, (float)point.Y);
        }

        private async void Deserialize_Click(object sender, RoutedEventArgs e) {

            BezierNode bezierNode = await CurveConverter.Deserialize();

            foreach (var point in bezierNode.Nodes) {
                Drawer.DrawPoint(point, Brushes.Black, CanvasXY);
            }

            _Curve = new Bezier(bezierNode, _accuracy);
            Drawer.DrawCurve(_Curve.GetBezierCurve(), CanvasXY);
            

        }

        private void Button_Click(object sender, RoutedEventArgs e) {

            BezierNode bezierNode = new BezierNode();
            bezierNode.Nodes.AddRange(_newPoints);

            CurveConverter.Serialize(bezierNode);
        }
    }
    #endregion
}

