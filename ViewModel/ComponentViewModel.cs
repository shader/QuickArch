using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using QuickArch.Model;
using QuickArch.Utilities;
using QuickArch.Properties;

namespace QuickArch.ViewModel
{
    public delegate void ComponentSelectionHandler(ComponentViewModel sender, EventArgs e);

    public abstract class ComponentViewModel : ViewModelBase
    {
        #region Fields
        protected Component _component;
        bool _isSelected;
        #endregion

        public ComponentViewModel(Component component)
        {
            _component = component;
            DisplayName = component.Title;
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
                if (true == value)
                {
                    Selected(this, EventArgs.Empty);
                }
            }
        }

        public event ComponentSelectionHandler Selected;

        public void Save()
        {
            if (_component != null)
            {
                _component.Save();
            }
        }
        public void SaveAs()
        {
            if (_component != null)
            {
                string filename = FileManager.GetSaveFile(_component.Title, Resources.Extension, Resources.Filter);
                if (filename != null)
                    _component.SaveAs(filename);
            }
        }
    }
}
