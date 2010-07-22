using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Data;
using System.Windows.Input;

using QuickArch.Properties;
using QuickArch.Model;
using QuickArch.DataAccess;

namespace QuickArch.ViewModel
{
   public class MainWindowViewModel : WorkspaceViewModel
   {
       #region Fields
       //Collection of commands to be displayed in UI
       Collection<CommandViewModel> _fileCommands, _editCommands, _viewCommands, _toolCommands, _diagramCommands, _documentCommands ;
       private RelayCommand _textBoxEnterCommand;
       //Maintain a list of ComponentManagers created and used by ComponentDiagrams
       List<ComponentManager> _componentManagers;
       //ObservableCollection of workspaces (component diagrams for now, maybe sequence charts later)
       ObservableCollection<WorkspaceViewModel> _workspaces;
       //List of DocumentViewModels created by the user/window
       ObservableCollection<DocumentViewModel> _documents;
       Document document;
       private int i;
       private int j;
       #endregion

       #region Constructor
       public MainWindowViewModel()
       {
           base.DisplayName = Resources.MainWindowViewModel_DisplayName;

           _componentManagers = new List<ComponentManager>();
           _documents = new ObservableCollection<DocumentViewModel>();

           //private counters for document name/component diagram name
           i = 0;
           j = 0;

           this.CreateNewDocument();
           this.CreateNewComponentDiagram();

           _fileCommands = new Collection<CommandViewModel>
               (new CommandViewModel[] {
                NewCommand("Save", param => _documents.ElementAt<DocumentViewModel>(0).Save()),
                NewCommand("Save As...", param => document.SaveAs()),
                NewCommand("Open", param => OpenDocument()),
                NewCommand("New Document", param => CreateNewDocument()),
               });

           _editCommands = new Collection<CommandViewModel>
               (new CommandViewModel[] {});

           _viewCommands = new Collection<CommandViewModel>
               (new CommandViewModel[] {});

           _toolCommands = new Collection<CommandViewModel>
               (new CommandViewModel[] {
                NewCommand("Export to PNG", param => document.ExportPng())
               });

           _diagramCommands = new Collection<CommandViewModel>
               (new CommandViewModel[] {
                   NewCommand(Resources.MainWindowViewModel_Command_CreateNewComponent, param => this.CreateNewComponent()),
                   NewCommand("Create New Link", param => this.CreateNewConnector()),
               });

           _documentCommands = new Collection<CommandViewModel>
               (new CommandViewModel[] {
                   NewCommand("New Component Diagram", param => CreateNewComponentDiagram())
               });
       }
       #endregion

       CommandViewModel NewCommand(string displayName, Action<object> execute, bool isEnabled=true)
       {
           return new CommandViewModel(displayName, new RelayCommand(execute), isEnabled);
       }

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
       void OpenDocument()
       {
           CreateNewDocument();
           document.Open();
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
           document = new Document("Document " + i);
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


       #region Presentation Properties

       #region Commands
       public Collection<CommandViewModel> FileCommands
       {
           get { return _fileCommands; }
       }
       public Collection<CommandViewModel> EditCommands
       {
           get { return _editCommands; }
       }
       public Collection<CommandViewModel> ViewCommands
       {
           get { return _viewCommands; }
       }
       public Collection<CommandViewModel> ToolCommands
       {
           get { return _toolCommands; }
       }
       public Collection<CommandViewModel> DiagramCommands
       {
           get { return _diagramCommands; }
       }
       public Collection<CommandViewModel> DocumentCommands
       {
           get { return _documentCommands; }
       }
       #endregion

       public ICommand TextBoxEnterCommand
       {
           get
           {
               if(_textBoxEnterCommand == null)
                   _textBoxEnterCommand = new RelayCommand(param => this.CreateNewComponent());

               return _textBoxEnterCommand;
           }
       }

       #endregion
   }
}