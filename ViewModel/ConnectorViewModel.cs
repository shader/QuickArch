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
        readonly Connector _connector;
        readonly ComponentManager _componentManager;
        bool _isSelected;

        #region Constructor
        public ConnectorViewModel(Connector connector, ComponentManager componentManager)
        {
            if (connector == null)
                throw new ArgumentNullException("connector");
            if (componentManager == null)
                throw new ArgumentNullException("componentManager");

            _connector = connector;
            _componentManager = componentManager;
        }
        #endregion

        #region Connector Properties

        public Component Start
        {
            get { return _connector.Start; }
            set
            {
                if (value == _connector.Start)
                    return;

                _connector.Start = value;

                base.OnPropertyChanged("Start");
            }
        }

        public Component End
        {
            get { return _connector.End; }
            set
            {
                if (value == _connector.End)
                    return;

                _connector.End = value;

                base.OnPropertyChanged("End");
            }
        }

        #endregion

        #region Presentation Properties

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected)
                    return;

                _isSelected = value;

                base.OnPropertyChanged("IsSelected");
            }
        }

        #endregion

        #region Private Helpers

        #endregion
    }
}
