using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;


namespace QuickArch
{
    class MyMenu : Menu
    {

        public MyMenu() : base()
        {
        }
        public MyMenu(Point position) : this()
        {
            this.setPosition(position);
        }
        public void setPosition(Point position)
        {
            Canvas.SetLeft(this, position.X);
            Canvas.SetTop(this, position.Y);
        }
    }
}
