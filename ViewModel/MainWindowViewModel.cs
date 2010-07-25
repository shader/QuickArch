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
using QuickArch.Utilities;

namespace QuickArch.ViewModel
{
   public class MainWindowViewModel : ViewModelBase
   {
       #region Fields
       //Collection of commands to be displayed in UI
       Collection<CommandViewModel> _fileCommands, _editCommands, _viewCommands, _toolCommands, _systemCommands ;
       RelayCommand _textBoxEnterCommand;
       //ObservableCollection of components
       ObservableCollection<ComponentViewModel> _componentVMs;
       #endregion

       #region Constructor
       public MainWindowViewModel()
       {
           DisplayName = Resources.MainWindowViewModel_DisplayName;

           _componentVMs = new ObservableCollection<ComponentViewModel>();

           CreateNewSystem();

           _fileCommands = new Collection<CommandViewModel>
               (new CommandViewModel[] {
                NewCommand("Save", param => GetActiveSystem().Save()),
                NewCommand("Save As...", param => GetActiveSystem().SaveAs()),
                NewCommand("Open", param => OpenDocument()),
                NewCommand("New Document", param => CreateNewSystem()),
               });

           _editCommands = new Collection<CommandViewModel>
               (new CommandViewModel[] {});

           _viewCommands = new Collection<CommandViewModel>
               (new CommandViewModel[] {});

           _toolCommands = new Collection<CommandViewModel>();
               //(new CommandViewModel[] {
               // NewCommand("Export to PNG", param => GetActiveSystem().ExportPng())
               //});
           
           _systemCommands = new Collection<CommandViewModel>
               (new CommandViewModel[] {
                   NewCommand("New Subsystem", param => CreateNewSystem()),
                   NewCommand("Create New Link", param => this.CreateNewConnector())
               });
       }
       #endregion

       CommandViewModel NewCommand(string displayName, Action<object> execute, bool isEnabled=true)
       {
           return new CommandViewModel(displayName, new RelayCommand(execute), isEnabled);
       }

       #region Components
       public ObservableCollection<ComponentViewModel> ComponentVMs
       {
           get
           {
               if(_componentVMs == null)
               {
                   _componentVMs = new ObservableCollection<ComponentViewModel>();
                   _componentVMs.CollectionChanged += this.OnComponentVMsChanged;
               }
               return _componentVMs;
           }
       }

       void OnComponentVMsChanged(object sender, NotifyCollectionChangedEventArgs e)
       {
           if (e.NewItems != null && e.NewItems.Count != 0)
               foreach (SystemViewModel document in e.NewItems)
                   document.RequestClose += this.OnSystemRequestClose;

           if (e.OldItems != null && e.OldItems.Count != 0)
               foreach (SystemViewModel document in e.OldItems)
                   document.RequestClose -= this.OnSystemRequestClose;
       }

       void OnSystemRequestClose(object sender, EventArgs e)
       {
           SystemViewModel dvm = sender as SystemViewModel;
           dvm.Dispose();
           this.ComponentVMs.Remove(dvm);
       }
       void OpenDocument()
       {
       }
       #endregion

       #region Private Helpers

       void CreateNewSystem()
       {
           CreateNewSystem(Resources.DefaultComponentName);
       }
       //overloaded method
       void CreateNewSystem(String title)
       {
           SystemViewModel current = GetActiveSystem() as SystemViewModel;
           SystemViewModel newSystemViewModel = new SystemViewModel(new QuickArch.Model.System(title));
           ComponentVMs.Add(newSystemViewModel);
       }
       void CreateNewConnector()
       {
           Connector connector = new Connector();
           SystemViewModel current = GetActiveSystem() as SystemViewModel;
           ConnectorViewModel newConnectorViewModel = new ConnectorViewModel(connector);
       }

       void SetActiveSystem(ComponentViewModel system)
       {
           Debug.Assert(this.ComponentVMs.Contains(system));

           System.ComponentModel.ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.ComponentVMs);
           if(collectionView != null)
               collectionView.MoveCurrentTo(system);
       }
       ComponentViewModel GetActiveSystem()
       {
           System.ComponentModel.ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.ComponentVMs);
           if (collectionView != null)
           {
               return collectionView.CurrentItem as ComponentViewModel;
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
       public Collection<CommandViewModel> DocumentCommands
       {
           get { return _systemCommands; }
       }
       #endregion

       public ICommand TextBoxEnterCommand
       {
           get
           {
               if(_textBoxEnterCommand == null)
                   _textBoxEnterCommand = new RelayCommand(param => this.CreateNewSystem((String)param));

               return _textBoxEnterCommand;
           }
       }

       #endregion
   }
}