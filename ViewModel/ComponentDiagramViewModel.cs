using System;
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
        readonly ComponentManager componentManager;

        //Constructor
        public ComponentDiagramViewModel(ComponentManager componentManager)
        {
            if (componentManager == null)
                throw new ArgumentNullException("componentManager");

            base.DisplayName = Resources.ComponentDiagramViewModel_DisplayName;

            this.componentManager = componentManager;

            //Subscribe for notifications of when a new component is added
            componentManager.ComponentAdded += this.OnComponentAddedToManager;

            //Populate the diagram with ComponentViewModels
            this.ShowComponents();
        }

        /// <summary>
        /// Create a list of components from the manager and
        /// convert to an ObservableCollection for the UI to handle
        /// </summary>
        void ShowComponents()
        {
            List<ComponentViewModel> all = (from comp in componentManager.getComponents() 
                                            select new ComponentViewModel(comp, componentManager)).ToList();
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

        #endregion

        #region Base Class Overrides

        protected override void OnDispose()
        {
            foreach (ComponentViewModel compVM in this.AllComponents)
                compVM.Dispose();

            this.AllComponents.Clear();
            this.AllComponents.CollectionChanged -= this.OnCollectionChanged;

            componentManager.ComponentAdded -= this.OnComponentAddedToManager;
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
        #endregion

        void OnComponentViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string IsSelected = "IsSelected";

            //Make sure that the property name we're referencing is valid (debug only)
            (sender as ComponentViewModel).VerifyPropertyName(IsSelected);
        }

        void OnComponentAddedToManager(object sender, ComponentAddedEventArgs e)
        {
            var viewModel = new ComponentViewModel(e.NewComponent, componentManager);
            this.AllComponents.Add(viewModel);
        }
    }
}
