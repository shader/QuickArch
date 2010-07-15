using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using QuickArch;
using QuickArch.Model;
using System.Xml.Linq;

namespace QuickArch.DataAccess
{
    public class ComponentManager
    {
        private List<Component> _components;
        private List<Connector> _links;
        private XDocument _xDoc;
        

        //Constructor
        public ComponentManager()
        {
            _components = new List<Component>();
            _links = new List<Connector>();
        }

        #region Events
        //Raised when a component is placed into the manager
        public event EventHandler<ComponentAddedEventArgs> ComponentAdded;
        //Raised when a connector is added to the manager
        public event EventHandler<ConnectorAddedEventArgs> ConnectorAdded;
        #endregion

        public void AddComponent(Component component)
        {
            if (component == null)
                throw new ArgumentNullException("component");
            if (!_components.Contains(component))
            {
                _components.Add(component);

                if (this.ComponentAdded != null)
                    this.ComponentAdded(this, new ComponentAddedEventArgs(component));
            }
        }

        public void RemoveComponent(Component component)
        {
            _components.Remove(component);
        }

        public void AddLink(Connector link)
        {
            _links.Add(link);
        }

        public void RemoveLink(Connector link)
        {
            _links.Remove(link);
        }

        public bool ContainsComponent(Component component)
        {
            if (component == null)
                throw new ArgumentNullException("component");

            return _components.Contains(component);
        }
        public bool ContainsConnector(Connector connector)
        {
            if (connector == null)
                throw new ArgumentNullException("connector");

            return _links.Contains(connector);
        }
        //Shallow copied list
        public List<Component> GetComponents()
        {
            return new List<Component>(_components);
        }

        public void Save()
        {
            _xDoc = new XDocument(new XElement("Components",
                                 from comp in _components
                                 where comp.Title != null
                                 select new XElement("Component", 
                                            new XAttribute("Title", comp.Title))));
            if (FileManager.File != null)
                _xDoc.Save(FileManager.File);
        }

        public void SaveAs()
        {
            string file = FileManager.SaveFile();
            if (file != null)
            {
                FileManager.File = file;
                Save();
            }
        }

        public void Open()
        {
            string file = FileManager.OpenFile();
            if (file != null)
            {
                FileManager.File = file;
                _xDoc = XDocument.Load(file);
                var components = _xDoc.Element("Components");
                var comps = components.Descendants("Component");
                var elements = from el in _xDoc.Element("Components").Descendants("Component") select el;
                foreach (var el in elements)
                {
                    AddComponent(new Component(el.Attribute("Title") != null ? el.Attribute("Title").Value : null,
                                               el.Attribute("Parent") != null ? el.Attribute("Parent").Value : null));
                }
            }
        }
    }
}
