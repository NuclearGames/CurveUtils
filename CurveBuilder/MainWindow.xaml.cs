using BezierCurveLib;
using BezierCurveLib.Utils;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Numerics;
using CurveBuilder.Uilts;
using System.Windows.Controls;
using System.Drawing;

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

        private float _widthRelation = 1;
        private float _heightRelation = 1;



        public MainWindow() {
            InitializeComponent();
            CanvasXY.Width = 1280;
            CanvasXY.Height = 720;
            Width.Text = CanvasXY.Width.ToString();
            Height.Text = CanvasXY.Height.ToString();
            Grid.DrawGrid(CanvasXY);
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
                    ratioWidth = _widthRelation,
                    ratioHeight = _heightRelation,
                };

                _curveObject = BezierCurveBuilder.Build(bezierCurveSourceModel, ACCURACY);
                Drawer.DrawCurve(_curveObject, CanvasXY);

            }
        }
        private Vector2 MousePositionNormalize(System.Windows.Point point) {
            Vector2 vector = StorageSystem.ConvertForStorage(new Vector2((float)point.X, (float)point.Y), (float)CanvasXY.Width, (float)CanvasXY.Height);
            point.X = Math.Round(vector.X / (CELL_SIZE / CanvasXY.Width)) * (CELL_SIZE / CanvasXY.Width);
            point.Y = Math.Round(vector.Y / (CELL_SIZE / CanvasXY.Height)) * (CELL_SIZE / CanvasXY.Height);

            return new Vector2((float)point.X, (float)point.Y);
        }

        private void Clear() {

            CanvasXY.Children.Clear();
            _curveObject = new BezierCurve(new List<Vector2>());
            Grid.DrawGrid(CanvasXY);

        }

        private void AbsolutePointView(Vector2 point) {
            labelX.Content = "X: " + point.X;
            labelY.Content = "Y: " + (1 - point.Y);
        }

        private void ReducedPointView(Vector2 point) {
            var canvasPoint = StorageSystem.ConvertForCanvas(point, (float)CanvasXY.Width / _widthRelation, (float)CanvasXY.Height / _heightRelation);
            AddLabelX.Content = "X: " + Math.Round(canvasPoint.X);
            AddLabelY.Content = "Y: " + Math.Round((CanvasXY.Height / _heightRelation) - canvasPoint.Y);
        }

        private void EvaluateYView(Vector2 point) {

            if (_curveObject == null) {
                return;
            }

            if (_curveObject.GetBezierCurve().Count <= 1) {
                return;
            }

            if (!_isExistCurve) {
                return;
            }
            if (point.X > _curveObject.MaxX() - ACCURACY || point.X < _curveObject.MinX() + ACCURACY) {
                return;
            }

            float y = _curveObject.Evaluate(point.X);


            LabelOutputY.Content = "OutputY: " + Math.Round((CanvasXY.Height / _heightRelation) - y * CanvasXY.Height / _heightRelation);

            CanvasXY.Children.Remove(_prevPoint);
            _prevPoint = Drawer.DrawPoint(point with { Y = y }, Brushes.Blue, CanvasXY);
        }

        #region Events

        private void CanvasXY_MouseDown(object sender, MouseButtonEventArgs e) {
            Vector2 point = MousePositionNormalize(e.GetPosition(CanvasXY));

            if (point.Y.CompareTo(float.NaN) == 0) {
                return;
            }

            Drawer.DrawPoint(point, Brushes.Black, CanvasXY);
            _newPoints.Add(point);
            ActiveRedrawing();
        }

        private void DrawCurve_Click(object sender, RoutedEventArgs e) {

            BezierCurveSourceModel bezierCurveSourceModel = new BezierCurveSourceModel {
                Nodes = new List<Vector2>(_newPoints),
                ratioWidth = _widthRelation,
                ratioHeight = _heightRelation,
            };

            _curveObject = BezierCurveBuilder.Build(bezierCurveSourceModel, ACCURACY);
            Drawer.DrawCurve(_curveObject, CanvasXY);

        }

        private void CanvasXY_MouseMove(object sender, MouseEventArgs e) {

            Vector2 point = MousePositionNormalize(e.GetPosition(CanvasXY));

            AbsolutePointView(point);
            
            ReducedPointView(point);

            EvaluateYView(point);

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
                ratioWidth = _widthRelation,
                ratioHeight = _heightRelation,
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

            var enterWidth = float.Parse(Width.Text);
            var enterHeight = float.Parse(Height.Text);


            if (CanvasXY.Width != enterWidth || CanvasXY.Height != enterWidth) {
                _widthRelation = (float)(CanvasXY.Width / enterWidth);
                _heightRelation = (float)(CanvasXY.Height / enterHeight);
            }
        }

    }
    #endregion
}

