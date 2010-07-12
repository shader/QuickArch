using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickArch.ViewModel;

namespace QuickArch.DataAccess
{
    public class Document
    {
        private List<ComponentDiagramViewModel> _componentDiagrams;
        //private List<SeqenceChartViewModel> _sequenceCharts;

        public Document(String title)
        {
            _componentDiagrams = new List<ComponentDiagramViewModel>();
            Title = title;
        }
        
        //raised when a Component Diagram (ViewModel) is added to the document
        public event EventHandler<ComponentDiagramAddedEventArgs> ComponentDiagramAdded;

        public String Title { get; set; }

        public List<ComponentDiagramViewModel> ComponentDiagrams
        {
            get { return _componentDiagrams; }
            set { _componentDiagrams = value; }
        }

        public void AddComponentDiagram(ComponentDiagramViewModel componentDiagram)
        {
            if (componentDiagram == null)
                throw new ArgumentNullException("componentDiagram");
            if(!_componentDiagrams.Contains(componentDiagram))
            {
                _componentDiagrams.Add(componentDiagram);

                if(this.ComponentDiagramAdded != null)
                    this.ComponentDiagramAdded(this, new ComponentDiagramAddedEventArgs(componentDiagram));
            }
        }

        public void RemoveComponentDiagram(ComponentDiagramViewModel componentDiagram)
        {
            _componentDiagrams.Remove(componentDiagram);
        }

        public bool ContainsComponentDiagram(ComponentDiagramViewModel componentDiagram)
        {
            if (componentDiagram == null)
                throw new ArgumentNullException("componentDiagram");

            return _componentDiagrams.Contains(componentDiagram);
        }

        public void Save()
        {
        }

        public void SaveAs()
        {
        }

        public void Open()
        {
        }
    }
}
