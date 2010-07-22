using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickArch.Model;
using System.Windows.Input;
using QuickArch.DataAccess;

namespace QuickArch.ViewModel
{
    public class ComponentViewModel : WorkspaceViewModel
    {
        readonly Component _component;
        readonly ComponentManager _componentManager;
        bool _isSelected;
        RelayCommand _saveCommand;

        public ComponentViewModel(Component component, ComponentManager componentManager)
        {
            if (component == null)
                throw new ArgumentNullException("component");
            if (componentManager == null)
                throw new ArgumentNullException("componentManager");

            _component = component;
            _componentManager = componentManager;
        }

        public ComponentViewModel(Component component, ComponentManager componentManager, String title)
            : this(component, componentManager)
        {
            _component.Title = title;
        }

        #region Component Properties
        public string Title
        {
            get { return _component.Title; }
            set
            {
                if (value == _component.Title)
                    return;

                _component.Title = value;

                base.OnPropertyChanged("Title");
            }
        }

        public string Parent
        {
            get { return _component.Parent; }
            set
            {
                if (value == _component.Parent)
                    return;

                _component.Parent = value;

                base.OnPropertyChanged("Parent");
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
        //Saves the component to the manager
        public void Save()
        {
            //if (!component.IsValid)
                //throw new InvalidOperationException(Strings.ComponentViewModel_Exception_CannotSave);

            if (this.IsNewComponent)
                _componentManager.AddComponent(_component);
        }
        #endregion

        #region Private Helpers
        //returns true if this component was created by the user and has not yet been saved to the manager
        bool IsNewComponent
        {
            get { return !_componentManager.ContainsComponent(_component); }
        }

        //returns true if the component is valid and can be saved
        bool CanSave
        {
            get {return true; }
        }
        #endregion
    }
}
