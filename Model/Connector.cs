using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace QuickArch.Model
{
    public class Connector : Component
    {
       public static Connector CreateNewConnector()
        {
            return new Connector();
        }

        public static Connector CreateConnector(Component start, Component end)
        {
            return new Connector
            {
                Start = start,
                End = end
            };
        }

        public Connector() { }

        public Component Start { get; set; }

        public Component End { get; set; }

        public override void Save() { }

        public override void Save(XElement element) { }
    }
}
