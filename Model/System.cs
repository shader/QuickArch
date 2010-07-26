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
        
        public System(String filename) : this(XElement.Load(filename))
        {
            Filename = filename;
        }

        public System(XElement source) : this(source, null) { }
            

        public System(XElement source, System parent)
            : this(source.Attribute("Title") != null ? source.Attribute("Title").Value : Resources.DefaultFilename,
                   null)
        {
            foreach (var child in source.Elements())
            {
                if (child.Name.LocalName == Resources.SubsystemTagName)
                {
                    AddComponent(new System(child, this));
                }
                else if (child.Name.LocalName == Resources.SequenceTagName)
                {
                    AddComponent(new Sequence(child, this));
                }
            }
        }

        public System(String title, System parent) : base(title, parent)
        {
            _components = new List<Component>();
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

        public void AddSubsystem(string title)
        {
            AddComponent(new System(title, this));
        }

        public override void Save()
        {
            if ( Filename != null)
            {
                XElement doc = new XElement(Resources.SystemTagName,
                                            new XAttribute("Title", Title));
                foreach ( Component cmp in _components ) 
                {
                    cmp.Save(doc); //add each component recursively to the save tree.
                }
                doc.Save(Filename); //write to file
            }
        }

        public override void Save(XElement parent)
        {
            if (Filename != null)
            {
                parent.Add(new XElement(Resources.SubsystemTagName,
                                        new XAttribute("Title", Title),
                                        new XAttribute("Filename", Filename)));
                Save();
            }
            else
            {
                XElement elm = new XElement(Resources.SubsystemTagName,
                                            new XAttribute("Title", Title));
                parent.Add(elm);
                foreach (Component cmp in _components)
                {
                    cmp.Save(elm); //add each component recursively to the save tree.
                }
            }
        }
        
        public void Open(string filename)
        {
            FileStream stream = File.OpenRead(Filename = filename);
            XElement.Load(stream);
        }
    }
}
