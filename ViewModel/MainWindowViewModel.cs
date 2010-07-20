using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using QuickArch.DataAccess;
using System.Collections.Specialized;
using QuickArch.Model;
using System.Diagnostics;
using System.Windows.Data;
using QuickArch.Properties;
using System.Windows.Input;

namespace QuickArch.ViewModel
{
   public class MainWindowViewModel : WorkspaceViewModel
   {
       #region Fields
       //Collection of commands to be displayed in UI
       Collection<CommandViewModel> _commands;
       //Maintain a list of ComponentManagers created and used by ComponentDiagrams
       List<ComponentManager> _componentManagers;
       //ObservableCollection of workspaces (component diagrams for now, maybe sequence charts later)
       ObservableCollection<WorkspaceViewModel> _workspaces;
       //List of DocumentViewModels created by the user/window
       ObservableCollection<DocumentViewModel> _documents;
       RelayCommand _newDocumentCommand;
       RelayCommand _saveCommand;
       RelayCommand _saveAsCommand;
       RelayCommand _openCommand;
       private int i;
       private int j;
       #endregion

       //Constructor
       public MainWindowViewModel()
       {
           _componentManagers = new List<ComponentManager>();
           _documents = new ObservableCollection<DocumentViewModel>();
           _commands = new Collection<CommandViewModel>(this.CreateCommands());
           base.DisplayName = Resources.MainWindowViewModel_DisplayName;

           //private counters for document name/component diagram name
           i = 0;
           j = 0;

           this.CreateNewDocument();
           this.CreateNewComponentDiagram();
       }

       #region Commands
       //Returns a read-only list of commands that the UI can display and execute
       public Collection<CommandViewModel> Commands
       {
           get
           {
               if (_commands == null)
               {
                   List<CommandViewModel> cmds = this.CreateCommands();
                   _commands = new Collection<CommandViewModel>(cmds);
               }
               return _commands;
           }
       }

       List<CommandViewModel> CreateCommands()
       {
           return new List<CommandViewModel>
           {
               new CommandViewModel(Resources.MainWindowViewModel_Command_CreateNewComponentDiagram, new RelayCommand(param => this.CreateNewComponentDiagram()),true),
               new CommandViewModel(Resources.MainWindowViewModel_Command_CreateNewComponent, new RelayCommand(param => this.CreateNewComponent()),true),
               new CommandViewModel("Create New Link", new RelayCommand(param => this.CreateNewConnector()),true)
               //new CommandViewModel(Resources.MainWindowViewModel_Command_CreateNewConnector, new RelayCommand(param => this.CreateNewConnector()))
           };
       }
       #endregion

       #region Workspaces

       public ObservableCollection<WorkspaceViewModel> Workspaces
       {
           get
           {
               if (_workspaces == null)
               {
                   _workspaces = new ObservableCollection<WorkspaceViewModel>();
                   _workspaces.CollectionChanged += this.OnWorkspacesChanged;
               }
               return _workspaces;
           }
       }

       void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
       {
           if (e.NewItems != null && e.NewItems.Count != 0)
               foreach (WorkspaceViewModel workspace in e.NewItems)
                   workspace.RequestClose += this.OnWorkspaceRequestClose;

           if (e.OldItems != null && e.OldItems.Count != 0)
               foreach (WorkspaceViewModel workspace in e.OldItems)
                   workspace.RequestClose -= this.OnWorkspaceRequestClose;
       }

       void OnWorkspaceRequestClose(object sender, EventArgs e)
       {
           WorkspaceViewModel workspace = sender as WorkspaceViewModel;
           workspace.Dispose();
           this.Workspaces.Remove(workspace);
       }
       #endregion

       #region Documents
       public ObservableCollection<DocumentViewModel> Documents
       {
           get
           {
               if(_documents == null)
               {
                   _documents = new ObservableCollection<DocumentViewModel>();
                   _documents.CollectionChanged += this.OnDocumentsChanged;
               }
               return _documents;
           }
       }

