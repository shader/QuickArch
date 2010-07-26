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
       RelayCommand _textBoxEnterCommand, _linkButtonCommand;
       //ObservableCollection of components
       #endregion

       #region Properties
       public ObservableCollection<ComponentViewModel> TreeVMs { get; private set; }
       public ObservableCollection<ComponentViewModel> TabVMs { get; private set; }
       public ComponentViewModel SelectedComponentVM { get; private set; }

       ComponentViewModel DisplayedComponent
       {
           get
           {
               System.ComponentModel.ICollectionView collectionView = CollectionViewSource.GetDefaultView(TabVMs);
               if (collectionView != null)
               {
                   return collectionView.CurrentItem as ComponentViewModel;
               }
               return null;
           }
           set
           {
               if (!TabVMs.Contains(value))
                   TabVMs.Add(value);

               System.ComponentModel.ICollectionView collectionView = CollectionViewSource.GetDefaultView(TabVMs);
               if (collectionView != null)
                   collectionView.MoveCurrentTo(value);
           }
       }
       #endregion

       #region Constructor
       public MainWindowViewModel()
       {
           DisplayName = Resources.MainWindowViewModel_DisplayName;

           TreeVMs = new ObservableCollection<ComponentViewModel>();
           TreeVMs.CollectionChanged += OnComponentVMsChanged;

           TabVMs = new ObservableCollection<ComponentViewModel>();
           TabVMs.CollectionChanged += OnComponentVMsChanged;

           _fileCommands = new Collection<CommandViewModel>
               (new CommandViewModel[] {
                NewCommand("Save", param => SelectedComponentVM.Save()),
                NewCommand("Save As...", param => SelectedComponentVM.SaveAs()),
                NewCommand("Save All", param => SaveAll()),
                NewCommand("Open", param => OpenComponent()),
                NewCommand("New Document", param => CreateNewDocument(Resources.DefaultName)),
                NewCommand("New System", param => CreateNewSystem())
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
       void OnComponentVMsChanged(object sender, NotifyCollectionChangedEventArgs e)
       {
           if (e.NewItems != null && e.NewItems.Count != 0)
               foreach (ComponentViewModel cvm in e.NewItems)
               {
                   cvm.RequestClose += OnComponentRequestClose;
                   cvm.Selected += OnComponentSelected;
                   if (cvm is SystemViewModel)
                   {
                       ((SystemViewModel)cvm).ComponentVMs.CollectionChanged += OnComponentVMsChanged;
                   }
               }

           if (e.OldItems != null && e.OldItems.Count != 0)
               foreach (ComponentViewModel cvm in e.OldItems)
               {
                   cvm.RequestClose -= OnComponentRequestClose;
                   cvm.Selected -= OnComponentSelected;
                   if (cvm is SystemViewModel)
                   {
                       ((SystemViewModel)cvm).ComponentVMs.CollectionChanged += OnComponentVMsChanged;
                   }
               }
       }

       void OnComponentRequestClose(object sender, EventArgs e)
       {          
           TabVMs.Remove((ComponentViewModel)sender);
       }

       void OnComponentSelected(ComponentViewModel cvm, EventArgs e)
       {
           SelectedComponentVM = cvm;
       }

       void OnComponentAdded(object sender, EventArgs e)
       {

       }
       #endregion

       #region Private Helpers

       void CreateNewSystem()
       {
           CreateNewSystem(Resources.DefaultComponentName);
       }
       void OpenComponent()
       {
       }
       //overloaded method
       void CreateNewSystem(String title)
       {
           if (SelectedComponentVM != null && SelectedComponentVM is SystemViewModel)
           {
               ((SystemViewModel)SelectedComponentVM).AddSubsystem(title);
           }
           //for top level system
           else
           {
               SystemViewModel sys = new SystemViewModel(new QuickArch.Model.System(title, null));
               TreeVMs.Add(sys);
               TabVMs.Add(sys);
           }
       }

       void CreateNewDocument(String title)
       {
           SystemViewModel sys = new SystemViewModel(new QuickArch.Model.System(title, null));
           TreeVMs.Add(sys);
           TabVMs.Add(sys);
       }

       void CreateNewConnector()
       {
           Connector connector = new Connector();
           SystemViewModel current = DisplayedComponent as SystemViewModel;
           ConnectorViewModel newConnectorViewModel = new ConnectorViewModel(connector);
           current.ComponentVMs.Add(newConnectorViewModel);
       }

       void SaveAll()
       {
           foreach (ComponentViewModel cvm in TreeVMs) 
           {
               cvm.Save();
           }
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
       public Collection<CommandViewModel> SystemCommands
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

       public ICommand LinkButtonCommand
       {
           get
           {
               if (_linkButtonCommand == null)
                   _linkButtonCommand = new RelayCommand(param => this.CreateNewConnector());

               return _linkButtonCommand;
           }
       }

       #endregion
   }
}