using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuickArch.View
{
    /// <summary>
    /// Interaction logic for ComponentDiagramView.xaml
    /// </summary>
    public partial class ComponentDiagramView : UserControl
    {
        AdornerLayer aLayer;

        bool _isDown;
        bool _isDragging;
        bool selected = false;
        UIElement selectedElement = null;

        Point _startPoint;
        private double _originalLeft;
        private double _originalTop;

        public ComponentDiagramView()
        {
            InitializeComponent();
        }
        /*
        private void Diagram_Created(object sender, RoutedEventArgs e)
        {
            this.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(Diagram_MouseLeftButtonDown);
            this.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(DragFinishedMouseHandler);
            this.PreviewMouseMove += new MouseEventHandler(Diagram_MouseMove);
        }
         */
    }
}