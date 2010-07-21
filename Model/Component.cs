using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace QuickArch.Model
{
    public class Component
    {
        public Component() { }

        public Component(string title, string parent)
        {
            Title = title;
            Parent = parent;
        }

        public string Title { get; set; }

        public string Parent { get; set; }
    }
}
