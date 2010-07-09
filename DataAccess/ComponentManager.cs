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
        private List<Component> components;
        private List<Connector> links;
        private XDocument xDoc;
        

        //Constructor
        public ComponentManager()
        {
            components = new List<Component>();
            links = new List<Connector>();
        }

        //Raised when a component is placed into the manager
        public event EventHandler<ComponentAddedEventArgs> ComponentAdded;

        public void addComponent(Component component)
        {
            if (component == null)
                throw new ArgumentNullException("component");
            if (!components.Contains(component))
            {
                components.Add(component);

                if (this.ComponentAdded != null)
                    this.ComponentAdded(this, new ComponentAddedEventArgs(component));
            }
        }

        public void removeComponent(Component component)
        {
            components.Remove(component);
        }

        public void addLink(Connector link)
        {
            links.Add(link);
        }

        public void removeLink(Connector link)
        {
            links.Remove(link);
        }

        public bool containsComponent(Component component)
        {
            if (component == null)
                throw new ArgumentNullException("component");

            return components.Contains(component);
        }
        //Shallow copied list
        public List<Component> getComponents()
        {
            return new List<Component>(components);
        }

        public void Save()
        {
            xDoc = new XDocument(new XElement("Components",
                                 from comp in components
                                 where comp.Title != null
                                 select new XElement("Component", 
                                            new XAttribute("Title", comp.Title))));
            if (FileManager.File != null)
                xDoc.Save(FileManager.File);
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
                xDoc = XDocument.Load(file);
                var components = xDoc.Element("Components");
                var comps = components.Descendants("Component");
                var elements = from el in xDoc.Element("Components").Descendants("Component") select el;
                foreach (var el in elements)
                {
                    addComponent(new Component(el.Attribute("Title") != null ? el.Attribute("Title").Value : null,
                                               el.Attribute("Parent") != null ? el.Attribute("Parent").Value : null));
                }
            }
        }
    }
}
