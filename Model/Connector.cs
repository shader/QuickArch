using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace QuickArch.Model
{
    public class Connector
    {
       public static Connector CreateNewComponent()
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

        protected Connector()
        {
        }

        public Component Start { get; set; }

        public Component End { get; set; }
    }
}
