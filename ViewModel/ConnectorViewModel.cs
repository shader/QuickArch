using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickArch.Model;
using QuickArch.Utilities;
using System.Windows.Input;

namespace QuickArch.ViewModel
{
    public class ConnectorViewModel : ComponentViewModel
    {
        #region Constructor
        public ConnectorViewModel(Connector connector) : base(connector)
        {
            if (connector == null)
                throw new ArgumentNullException("connector");
        }
        #endregion

        #region Connector Properties

        public Component Start
        {
            get { return ((Connector)_component).Start; }
            set
            {
                if (value == ((Connector)_component).Start)
                    return;

                ((Connector)_component).Start = value;

                base.OnPropertyChanged("Start");
            }
        }

        public Component End
        {
            get { return ((Connector)_component).End; }
            set
            {
                if (value == ((Connector)_component).End)
                    return;

                ((Connector)_component).End = value;

                base.OnPropertyChanged("End");
            }
        }
        #endregion
    }
}
