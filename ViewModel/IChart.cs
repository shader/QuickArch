using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Xml.Linq;

using QuickArch.ViewModel;
using QuickArch.DataAccess;

namespace QuickArch.ViewModel
{
    public interface IChart
    {
        void Save(XElement parent);
        void Load(XElement parent);
        event PropertyChangedEventHandler PropertyChanged;
    }
}