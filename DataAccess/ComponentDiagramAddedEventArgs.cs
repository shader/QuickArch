using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickArch.ViewModel;

namespace QuickArch.DataAccess
{
    public class ComponentDiagramAddedEventArgs : EventArgs
    {
        public ComponentDiagramAddedEventArgs(ComponentDiagramViewModel componentDiagram)
        {
            this.NewComponentDiagram = componentDiagram;
        }

        public ComponentDiagramViewModel NewComponentDiagram { get; private set; }
    }
}
