using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;

using QuickArch.ViewModel;
using QuickArch.DataAccess;

namespace QuickArch.Model
{
    public class Document
    {
        private List<IChart> _charts;
        private string _file;

        public Document(String title)
        {
            _charts = new List<IChart>();
            Title = title;
        }
        
        //raised when a Chart (ViewModel) is added to the document
        public event EventHandler<ChartAddedEventArgs> ChartAdded;

        public String Title { get; set; }

        public List<IChart> Charts
        {
            get { return _charts; }
            set { _charts = value; }
        }

        public void AddChart(IChart chart)
        {
            if (chart == null)
                throw new ArgumentNullException("chart");
            if(!_charts.Contains(chart))
            {
                _charts.Add(chart);

                if(this.ChartAdded != null)
                    this.ChartAdded(this, new ChartAddedEventArgs(chart));
            }
        }

        public void RemoveChart(IChart chart)
        {
            _charts.Remove(chart);
        }

        public bool ContainsChart(IChart chart)
        {
            if (chart == null)
                throw new ArgumentNullException("chart");

            return _charts.Contains(chart);
        }

        public void Save()
        {
        }

        public void SaveAs()
        {
        }

        public void Open()
        {
            _file = FileManager.OpenFile("", ".xml", "Xml documents (.xml)|*.xml");
            FileStream stream = File.OpenRead(_file);
            XElement.Load(stream);
        }
    }
}
