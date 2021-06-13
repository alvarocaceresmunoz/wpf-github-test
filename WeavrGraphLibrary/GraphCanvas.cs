using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using WeavrGraphLibrary.DataProcessing;
using WeavrGraphLibrary.DataStructures;

namespace WeavrGraphLibrary
{
    public class GraphCanvas : Canvas
    {
        List<TimeSeries<double>> data = new List<TimeSeries<double>>();
        Dictionary<TimeSeries<double>, SeriesVisual> visuals = new Dictionary<TimeSeries<double>, SeriesVisual>();

        class SeriesVisual
        {
            public Path Path { get; set; }
            public PathGeometry Geometry { get; set; }
            public PathFigure Figure { get; set; }
            public int ProcessedIndex { get; set; } = 0;

            public SeriesVisual()
            {
                Path = new Path() { Stroke = Brushes.Yellow, StrokeThickness = 1, Effect = new DropShadowEffect {
                    ShadowDepth = 0,
                    Color = Colors.Yellow,
                    BlurRadius = 15
                } };
                Geometry = new PathGeometry();
                Figure = new PathFigure() { IsClosed = false };
                Geometry.Figures.Add(Figure);
                Path.Data = Geometry;
                Path.MouseEnter += Path_MouseEnter;
                Path.MouseLeave += Path_MouseLeave;
            }

            private void Path_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
            {
                Path.Stroke = Brushes.Yellow;
            }

            private void Path_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
            {
                Path.Stroke = Brushes.Orange;
            }
        }

        public GraphCanvas()
        {
            Background = Brushes.Transparent;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
        }

        public void AddTimeSeries(TimeSeries<double> series)
        {
            data.Add(series);
            InitialiseVisuals(series);
            DataWatcher.Subscribe(this, series, new Action(OnDataChanged));
        }

        void InitialiseVisuals(TimeSeries<double> series)
        {
            SeriesVisual visual = new SeriesVisual();
            visuals.Add(series, visual);
            Children.Add(visual.Path);
        }

        void OnDataChanged()
        {
            foreach (TimeSeries<double> series in data)
            {
                for (int i = visuals[series].ProcessedIndex; i < series.Entries.Count; i++)
                {
                    visuals[series].Figure.Segments.Add(
                        new LineSegment(
                            new Point(series.Entries[i].Time, series.Entries[i].Value), true
                        )
                        );
                    visuals[series].ProcessedIndex = i;
                }
            }
        }
    }
}
