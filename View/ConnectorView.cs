using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows;

namespace QuickArch.View
{
    public class ConnectorView : Shape
    {
        LineGeometry linegeo;

        public static readonly DependencyProperty StartPointProperty = DependencyProperty.Register("StartPoint", typeof(Point), typeof(ConnectorView), new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsMeasure));
        public static readonly DependencyProperty EndPointProperty = DependencyProperty.Register("EndPoint", typeof(Point), typeof(ConnectorView), new FrameworkPropertyMetadata(new Point(0, 0), FrameworkPropertyMetadataOptions.AffectsMeasure));

        public Point StartPoint
        {
            get { return (Point)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }

        public Point EndPoint
        {
            get { return (Point)GetValue(EndPointProperty); }
            set { SetValue(EndPointProperty, value); }
        }

        public ConnectorView()
        {
            linegeo = new LineGeometry();
                        
            this.Stroke = Brushes.Black;
            this.StrokeThickness = 2;
        }

        protected override Geometry DefiningGeometry
        {
            get
            {
                linegeo.StartPoint = StartPoint;
                linegeo.EndPoint = EndPoint;
                return linegeo;
            }
        }
    }
}
