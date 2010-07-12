using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickArch.DataAccess;

namespace QuickArch.ViewModel
{
    public class DocumentViewModel : WorkspaceViewModel
    {
        readonly Document _document;
        bool isSelected;
        RelayCommand saveCommand;
        RelayCommand openCommand;

        public DocumentViewModel(Document document)
        {
            if (document == null)
                throw new ArgumentNullException("document");
            
            _document = document;
            
            base.DisplayName = "Document1";
        }
    }
}
