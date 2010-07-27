using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickArch.ViewModel
{
    public class TemporaryConnectorViewModel : ComponentViewModel
    {
        public TemporaryConnectorViewModel(SystemViewModel start) : base(null)
        {
            Start = start;
        }

        public SystemViewModel Start { get; set; }
    }
}
