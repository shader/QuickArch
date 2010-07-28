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
            Bitmap b = icon.ToBitmap();
            b.Save(strm, System.Drawing.Imaging.ImageFormat.Png);
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = strm;
            bi.EndInit();

            Icon = bi;
        }

        public ICommand Command { get; private set; }

        public bool IsEnabled { get; set; }

        public BitmapImage Icon
        {
            get;
            private set;
        }
    }
}