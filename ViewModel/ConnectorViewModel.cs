using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickArch.Model;
using QuickArch.DataAccess;
using System.Windows.Input;

namespace QuickArch.ViewModel
{
    public class ConnectorViewModel : WorkspaceViewModel
    {
        readonly Connector _connector;
        readonly ComponentManager _componentManager;
        bool _isSelected;
        RelayCommand _saveCommand;

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

        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                        param => this.Save(),
                        param => this.CanSave
                        );
                }
                return _saveCommand;
            }
        }
        #endregion

        #region Public Methods
        public void Save()
        {
            if (this.IsNewConnector)
                _componentManager.AddLink(_connector);
        }
        #endregion

        #region Private Helpers
        bool IsNewConnector
        {
            get { return !_componentManager.ContainsConnector(_connector); }
        }

        //returns true if the component is valid and can be saved
        bool CanSave
        {
            get {return true; }
        }
        #endregion
    }
}
