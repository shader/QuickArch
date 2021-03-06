﻿using System;
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

        public Component Component
        {
            get { return _component; }
        }

        public ComponentViewModel(Component component)
        {
            _component = component;
        }

        public Dictionary<String,String> Properties 
        {
            get
            {
                return _component.Properties;
            }
        }
        
        public override string DisplayName
        {
            get
            {
                return _component.Name;
            }
            set
            {
                base.DisplayName = value;
                _component.Name = value;
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
                    OnPropertyChanged("IsSelected");
                }
                if (true == value && Selected != null)
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
                if (_component.Filename != null)
                    _component.Save();
                else
                    SaveAs();
            }
        }
        public void SaveAs()
        {
            if (_component != null)
            {
                string filename = FileManager.GetSaveFile(_component.Name, Resources.Extension, Resources.Filter);
                if (filename != null)
                    _component.SaveAs(filename);
            }
        }
    }
}
