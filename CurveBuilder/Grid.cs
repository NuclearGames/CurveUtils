using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CurveBuilder {
    internal static class Grid {

        private static int _cellSize = 10;

        #region Grid
        public static void DrawGrid(Canvas CanvasXY) {

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
    }
}
