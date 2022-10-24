using BezierCurveLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Numerics;
using System.Drawing;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography;

namespace CurveBuilder {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        
        private List<Vector2> _points = new List<Vector2>();
        private List<Vector2> curvePoints = new List<Vector2>();
        private int _cellSize = 10;

        private bool isCurveExist = false;
        private Ellipse? prevPoint;

        public MainWindow() {
            InitializeComponent();
            DrawGrid();
        }

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

        private List<Vector2> CalculateCurve() {

            Bezier curve = new Bezier();
            curvePoints = curve.GetBezierCurve(_points, 0.001f);

            CurveConverter.Serialize(curve);

            return curvePoints;
        }
        private void DrawCurve(List<Vector2> curve) {

            for (int i = 0; i < curve.Count; i++) {
                DrawPoint(curve[i], Brushes.Red);
            }

            isCurveExist = true;
        }

        private void CanvasXY_MouseDown(object sender, MouseButtonEventArgs e) {
            Vector2 point = MousePositionNormalize(e.GetPosition(CanvasXY));

            DrawPoint(point, Brushes.Black);
            _points.Add(point);
        }

        private void DrawCurve_Click(object sender, RoutedEventArgs e) {
            DrawCurve(CalculateCurve());
        }

        private void CanvasXY_MouseMove(object sender, MouseEventArgs e) {

            Vector2 point = MousePositionNormalize(e.GetPosition(CanvasXY));

            labelX.Content = "X: " + point.X;
            labelY.Content = "Y: " + (CanvasXY.Height - point.Y);

            if (!isCurveExist) {
                return;
            }

            float Y = new Bezier().GetYFromBezier(point.X, curvePoints);

            LabelOutputY.Content = "OutputY: " + (CanvasXY.Height - Y);

            CanvasXY.Children.Remove(prevPoint);
            prevPoint = DrawPoint(new Vector2(point.X, Y), Brushes.Blue);
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

        private void CanvasXY_MouseRightButtonDown(object sender, MouseButtonEventArgs e) {

            Vector2 point = MousePositionNormalize(e.GetPosition(CanvasXY));

            _points.Remove(new Vector2((float)point.X, (float)point.Y));
            HitTestResult result = VisualTreeHelper.HitTest(CanvasXY, new System.Windows.Point(point.X, point.Y));

            if(result == null) {
                return;
            }

            CanvasXY.Children.Remove(result.VisualHit as UIElement);
        }
        private void ClearGrid_Click(object sender, RoutedEventArgs e) {
            CanvasXY.Children.Clear();
            _points.Clear();
            DrawGrid();
            isCurveExist = false;   
        }

        private Vector2 MousePositionNormalize(System.Windows.Point point) {

            point.X = Math.Round(point.X / _cellSize) * _cellSize;
            point.Y = Math.Round(point.Y / _cellSize) * _cellSize;

            return new Vector2((float)point.X, (float)point.Y);
        }

        private async void Deserialize_Click(object sender, RoutedEventArgs e) {
            Bezier curve = await CurveConverter.Deserialize();
            DrawCurve(curve.Points);
        }

    }
}

