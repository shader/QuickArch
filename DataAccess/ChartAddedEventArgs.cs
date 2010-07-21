using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickArch.ViewModel;

namespace QuickArch.DataAccess
{
    public class ChartAddedEventArgs : EventArgs
    {
        public ChartAddedEventArgs(IChart chart)
        {
            this.NewChart = chart;
        }

        public IChart NewChart { get; private set; }
    }
}
