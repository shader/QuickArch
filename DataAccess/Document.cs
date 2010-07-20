using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickArch.ViewModel;
using System.Xml.Linq;

namespace QuickArch.DataAccess
{
    public class Document
    {
        private List<ComponentDiagramViewModel> _componentDiagrams;
        //private List<SeqenceChartViewModel> _sequenceCharts;
        private XDocument _xDoc;

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
            if (_componentDiagrams.Count == 0)
                throw new Exception("componentDiagrams");

            _xDoc = new XDocument(new XElement("document", new XAttribute("version", "1.0"),
                                    new XElement("component_diagram",new XAttribute("name",_componentDiagrams.ElementAt(0).DisplayName))
                                    )
            );

            if (FileManager.File != null)
                _xDoc.Save(FileManager.File);
        }

        public void SaveAs()
        {
        }

        public void Open()
        {
        }
    }
}
