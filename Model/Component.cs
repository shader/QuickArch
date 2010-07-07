using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace QuickArch.Model
{
    public class Component
    {
        public static Component CreateNewComponent()
        {
            return new Component();
        }

        public static Component CreateComponent(string title, string parent)
        {
            return new Component
            {
                Title = title,
                Parent = parent
            };
        }

        protected Component()
        {
        }

        public string Title { get; set; }

        public string Parent { get; set; }
    }
}
