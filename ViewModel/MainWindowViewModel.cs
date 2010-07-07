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

namespace QuickArch.ViewModel
{
   public class MainWindowViewModel : WorkspaceViewModel
   {
       #region Fields
       ReadOnlyCollection<CommandViewModel> commands;
       readonly ComponentManager componentManager;
       ObservableCollection<WorkspaceViewModel> workspaces;
       #endregion

       //Constructor
       public MainWindowViewModel()
       {
           componentManager = new ComponentManager();
           base.DisplayName = Resources.MainWindowViewModel_DisplayName;
       }

       #region Commands
       //Returns a read-only list of commands that the UI can display and execute
       public ReadOnlyCollection<CommandViewModel> Commands
       {
           get
           {
               if (commands == null)
               {
                   List<CommandViewModel> cmds = this.createCommands();
                   commands = new ReadOnlyCollection<CommandViewModel>(cmds);
               }
               return commands;
           }
       }

       List<CommandViewModel> createCommands()
       {
           return new List<CommandViewModel>
           {
               new CommandViewModel(Resources.MainWindowViewModel_Command_CreateNewComponent, new RelayCommand(param => this.CreateNewComponent())),
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

       void CreateNewComponent()
       {
           Component component = Component.CreateNewComponent();
           ComponentViewModel workspace = new ComponentViewModel(component, componentManager);
           this.Workspaces.Add(workspace);
           this.SetActiveWorkspace(workspace);
       }

       void SetActiveWorkspace(WorkspaceViewModel workspace)
       {
           Debug.Assert(this.Workspaces.Contains(workspace));

           System.ComponentModel.ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
           if(collectionView != null)
               collectionView.MoveCurrentTo(workspace);
       }
       #endregion
   }
}
