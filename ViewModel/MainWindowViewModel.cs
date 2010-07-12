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
       Collection<CommandViewModel> commands;
       //Maintain a list of ComponentManagers created and used by ComponentDiagrams
       List<ComponentManager> componentManagers;
       //ObservableCollection of workspaces (component diagrams for now, maybe sequence charts later)
       ObservableCollection<WorkspaceViewModel> workspaces;
       //List of DocumentViewModels created by the user/window
       List<DocumentViewModel> documents;
       RelayCommand newDocumentCommand;
       RelayCommand saveCommand;
       RelayCommand saveAsCommand;
       RelayCommand openCommand;
       private int i;
       #endregion

       //Constructor
       public MainWindowViewModel()
       {
           componentManagers = new List<ComponentManager>();
           documents = new List<DocumentViewModel>();
           base.DisplayName = Resources.MainWindowViewModel_DisplayName;
           i = 0;
       }

       #region Commands
       //Returns a read-only list of commands that the UI can display and execute
       public Collection<CommandViewModel> Commands
       {
           get
           {
               if (commands == null)
               {
                   List<CommandViewModel> cmds = this.CreateCommands();
                   commands = new Collection<CommandViewModel>(cmds);
               }
               return commands;
           }
       }

       List<CommandViewModel> CreateCommands()
       {
           return new List<CommandViewModel>
           {
               new CommandViewModel(Resources.MainWindowViewModel_Command_CreateNewComponentDiagram, new RelayCommand(param => this.CreateNewComponentDiagram()),true),
               new CommandViewModel(Resources.MainWindowViewModel_Command_CreateNewComponent, new RelayCommand(param => this.CreateNewComponent()),true)
               //new CommandViewModel(Resources.MainWindowViewModel_Command_CreateNewConnector, new RelayCommand(param => this.CreateNewConnector()))
           };
       }
       #endregion

       #region Workspaces

       public ObservableCollection<WorkspaceViewModel> Workspaces
       {
           get
           {
               if (workspaces == null)
               {
                   workspaces = new ObservableCollection<WorkspaceViewModel>();
                   workspaces.CollectionChanged += this.OnWorkspacesChanged;
               }
               return workspaces;
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

       #region Private Helpers

       void CreateNewComponentDiagram()
       {
           ComponentManager newComponentManager = new ComponentManager();
           ComponentDiagramViewModel workspace = new ComponentDiagramViewModel(newComponentManager);
           this.Workspaces.Add(workspace);
           this.SetActiveWorkspace(workspace);
       }
       void CreateNewComponent()
       {
           Component component = new Component();
           ComponentDiagramViewModel current = GetActiveWorkspace() as ComponentDiagramViewModel;
           ComponentViewModel newComponentViewModel = new ComponentViewModel(component, current.getComponentManager());
           newComponentViewModel.Save();
       }
       void CreateNewDocument()
       {
           i++;
           Document document = new Document("Document " + i);
           DocumentViewModel newDocument = new DocumentViewModel(document);
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
               //if(newDocumentCommand == null)
                  // newDocumentCommand = new RelayCommand(param => this.CreateNewDocument());

               return newDocumentCommand;
           }
       }
       #endregion

       /*
       #region SaveCommand
       //returns the command that attempts to save all of the data in the component diagrams.
       public ICommand SaveCommand
       {
           get
           {
               if (saveCommand == null)
                   saveCommand = new RelayCommand(param => document.Save());

               return saveCommand;
           }
       }
       #endregion

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
