using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickArch.DataAccess;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

namespace QuickArch.ViewModel
{
    public class DocumentViewModel : WorkspaceViewModel
    {
        readonly Document _document;
        bool _isSelected;
        bool _isExpanded;
        RelayCommand _saveCommand;
        RelayCommand _openCommand;

        #region Constructor
        public DocumentViewModel(Document document)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            
            _document = document;
            
            base.DisplayName = _document.Title;

            _document.ComponentDiagramAdded += this.OnComponentDiagramAddedToDocument;

            this.ShowComponentDiagrams();
        }
        #endregion

        #region Properties
        public ObservableCollection<ComponentDiagramViewModel> ComponentDiagrams { get; private set; }

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
            }
        }

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

        #region Helper methods
        void ShowComponentDiagrams()
        {
            List<ComponentDiagramViewModel> all = _document.ComponentDiagrams.ToList();

            foreach (ComponentDiagramViewModel diagram in all)
                diagram.PropertyChanged += this.OnComponentDiagramViewModelPropertyChanged;

            this.ComponentDiagrams = new ObservableCollection<ComponentDiagramViewModel>(all);
            this.ComponentDiagrams.CollectionChanged += this.OnCollectionChanged;
        }
        #endregion

        #region Event Handling Methods
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (ComponentDiagramViewModel compVM in e.NewItems)
                    compVM.PropertyChanged += this.OnComponentDiagramViewModelPropertyChanged;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (ComponentDiagramViewModel compVM in e.OldItems)
                    compVM.PropertyChanged -= this.OnComponentDiagramViewModelPropertyChanged;
        }

        void OnComponentDiagramViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string IsSelected = "IsSelected";

            //Make sure that the property name we're referencing is valid (debug only)
            (sender as ComponentDiagramViewModel).VerifyPropertyName(IsSelected);
        }

        //doesn't really do anything, can be implemented for other things
        void OnComponentDiagramAddedToDocument(object sender, ComponentDiagramAddedEventArgs e)
        {
        }
        
        #endregion

        public void Save()
        {
            _document.Save();
        }
        public void Add(ComponentDiagramViewModel newDiagram)
        {
            this.ComponentDiagrams.Add(newDiagram);
            _document.AddComponentDiagram(newDiagram);
        }
    }
}
