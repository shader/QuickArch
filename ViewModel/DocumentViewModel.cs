using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickArch.DataAccess;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;

using QuickArch.Model;

namespace QuickArch.ViewModel
{
    public class DocumentViewModel : WorkspaceViewModel
    {
        readonly Document _document;
        private IChart _selectedChart;
        bool _isSelected;
        bool _isExpanded;

        #region Constructor
        public DocumentViewModel(Document document)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            
            _document = document;
            
            base.DisplayName = _document.Title;

            _document.ChartAdded += this.OnChartAddedToDocument;

            this.ShowCharts();
        }
        #endregion

        #region Presentation Properties
        public ObservableCollection<IChart> Charts { get; private set; }

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

        public IChart SelectedChart
        {
            get { return _selectedChart; }
            set
            {
                if (value != _selectedChart)
                    _selectedChart = value;

                this.OnPropertyChanged("SelectedChart");
            }
        }
        #endregion

        #region Helper methods
        void ShowCharts()
        {
            List<IChart> all = _document.Charts.ToList();

            foreach (IChart chart in all)
                chart.PropertyChanged += this.OnChartPropertyChanged;

            this.Charts = new ObservableCollection<IChart>(all);
            this.Charts.CollectionChanged += this.OnCollectionChanged;
        }
        #endregion

        #region Event Handling Methods
        void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (ComponentDiagramViewModel compVM in e.NewItems)
                    compVM.PropertyChanged += this.OnChartPropertyChanged;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (ComponentDiagramViewModel compVM in e.OldItems)
                    compVM.PropertyChanged -= this.OnChartPropertyChanged;
        }

        void OnChartPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string IsSelected = "IsSelected";

            //Make sure that the property name we're referencing is valid (debug only)
            (sender as ComponentDiagramViewModel).VerifyPropertyName(IsSelected);
        }

        //doesn't really do anything, can be implemented for other things
        void OnChartAddedToDocument(object sender, ChartAddedEventArgs e)
        {
        }
        
        #endregion

        public void Save()
        {
            _document.Save();
        }
        public void Add(IChart chart)
        {
            this.Charts.Add(chart);
            _document.AddChart(chart);
        }

    }
}
