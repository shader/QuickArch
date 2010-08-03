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
using System.Collections.ObjectModel;
using QuickArch.ViewModel;

namespace QuickArch.View
{
    /// <summary>
    /// Interaction logic for ComponentDiagramView.xaml
    /// </summary>
    public partial class SystemDiagramView : UserControl
    {
        AdornerLayer aLayer;

        bool _isDown, _isDragging;
        bool selected = false;
        bool isAddNewLink = false;

        UIElement selectedElement = null;

        Point _startPoint, _originalPoint;
        private Canvas myCanvas;
        private ObservableCollection<SystemBasicView> _components;
        private double xInc;

        public SystemDiagramView()
        {
            InitializeComponent();
            _components = new ObservableCollection<SystemBasicView>();
        }

        private void myCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            myCanvas = sender as Canvas;
            myCanvas.MouseLeftButtonDown += new MouseButtonEventHandler(myCanvas_MouseLeftButtonDown);
            myCanvas.MouseLeftButtonUp += new MouseButtonEventHandler(DragFinishedMouseHandler);
        }

        private void ArrangeChildren()
        {
            if (myCanvas != null)
            {
                if (myCanvas.Children.Count != 0)
                {
                    foreach (SystemBasicView child in myCanvas.Children)
                    {
                        Canvas.SetLeft(child, xInc);
                    }
                }
            }
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
                if (selectedElement != null && aLayer.GetAdorners(selectedElement) != null)
                {
                    // Remove the adorner from the selected element
                    aLayer.Remove(aLayer.GetAdorners(selectedElement)[0]);
                    selectedElement = null;
                }
            }

            // If a componentView is selected, add adorner layer to it
            if (e.Source.GetType() == typeof(SystemBasicView))
            {
                _isDown = true;
                _startPoint = e.GetPosition(myCanvas);

                selectedElement = e.Source as UIElement;
                _originalPoint = selectedElement.TranslatePoint(new Point(0,0), myCanvas);

                aLayer = AdornerLayer.GetAdornerLayer(selectedElement);
                aLayer.Add(new ResizingAdorner(selectedElement, myCanvas));

                selected = true;
                e.Handled = true;
            }
        }
        void myCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        public void NewLinkButton_Click(object sender, RoutedEventArgs e)
        {
            isAddNewLink = true;
            Mouse.OverrideCursor = Cursors.Cross;
        }

        private void Components_Updated(object sender, DataTransferEventArgs e)
        {
            SystemBasicView s = sender as SystemBasicView;
            if (s != null)
            {
                xInc += s.Width;
                _components.Add(s);
            }
        }

        private void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            ContextMenu menu = sender as ContextMenu;
            menu.DataContext = DataContext;
        }
    }

    #region DataTemplateSelectorClass
    public class ItemsControlTemplateSelector : DataTemplateSelector
    {
        private DataTemplate _componentTemplate = null;
        public DataTemplate ComponentTemplate
        {
            get { return _componentTemplate; }
            set { _componentTemplate = value; }
        }

        private DataTemplate _tempConnectorTemplate = null;
        public DataTemplate TempConnectorTemplate
        {
            get { return _tempConnectorTemplate; }
            set { _tempConnectorTemplate = value; }
        }

        public override DataTemplate SelectTemplate(object item,
            DependencyObject container)
        {
            if (item is ComponentViewModel)
            {
                if (item is SystemViewModel)
                    return _componentTemplate;
                if (item is TemporaryConnectorViewModel)
                    return _tempConnectorTemplate;
                if (item is ConnectorViewModel)
                    return _tempConnectorTemplate;
            }
            return base.SelectTemplate(item, container);
        }
    }
    #endregion
}