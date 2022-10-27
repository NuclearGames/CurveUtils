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

        private Bezier? _curve;
        private Ellipse? _prevPoint;

        private List<Vector2> _newPoints = new List<Vector2>();

        private readonly float _accuracy = 0.001f;
        private readonly int _cellSize = 10;

        private bool _isExistCurve = false;
        private bool _isEnableActivateRedrawing = false;

        

        public MainWindow() {
            InitializeComponent();
            Grid.DrawGrid(CanvasXY);
            Subscribe();
        }

        private void Subscribe() {
            Drawer.onCurveDrawn += CurveExistHandler;
        }

        public void Discribe() {
            Drawer.onCurveDrawn -= CurveExistHandler;
        }

        private void CurveExistHandler(bool value) {

            _isExistCurve = value;

        }
        private void ActiveRedrawing() {

            if (_newPoints.Count >= 3 && _isEnableActivateRedrawing) {

                Clear();

                Drawer.DrawPoints(_newPoints, Brushes.Black, CanvasXY);
                BezierNode bezierNode = new BezierNode();

                bezierNode.Nodes.AddRange(_newPoints);

                _curve = new Bezier(bezierNode, _accuracy);
                Drawer.DrawCurve(_curve.GetBezierCurve(), CanvasXY);

            }
        }
        private void Clear() {

            CanvasXY.Children.Clear();
            Grid.DrawGrid(CanvasXY);

        }

        #region Events

        private void CanvasXY_MouseDown(object sender, MouseButtonEventArgs e) {

            Vector2 point = MousePositionNormalize(e.GetPosition(CanvasXY));
            if(point.Y == float.NaN) {
                return;
            } 

            Drawer.DrawPoint(point, Brushes.Black, CanvasXY);
            _newPoints.Add(point);
            ActiveRedrawing();
        }

        private void DrawCurve_Click(object sender, RoutedEventArgs e) {
            
            BezierNode bezierNode = new BezierNode();

            bezierNode.Nodes.AddRange(_newPoints);

            _curve = new Bezier(bezierNode, _accuracy);
            Drawer.DrawCurve(_curve.GetBezierCurve(), CanvasXY);

        }

        private void CanvasXY_MouseMove(object sender, MouseEventArgs e) {

            Vector2 point = MousePositionNormalize(e.GetPosition(CanvasXY));

            labelX.Content = "X: " + point.X;
            labelY.Content = "Y: " + (CanvasXY.Height - point.Y);

            if (_curve == null) {
                return;
            }

            if(_curve.GetBezierCurve().Count <= 1) {
                return;
            }

            if (!_isExistCurve) {
                return;
            }

            if (point.X > _curve.MaxX()-1 || point.X < _curve.MinX()+1) {
                return;
            }

            float Y = _curve.Evaluate(point.X);
            

            LabelOutputY.Content = "OutputY: " + (CanvasXY.Height - Y);

            CanvasXY.Children.Remove(_prevPoint);
            _prevPoint = Drawer.DrawPoint(new Vector2(point.X, Y), Brushes.Blue, CanvasXY);
        }


        private void CanvasXY_MouseRightButtonDown(object sender, MouseButtonEventArgs e) {

            Vector2 point = MousePositionNormalize(e.GetPosition(CanvasXY));

            _newPoints.Remove(new Vector2((float)point.X, (float)point.Y));
            HitTestResult result = VisualTreeHelper.HitTest(CanvasXY, new System.Windows.Point(point.X, point.Y));

            if(result == null) {
                return;
            }

            CanvasXY.Children.Remove(result.VisualHit as UIElement);

            ActiveRedrawing();
        }

        private void ClearGrid_Click(object sender, RoutedEventArgs e) {

            Clear();
            _newPoints.Clear();

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

            _curve = new Bezier(bezierNode, _accuracy);
            Drawer.DrawCurve(_curve.GetBezierCurve(), CanvasXY);
            

        }

        private void Serialize_Click(object sender, RoutedEventArgs e) {

            BezierNode bezierNode = new BezierNode();
            bezierNode.Nodes.AddRange(_newPoints);

            CurveConverter.Serialize(bezierNode);
        }

        private void CheckBox_Switcher(object sender, RoutedEventArgs e) {

            if (_isEnableActivateRedrawing) {

                _isEnableActivateRedrawing = false;

            } else {

                _isEnableActivateRedrawing = true;

            }
        }

        private void Window_Closed(object sender, EventArgs e) {
            Discribe();
        }
    }
    #endregion
}

