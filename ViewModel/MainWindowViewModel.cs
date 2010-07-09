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
       Collection<CommandViewModel> commands;
       readonly ComponentManager componentManager;
       ObservableCollection<WorkspaceViewModel> workspaces;
       RelayCommand saveCommand;
       RelayCommand saveAsCommand;
       RelayCommand openCommand;
       #endregion

       //Constructor
       public MainWindowViewModel()
       {
           componentManager = new ComponentManager();
           base.DisplayName = Resources.MainWindowViewModel_DisplayName;
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
           ComponentDiagramViewModel workspace = new ComponentDiagramViewModel(componentManager);
           this.Workspaces.Add(workspace);
           this.SetActiveWorkspace(workspace);
       }
       void CreateNewComponent()
       {
           Component component = new Component();
           ComponentViewModel newComponentViewModel = new ComponentViewModel(component, componentManager);
           newComponentViewModel.Save();
       }

       void SetActiveWorkspace(WorkspaceViewModel workspace)
       {
           Debug.Assert(this.Workspaces.Contains(workspace));

           System.ComponentModel.ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
           if(collectionView != null)
               collectionView.MoveCurrentTo(workspace);
       }
       #endregion

       #region SaveCommand
       //returns the command that attempts to save all of the data in the component diagrams.
       public ICommand SaveCommand
       {
           get
           {
               if (saveCommand == null)
                   saveCommand = new RelayCommand(param => componentManager.Save());

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
                   saveAsCommand = new RelayCommand(param => componentManager.SaveAs());

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
                   openCommand = new RelayCommand(param => componentManager.Open());

               return openCommand;
           }
       }
       #endregion
   }
}
