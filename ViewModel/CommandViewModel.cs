using System;
using System.Windows.Input;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace QuickArch.ViewModel
{
    /// <summary>
    /// Represents an actionable item displayed by a View.
    /// </summary>
    public class CommandViewModel : ViewModelBase
    {

        public CommandViewModel(string displayName, ICommand command, bool isEnabled, Icon icon)
        {
            if (command == null)
                throw new ArgumentNullException("command");

            base.DisplayName = displayName;
            this.Command = command;
            this.IsEnabled = isEnabled;

            System.IO.MemoryStream strm = new System.IO.MemoryStream();
            icon.Save(strm);
            Icon = BitmapFrame.Create(strm);
        }

        public ICommand Command { get; private set; }

        public bool IsEnabled { get; set; }

        public BitmapFrame Icon
        {
            get;
            private set;
        }
    }
}