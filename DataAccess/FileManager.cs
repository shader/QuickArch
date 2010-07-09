using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickArch.DataAccess
{
    public class FileManager
    {
        static string _file;

        public static string File 
        {
            get
            {
                if (_file == null)
                    _file = SaveFile();
                return _file;
            }
            set { _file = value; } 
        }

        public static string SaveFile()
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "Xml documents (.xml)|*.xml"; // Filter files by extension
            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                return dlg.FileName;
            }
            else return null;
        }

        public static string OpenFile()
        {
            // Configure save file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".xml"; // Default file extension
            dlg.Filter = "Xml documents (.xml)|*.xml"; // Filter files by extension
            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                return dlg.FileName;
            }
            else return null;
        }
    }
}
