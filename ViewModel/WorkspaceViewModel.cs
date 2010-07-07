using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace QuickArch.ViewModel
{
    public abstract class WorkspaceViewModel : ViewModelBase
    {
        #region Fields
        RelayCommand closeCommand;
        #endregion

        #region Constructor
        protected WorkspaceViewModel()
        {
        }
        #endregion
        #region CloseCommand
        //returns the command that attempts to remove this workspace from the UI
        public ICommand CloseCommand
        {
            get
            {
                if (closeCommand == null)
                    closeCommand = new RelayCommand(param => this.OnRequestClose());

                return closeCommand;
            }
        }
        #endregion
        #region RequestClose [event]
        //raised when this workspace should be removed from the UI
        public event EventHandler RequestClose;

        void OnRequestClose()
        {
            EventHandler handler = this.RequestClose;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
        #endregion
    }
}
