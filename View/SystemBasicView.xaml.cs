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
using System.Windows.Controls.Primitives;

namespace QuickArch.View
{
    /// <summary>
    /// Interaction logic for ComponentView.xaml
    /// </summary>
    public partial class SystemBasicView : UserControl
    {
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(SystemBasicView));

        private Guid id;

        public List<ConnectorView> EndLines { get; private set; }
        public List<ConnectorView> StartLines { get; private set; }

        public bool IsSelected
        {
            get { return (bool)base.GetValue(IsSelectedProperty); }
            set { base.SetValue(IsSelectedProperty, value); }
        }

        public Point Position
        {
            get
            { return new Point(Canvas.GetLeft(this), Canvas.GetTop(this)); }
            set
            {
                Canvas.SetLeft(this, value.X);
                Canvas.SetTop(this, value.Y);
            }
        }

        public double Left
        {
            get { return this.Position.X; }
            set { Canvas.SetLeft(this, value); }
        }

        public double Top
        {
            get { return this.Position.Y; }
            set { Canvas.SetTop(this, value); }
        }

        public SystemBasicView()
        {
            InitializeComponent();
            id = Guid.NewGuid();

            StartLines = new List<ConnectorView>();
            EndLines = new List<ConnectorView>();
        }

        public Guid ID
        { 
            get { return id; } 
        }
    }
}
