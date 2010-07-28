using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;
using QuickArch.Properties;

namespace QuickArch.Model
{
    public abstract class Component
    {
        public string Filename;
        protected Component _parent;

        public Component() : this(null) { }

        public Component(string title) : this(title, null) { }

        public Component(string title, Component parent)
        {
            Title = title;
            _parent = parent;
        }

        public string Title { get; set; }

        public abstract void Save(); // save to the predefined file
        public abstract void Save(XElement parent); // save information under parent's tree
        public void SaveAs(string filename) // set filename, and then save
        {
            Filename = filename;
            Save();
        }
        public void Delete()
        {
            (_parent as System).RemoveComponent(this);
        }
    }
}
