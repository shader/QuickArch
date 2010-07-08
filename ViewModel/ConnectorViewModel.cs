using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickArch.Model;
using QuickArch.DataAccess;

namespace QuickArch.ViewModel
{
    public class ConnectorViewModel : WorkspaceViewModel
    {
        readonly Connector connector;
        readonly ComponentManager componentManager;
        bool isSelected;

        #region Constructor
        public ConnectorViewModel(Connector connector, ComponentManager componentManager)
        {
            if (connector == null)
                throw new ArgumentNullException("connector");
            if (componentManager == null)
                throw new ArgumentNullException("componentManager");

            this.connector = connector;
            this.componentManager = componentManager;
        }
        #endregion

        #region Connector Properties

        public Component Start
        {
            get { return connector.Start; }
            set
            {
                if (value == connector.Start)
                    return;

                connector.Start = value;

                base.OnPropertyChanged("Start");
            }
        }

        public Component End
        {
            get { return connector.End; }
            set
            {
                if (value == connector.End)
                    return;

                connector.End = value;

                base.OnPropertyChanged("End");
            }
        }

        #endregion

        #region Presentation Properties

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value == isSelected)
                    return;

                isSelected = value;

                base.OnPropertyChanged("IsSelected");
            }
        }

        #endregion

        #region Private Helpers

        #endregion
    }
}
