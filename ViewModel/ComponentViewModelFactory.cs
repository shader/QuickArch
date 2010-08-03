using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuickArch.Model;

namespace QuickArch.ViewModel
{
    public static class ComponentViewModelFactory
    {
        public static ComponentViewModel GetViewModel(QuickArch.Model.Component component)
        {
            if (component is QuickArch.Model.System)
            {
                return new SystemViewModel(component as QuickArch.Model.System);
            }
            else if (component is Sequence)
            {
                return new SequenceViewModel(component as Sequence);
            }
            else if (component is Document)
            {
                return new DocumentViewModel(component as Document);
            }
            else if (component is SystemDiagram)
            {
                return new SystemDiagramViewModel(component as QuickArch.Model.SystemDiagram);
            }
            else
                return null;
        }
    }
}
