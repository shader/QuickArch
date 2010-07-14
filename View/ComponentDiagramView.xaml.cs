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

        Point _startPoint, _originalPoint;
        private Canvas myCanvas;

        public ComponentDiagramView()
        {
            InitializeComponent();
        }

        private void myCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            myCanvas = sender as Canvas;
            myCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(myCanvas_MouseLeftButtonDown);
            myCanvas.MouseLeftButtonUp += new MouseButtonEventHandler(DragFinishedMouseHandler);
        }   

        private void Diagram_Created(object sender, RoutedEventArgs e)
        {
            this.MouseLeftButtonDown += new MouseButtonEventHandler(Diagram_MouseLeftButtonDown);
            this.MouseLeftButtonUp += new MouseButtonEventHandler(DragFinishedMouseHandler);
            this.MouseMove += new MouseEventHandler(Diagram_MouseMove);
            this.MouseLeave += new MouseEventHandler(Diagram_MouseLeave);
        }

        // Handler for drag stopping on leaving the window
        void Diagram_MouseLeave(object sender, MouseEventArgs e)
        {
            StopDragging();
            e.Handled = true;
        }

        // Handler for drag stopping on user choise
        void DragFinishedMouseHandler(object sender, MouseButtonEventArgs e)
        {
            StopDragging();
            e.Handled = true;
        }

        // Method for stopping dragging
        private void StopDragging()
        {
            if (_isDown)
            {
                _isDown = false;
                _isDragging = false;
            }
        }

        // Hanler for providing drag operation with selected element
        void Diagram_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDown)
            {
                if ((_isDragging == false) &&
                    ((Math.Abs(e.GetPosition(myCanvas).X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance) ||
                    (Math.Abs(e.GetPosition(myCanvas).Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)))
                    _isDragging = true;

                if (_isDragging)
                {
                    Point position = Mouse.GetPosition(myCanvas);
                    var newX = position.X - (_startPoint.X - _originalPoint.X);
                    var newY = position.Y - (_startPoint.Y - _originalPoint.Y);
                    selectedElement.RenderTransform = new TranslateTransform(newX, newY);
                }
            }
        }

        // Handler for clearing element selection, adorner removal
        void Diagram_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selected)
            {
                selected = false;
                if (selectedElement != null)
                {
                    aLayer.Remove(aLayer.GetAdorners(selectedElement)[0]);
                    selectedElement = null;
                }
            }
        }

        // Handler for element selection on the canvas providing resizing adorner
        void myCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Remove selection on clicking anywhere the window
            if (selected)
            {
                selected = false;
                if (selectedElement != null)
                {
                    // Remove the adorner from the selected element
                    aLayer.Remove(aLayer.GetAdorners(selectedElement)[0]);
                    selectedElement = null;
                }
            }

            // If any element except canvas is clicked, 
            // assign the selected element and add the adorner
            if (e.Source != myCanvas)
            {
                _isDown = true;
                _startPoint = e.GetPosition(myCanvas);

                selectedElement = e.Source as UIElement;
                _originalPoint = selectedElement.TranslatePoint(new Point(0,0), myCanvas);

                aLayer = AdornerLayer.GetAdornerLayer(selectedElement);
                aLayer.Add(new ResizingAdorner(selectedElement));
                selected = true;
                e.Handled = true;
            }
        }
    }
}