using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

using QuickArch.Model;

namespace QuickArch.ViewModel
{
    public class DocumentViewModel : SystemViewModel
    {
        public DocumentViewModel(Document document)
            : base(document)
        {
            (Component as Document).ComponentsChanged += OnDocumentComponentsChanged;
        }

        public void AddSystemDiagram(string name)
        {
            (Component as Document).AddComponent(new SystemDiagram(name, Component as Document));            
        }

        public void OnDocumentComponentsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
        }
    }
}