       void OnDocumentsChanged(object sender, NotifyCollectionChangedEventArgs e)
       {
           if (e.NewItems != null && e.NewItems.Count != 0)
               foreach (DocumentViewModel document in e.NewItems)
                   document.RequestClose += this.OnDocumentRequestClose;

           if (e.OldItems != null && e.OldItems.Count != 0)
               foreach (DocumentViewModel document in e.OldItems)
                   document.RequestClose -= this.OnDocumentRequestClose;
       }

       void OnDocumentRequestClose(object sender, EventArgs e)
       {
           DocumentViewModel dvm = sender as DocumentViewModel;
           dvm.Dispose();
           this.Documents.Remove(dvm);
       }
       #endregion

       #region Private Helpers

       void CreateNewComponentDiagram()
       {
           j++;
           ComponentManager newComponentManager = new ComponentManager();
           ComponentDiagramViewModel workspace = new ComponentDiagramViewModel(newComponentManager, "Component Diagram " + j);
           //get current document and add component diagram to it
           //ERRORS
           for(int count = 0; count < _documents.Count; count++)
           {
               if (_documents.ElementAt<DocumentViewModel>(count).IsSelected || _documents.Count==1)
               {
                   _documents.ElementAt<DocumentViewModel>(count).Add(workspace);
               }
           }
           this.Workspaces.Add(workspace);
           this.SetActiveWorkspace(workspace);
       }
       void CreateNewComponent()
       {
           Component component = new Component();
           ComponentDiagramViewModel current = GetActiveWorkspace() as ComponentDiagramViewModel;
           ComponentViewModel newComponentViewModel = new ComponentViewModel(component, current.GetComponentManager());
           newComponentViewModel.Save();
       }
       void CreateNewConnector()
       {
           Connector connector = new Connector();
           ComponentDiagramViewModel current = GetActiveWorkspace() as ComponentDiagramViewModel;
           ConnectorViewModel newConnectorViewModel = new ConnectorViewModel(connector, current.GetComponentManager());
           newConnectorViewModel.Save();
           //Mouse.OverrideCursor = Cursors.Cross;
       }
       void CreateNewDocument()
       {
           i++;
           Document document = new Document("Document " + i);
           DocumentViewModel newDocument = new DocumentViewModel(document);
           _documents.Add(newDocument);
       }

       void SetActiveWorkspace(WorkspaceViewModel workspace)
       {
           Debug.Assert(this.Workspaces.Contains(workspace));

           System.ComponentModel.ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
           if(collectionView != null)
               collectionView.MoveCurrentTo(workspace);
       }
       WorkspaceViewModel GetActiveWorkspace()
       {
           System.ComponentModel.ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
           if (collectionView != null)
           {
               return collectionView.CurrentItem as WorkspaceViewModel;
           }
           return null;
       }
       #endregion
       
       #region NewDocumentCommand
       public ICommand NewDocumentCommand
       {
           get
           {
               if(_newDocumentCommand == null)
                  _newDocumentCommand = new RelayCommand(param => this.CreateNewDocument());

               return _newDocumentCommand;
           }
       }
       #endregion
       
       #region SaveCommand
       //returns the command that attempts to save all of the data in the component diagrams.
       public ICommand SaveCommand
       {
           get
           {
               if (_saveCommand == null)
                   _saveCommand = new RelayCommand(param => _documents.ElementAt<DocumentViewModel>(0).Save());

               return _saveCommand;
           }
       }
       #endregion
       /*
       
       #region SaveAsCommand
       //returns the command that attempts to save all of the data in the component diagrams.
       public ICommand SaveAsCommand
       {
           get
           {
               if (saveAsCommand == null)
                   saveAsCommand = new RelayCommand(param => document.SaveAs());

               return saveAsCommand;
           }
       }
       #endregion

       #region OpenCommand
       //returns the command that loads a document
       public ICommand OpenCommand
       {
           get
           {
               if (openCommand == null)
                   openCommand = new RelayCommand(param => document.Open());

               return openCommand;
           }
       }
       #endregion
       */
   }
}
