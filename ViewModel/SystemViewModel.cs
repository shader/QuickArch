﻿using System;
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
        private List<ConnectorViewModel> _connections;
        ICommand _deleteCommand;
        bool _isExpanded;

        #region Properties
        public ObservableCollection<ComponentViewModel> ComponentVMs { get; private set; }

        public event ComponentSelectionHandler ComponentSelected;
    
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
        public QuickArch.Model.System System
        {
            get { return _component as QuickArch.Model.System; }
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

        public void AddConnector(ConnectorViewModel newConnectorVM)
        {
           // ((QuickArch.Model.System)_component).AddConnector();
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

        //added to ComponentSelected Event when Link button is clicked
        public void StartTempConnector(ComponentViewModel start, EventArgs e)
        {
            //Create temporary connector for visual display
            TemporaryConnectorViewModel temp = new TemporaryConnectorViewModel(start as SystemViewModel);
            ComponentVMs.Add(temp);
            //remove handler from event to prevent being called again
            ComponentSelected -= StartTempConnector;
            //create event handler closure to remember start
            ComponentSelectionHandler h = null;
            h = (end, args) =>
            {
                //create real connector with remembered start and new end
                CreateConnector(start as SystemViewModel, end as SystemViewModel);
                //remove h so not called again
                ComponentSelected -= h;
                //remove temporary connector
                ComponentVMs.Remove(temp);
            };
            //add anonymous event handler "h" to ComponentSelected
            ComponentSelected += h;
        }
        
        void CreateConnector(SystemViewModel start, SystemViewModel end)
        {
            System.AddConnector(start.System, end.System);
        }

        #endregion
    }
}
