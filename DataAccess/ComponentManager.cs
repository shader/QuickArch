using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using QuickArch;
using QuickArch.Model;

namespace QuickArch.DataAccess
{
    public class ComponentManager
    {
        private List<Component> components;
        private List<Connector> links;

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

        public List<Component> getComponents()
        {
            return new List<Component>(components);
        }
    }
}
