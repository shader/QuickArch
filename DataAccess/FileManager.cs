using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace QuickArch.DataAccess
{
    public class FileManager
    {
        public static string GetSaveFile(string defaultName, string defaultExt, string Filter)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = defaultName;
            dlg.DefaultExt = defaultExt; // Default file extension
            dlg.Filter = Filter; // Filter files by extension

            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                return dlg.FileName;
            }
            else return null;
        }

        public static string OpenFile(string name, string ext, string filter)
        {
            // Configure save file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = name; // Default file name
            dlg.DefaultExt = ext; // Default file extension
            dlg.Filter = filter; // Filter files by extension
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
