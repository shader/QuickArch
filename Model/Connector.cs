using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace QuickArch.Model
{
    public class Connector
    {
        private Component startComponent;
        private Component endComponent;

        public Connector(Component start, Component end)
        {
            startComponent = start;
            endComponent = end;
        }
    }
}
