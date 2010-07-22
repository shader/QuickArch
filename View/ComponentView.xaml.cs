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
    public partial class ComponentView : UserControl
    {
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(ComponentView));
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(Point), typeof(ComponentView));

        private Guid id;

        public bool IsSelected
        {
            get { return (bool)base.GetValue(IsSelectedProperty); }
            set { base.SetValue(IsSelectedProperty, value); }
        }

        public Point Position
        {
            get { return (Point)base.GetValue(PositionProperty); }
            set { base.SetValue(PositionProperty, value); }
        }

        public ComponentView()
        {
            this.RenderTransform = new TranslateTransform(0, 0);
            InitializeComponent();
            id = Guid.NewGuid();
        }

        public Guid ID
        { 
            get { return id; } 
        }
    }
}
