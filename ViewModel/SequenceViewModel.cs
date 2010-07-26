using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using QuickArch.Model;

namespace QuickArch.ViewModel
{
    public class SequenceViewModel : ComponentViewModel
    {
        public SequenceViewModel(Sequence sequence) : base(sequence)
        {
            if (sequence == null)
                throw new ArgumentNullException("sequence");
        }
    }
}
