using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickArch.Model;

namespace QuickArch.DataAccess
{
    class ConnectorAddedEventArgs : EventArgs
    {
            public ConnectorAddedEventArgs(Connector newConnector)
            {
                this.NewConnector = newConnector;
            }

            public Connector NewConnector { get; private set; }
    }
}
