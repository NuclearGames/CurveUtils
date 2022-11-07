using BezierCurveLib;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Numerics;
using CurveBuilder.Uilts;
using System.Windows.Controls;

namespace CurveBuilder {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private const float ACCURACY = 0.001f;
        private const int CELL_SIZE = 10;
        
        private BezierCurve? _curveObject;
        private Ellipse? _prevPoint;

        private List<Vector2> _newPoints = new List<Vector2>();

        private bool _isExistCurve = false;
        private bool _isEnableActivateRedrawing = false;

        public MainWindow() {
            InitializeComponent();
            Grid.DrawGrid(CanvasXY);
            Width.Text = CanvasXY.Width.ToString();
            Height.Text = CanvasXY.Height.ToString();
            Subscribe();
        }

        private void Subscribe() {
            Drawer.onCurveDrawn += CurveExistHandler;
        }

        public void Unsubscribe() {
            Drawer.onCurveDrawn -= CurveExistHandler;
        }

        private void CurveExistHandler(bool value) {

            _isExistCurve = value;

        }
        private void ActiveRedrawing() {

            if (_newPoints.Count >= 3 && _isEnableActivateRedrawing) {

                Clear();

                Drawer.DrawPoints(_newPoints, Brushes.Black, CanvasXY);
                BezierCurveSourceModel bezierCurveSourceModel = new BezierCurveSourceModel {
                    Nodes = new List<Vector2>(_newPoints),
                    Width = (float)CanvasXY.Width,
                    Height = (float)CanvasXY.Height,
                };

                _curveObject = BezierCurveBuilder.Build(bezierCurveSourceModel, ACCURACY);
                Drawer.DrawCurve(_curveObject, CanvasXY);

            }
        }
        private void Clear() {

            CanvasXY.Children.Clear();
            _curveObject = new BezierCurve(new List<Vector2>());
            Grid.DrawGrid(CanvasXY);

        }

        #region Events

        private void CanvasXY_MouseDown(object sender, MouseButtonEventArgs e) {

            Vector2 point = MousePositionNormalize(e.GetPosition(CanvasXY));
            if(point.Y.CompareTo(float.NaN) == 0) {
                return;
            } 

            Drawer.DrawPoint(point, Brushes.Black, CanvasXY);
            _newPoints.Add(point);
            ActiveRedrawing();
        }

        private void DrawCurve_Click(object sender, RoutedEventArgs e) {

            BezierCurveSourceModel bezierCurveSourceModel = new BezierCurveSourceModel {
                Nodes = new List<Vector2>(_newPoints),
                Width = (float)CanvasXY.Width,
                Height = (float)CanvasXY.Height,
            };

            _curveObject = BezierCurveBuilder.Build(bezierCurveSourceModel, ACCURACY);
            Drawer.DrawCurve(_curveObject, CanvasXY);

        }

        private void CanvasXY_MouseMove(object sender, MouseEventArgs e) {

            Vector2 point = MousePositionNormalize(e.GetPosition(CanvasXY));

            labelX.Content = "X: " + point.X;
            labelY.Content = "Y: " + (1 - point.Y);

            if (_curveObject == null) {
                return;
            }

            if(_curveObject.GetBezierCurve().Count <= 1) {
                return;
            }

            if (!_isExistCurve) {
                return;
            }

            if (point.X > _curveObject.MaxX() - ACCURACY || point.X < _curveObject.MinX() + ACCURACY) {
                return;
            }

            float y = _curveObject.Evaluate(point.X);
            

            LabelOutputY.Content = "OutputY: " + Math.Round(y*CanvasXY.Height);

            CanvasXY.Children.Remove(_prevPoint);
            _prevPoint = Drawer.DrawPoint(point with { Y = y }, Brushes.Blue, CanvasXY);
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

            point.X = Math.Round((point.X / CanvasXY.Width) / (CELL_SIZE /CanvasXY.Width)) * (CELL_SIZE / CanvasXY.Width);
            point.Y = Math.Round((point.Y / CanvasXY.Height) / (CELL_SIZE / CanvasXY.Height)) * (CELL_SIZE / CanvasXY.Height);

            return new Vector2((float)point.X, (float)point.Y);
        }

        private void Deserialize_Click(object sender, RoutedEventArgs e) {

            BezierCurveSourceModel bezierCurveSourceModel = CurveConverter.Deserialize();

            foreach (var point in bezierCurveSourceModel.Nodes) {
                Drawer.DrawPoint(point, Brushes.Black, CanvasXY);
            }

            _curveObject = BezierCurveBuilder.Build(bezierCurveSourceModel, ACCURACY);
            Drawer.DrawCurve(_curveObject, CanvasXY);
            

        }

        private void Serialize_Click(object sender, RoutedEventArgs e) {
            BezierCurveSourceModel bezierCurveSourceModel = new BezierCurveSourceModel {
                Nodes = new List<Vector2>(_newPoints),
                Width = (float)CanvasXY.Width,
                Height = (float)CanvasXY.Height,
            };

            CurveConverter.Serialize(bezierCurveSourceModel);
        }

        private void CheckBox_Switcher(object sender, RoutedEventArgs e) {
            _isEnableActivateRedrawing = !_isEnableActivateRedrawing;
        }

        private void Window_Closed(object sender, EventArgs e) {
            Unsubscribe();
        }

        private void CalculateRelation_Click(object sender, RoutedEventArgs e) {
            CanvasXY.Width = Convert.ToDouble(Width.Text);
            CanvasXY.Height = Convert.ToDouble(Height.Text);
            Clear();
            Grid.DrawGrid(CanvasXY);

            if(_curveObject != null) {
                Drawer.DrawCurve(_curveObject, CanvasXY);
            }

            if(_newPoints.Count > 0) {
                Drawer.DrawPoints(_newPoints, Brushes.Black, CanvasXY);
            }
        }

    }
    #endregion
}

