﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickArch.DataAccess;
using QuickArch.Properties;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;

namespace QuickArch.ViewModel
{
    /// <summary>
    /// Represents a container of ComponentViewModel and ConnectorViewModel 
    /// objects that has support for staying synchronized with the 
    /// ComponentManager.
    /// </summary>
    public class ComponentDiagramViewModel : WorkspaceViewModel
    {
        readonly ComponentManager _componentManager;
        bool _isSelected;

        #region Constructor
        public ComponentDiagramViewModel(ComponentManager componentManager, string displayName)
        {
            if (componentManager == null)
                throw new ArgumentNullException("componentManager");

            base.DisplayName = displayName;

            _componentManager = componentManager;

            //Subscribe for notifications of when a new component is added
            _componentManager.ComponentAdded += this.OnComponentAddedToManager;
            //_componentManager.ConnectorAdded

            //Populate the diagram with ComponentViewModels
            this.ShowComponents();
        }
        #endregion

        public ComponentManager GetComponentManager()
        {
            return _componentManager;
        }

        /// <summary>
        /// Create a list of components from the manager and
        /// convert to an ObservableCollection for the UI to handle
        /// </summary>
        void ShowComponents()
        {
            List<ComponentViewModel> all = (from comp in _componentManager.GetComponents() 
                                            select new ComponentViewModel(comp, _componentManager)).ToList();
            foreach (ComponentViewModel cvm in all)
                cvm.PropertyChanged += this.OnComponentViewModelPropertyChanged;

            this.AllComponents = new ObservableCollection<ComponentViewModel>(all);
            this.AllComponents.CollectionChanged += this.OnCollectionChanged;
        }

        #region Public Interface

        /// <summary>
        /// Returns an ObservableCollection of ComponentViewModel objects
        /// </summary>
        public ObservableCollection<ComponentViewModel> AllComponents { get; private set; }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        #endregion

        #region Base Class Overrides

        protected override void OnDispose()
        {
            foreach (ComponentViewModel compVM in this.AllComponents)
                compVM.Dispose();

            this.AllComponents.Clear();
            this.AllComponents.CollectionChanged -= this.OnCollectionChanged;

            _componentManager.ComponentAdded -= this.OnComponentAddedToManager;
        }
        #endregion

        #region Event Handling Methods

        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (ComponentViewModel compVM in e.NewItems)
                    compVM.PropertyChanged += this.OnComponentViewModelPropertyChanged;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (ComponentViewModel compVM in e.OldItems)
                    compVM.PropertyChanged -= this.OnComponentViewModelPropertyChanged;
        }

        void OnComponentViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string IsSelected = "IsSelected";

            //Make sure that the property name we're referencing is valid (debug only)
            (sender as ComponentViewModel).VerifyPropertyName(IsSelected);
        }

        void OnComponentAddedToManager(object sender, ComponentAddedEventArgs e)
        {
            var viewModel = new ComponentViewModel(e.NewComponent, _componentManager);
            this.AllComponents.Add(viewModel);
        }
        #endregion
    }
}
