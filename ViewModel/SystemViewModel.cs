using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickArch.Utilities;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Input;
using System.ComponentModel;

using QuickArch.Model;

namespace QuickArch.ViewModel
{
    public class SystemViewModel : ComponentViewModel
    {
        private ComponentViewModel _selectedComponent;
        ICommand _deleteCommand;
        bool _isExpanded;

        #region Properties
        public ObservableCollection<ComponentViewModel> ComponentVMs { get; private set; }
    
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    this.OnPropertyChanged("IsExpanded");
                }
                if (value == true)
                {
                    LoadComponentViews();
                }
            }
        }

        public ComponentViewModel SelectedComponent
        {
            get { return _selectedComponent; }
            set
            {
                if (value != _selectedComponent)
                    _selectedComponent = value;

                this.OnPropertyChanged("SelectedComponent");
            }
        }
        #endregion

        public SystemViewModel(QuickArch.Model.System system) : base(system)
        {
            if (system == null)
                throw new ArgumentNullException("system");

            ComponentVMs = new ObservableCollection<ComponentViewModel>();
            if(system.Components.Count > 0)
                ComponentVMs.Add(new ComponentPlaceHolder());
                
                        
            ((QuickArch.Model.System)_component).ComponentAdded += OnComponentAddedToSystem;
        }
        
        #region Helper methods
        /// <summary>
        /// LoadComponentViews creates viewmodels dynamically for components stored in the system when it is expanded.
        /// </summary>
        void LoadComponentViews()
        {
            if (ComponentVMs.Count > 0 && ComponentVMs[0] is ComponentPlaceHolder) //place holder implies we need to load components
            {
                ComponentVMs = new ObservableCollection<ComponentViewModel>();
                foreach (var comp in ((QuickArch.Model.System)_component).Components)
                {
                    if (comp is QuickArch.Model.System)
                    {
                        ComponentVMs.Add(new SystemViewModel((QuickArch.Model.System)comp));
                    }
                    else if (comp is Sequence)
                    {
                        ComponentVMs.Add(new SequenceViewModel((Sequence)comp));
                    }
                }
                foreach (var vm in ComponentVMs) { vm.PropertyChanged += this.OnComponentPropertyChanged; }
                ComponentVMs.CollectionChanged += OnCollectionChanged;
            }
        }
        #endregion

        public void AddSubsystem(string title)
        {
            ((QuickArch.Model.System)_component).AddSubsystem(title);
        }

        public ICommand DeleteCommand
        {
            get
            {
                if (_deleteCommand == null)
                    _deleteCommand = new RelayCommand(param => this.OnDispose());

                return _deleteCommand;
            }
        }

        #region Event Handling Methods
        protected override void OnDispose()
        {
            foreach (ComponentViewModel compVM in this.ComponentVMs)
                compVM.Dispose();

            ComponentVMs.Clear();
            ComponentVMs.CollectionChanged -= OnCollectionChanged;
        }

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (ComponentViewModel compVM in e.NewItems)
                {
                    compVM.PropertyChanged += OnComponentPropertyChanged;
                }

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (ComponentViewModel compVM in e.OldItems)
                    compVM.PropertyChanged -= OnComponentPropertyChanged;
        }

        void OnComponentPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string IsSelected = "IsSelected";

            //Make sure that the property name we're referencing is valid (debug only)
            (sender as ComponentViewModel).VerifyPropertyName(IsSelected);
        }

        //doesn't really do anything, can be implemented for other things
        void OnComponentAddedToSystem(object sender, ComponentAddedEventArgs e)
        {
            if (sender is QuickArch.Model.System)
            {
                ComponentVMs.Add(new SystemViewModel((QuickArch.Model.System)e.NewComponent));
            }
            else if (sender is Sequence)
            {
                ComponentVMs.Add(new SequenceViewModel((Sequence)e.NewComponent));
            }
        }
        
        #endregion
    }
}
