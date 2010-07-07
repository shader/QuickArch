using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickArch.Model;

namespace QuickArch.DataAccess
{
    public class ComponentAddedEventArgs : EventArgs
    {
        public ComponentAddedEventArgs(Component newComponent)
        {
            this.NewComponent = newComponent;
        }

        public Component NewComponent { get; private set; }
    }
}
