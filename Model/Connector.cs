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

       public Connector(System start, System end)
       {
           Start = start;
           End = end;
       }

        public Connector() { }

        public System Start { get; set; }

        public System End { get; set; }

        public override void Save() { }

        public override void Save(XElement element) { }
    }
}
