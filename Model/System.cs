using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

using QuickArch.ViewModel;
using QuickArch.Utilities;
using QuickArch.Properties;

namespace QuickArch.Model
{
    public class System : Component
    {
        private List<Component> _components;

        public System(String title)
        {
            _components = new List<Component>();
            Title = title;
        }
        
        //raised when a Component is added to the construct
        public event EventHandler<ComponentAddedEventArgs> ComponentAdded;
        
        public List<Component> Components
        {
            get { return _components; }
            set { _components = value; }
        }

        public void AddComponent(Component comp)
        {
            if (comp == null)
                throw new ArgumentNullException("comp");
            if(!_components.Contains(comp))
            {
                _components.Add(comp);

                if(this.ComponentAdded != null)
                    this.ComponentAdded(this, new ComponentAddedEventArgs(comp));
            }
        }

        public override void Save()
        {
        }

        public override void Save(XElement element)
        {
        }
        
        public void Open(string filename)
        {
            FileStream stream = File.OpenRead(Filename = filename);
            XElement.Load(stream);
        }
    }
}
