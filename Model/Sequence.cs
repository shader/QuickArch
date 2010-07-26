using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace QuickArch.Model
{
    public class Sequence : Component
    {
        public Sequence(XElement source) : this(source, null) { }

        public Sequence(XElement source, System parent)
        {
        }

        public override void Save()
        {
        }

        public override void Save(XElement parent)
        {
        }
    }
}
