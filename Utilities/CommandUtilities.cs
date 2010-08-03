using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuickArch.ViewModel;
using System.Drawing;

namespace QuickArch.Utilities
{
    public static class CommandUtilities
    {
        public static CommandViewModel NewCommand(string displayName, Action<object> execute, Icon icon, bool isEnabled = true)
        {
            return new CommandViewModel(displayName, new RelayCommand(execute), isEnabled, icon);
        }
    }
}
