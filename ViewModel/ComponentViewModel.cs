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
        readonly Component component;
        readonly ComponentManager componentManager;
        bool isSelected;
        RelayCommand saveCommand;

        public ComponentViewModel(Component component, ComponentManager componentManager)
        {
            if (component == null)
                throw new ArgumentNullException("component");
            if (componentManager == null)
                throw new ArgumentNullException("componentManager");

            this.component = component;
            this.componentManager = componentManager;
        }

        #region Component Properties
        public string Title
        {
            get { return component.Title; }
            set
            {
                if (value == component.Title)
                    return;

                component.Title = value;

                base.OnPropertyChanged("Title");
            }
        }

        public string Parent
        {
            get { return component.Parent; }
            set
            {
                if (value == component.Parent)
                    return;

                component.Parent = value;

                base.OnPropertyChanged("Parent");
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
        
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    saveCommand = new RelayCommand(
                        param => this.Save(),
                        param => this.CanSave
                        );
                }
                return saveCommand;
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
                componentManager.addComponent(component);
        }
        #endregion

        #region Private Helpers
        //returns true if this component was created by the user and has not yet been saved to the manager
        bool IsNewComponent
        {
            get { return !componentManager.containsComponent(component); }
        }

        //returns true if the component is valid and can be saved
        bool CanSave
        {
            get {return true; }
        }
        #endregion
    }
}
