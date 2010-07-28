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
using System.Drawing;

namespace QuickArch.ViewModel
{
   public class MainWindowViewModel : ViewModelBase
   {
       #region Fields
       //Collection of commands to be displayed in UI
       Collection<CommandViewModel> _fileCommands, _editCommands, _viewCommands, _toolCommands, _systemCommands, _treeviewCommands ;
       RelayCommand _textBoxEnterCommand, _linkButtonCommand;
       //ObservableCollection of components
       #endregion

       #region Properties
       public ObservableCollection<ComponentViewModel> TreeVMs { get; private set; }
       public ObservableCollection<ComponentViewModel> TabVMs { get; private set; }
       public ComponentViewModel SelectedComponentVM { get; set; }

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

           #region Build Command Collections
           _fileCommands = new Collection<CommandViewModel>
               (new CommandViewModel[] {
                NewCommand("Save", param => SelectedComponentVM.Save(), Resources.save),
                NewCommand("Save As...", param => SelectedComponentVM.SaveAs(), Resources.save),
                NewCommand("Save All", param => SaveAll(),Resources.save),
                NewCommand("Open", param => OpenSystem(), Resources.folder),
                NewCommand("New Document", param => CreateNewDocument(Resources.DefaultDocumentName), Resources.document),
                NewCommand("New Subsystem", param => CreateNewSystem(), Resources.gear)
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
                   NewCommand("New Subsystem", param => CreateNewSystem(), Resources.gear),
                   //NewCommand("Create New Link", param => this.CreateNewConnector(), new Icon("")),
                   NewCommand("Delete", param => this.DeleteSystem(), Resources.delete)
               });

           _treeviewCommands = new Collection<CommandViewModel>
           (new CommandViewModel[] {
               NewCommand("New Document", param => CreateNewDocument(Resources.DefaultDocumentName), Resources.document)
           });
           #endregion
       }
       #endregion

       CommandViewModel NewCommand(string displayName, Action<object> execute, Icon icon, bool isEnabled=true)
       {
           return new CommandViewModel(displayName, new RelayCommand(execute), isEnabled, icon);
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
       #endregion

       #region Private Helpers

       void OpenSystem()
       {
           string filename = FileManager.OpenFile(Resources.DefaultFilename, Resources.Extension, Resources.Filter);
           if (filename != null)
           {
               SystemViewModel sys = new SystemViewModel(new QuickArch.Model.System(filename));
               TreeVMs.Add(sys);
               TabVMs.Add(sys);
           }
       }

       void CreateNewSystem()
       {
           CreateNewSystem(Resources.DefaultComponentName);
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
       }

       void DeleteSystem(SystemViewModel sys)
       {
           if (TreeVMs.Contains(sys))
           {
               TreeVMs.Remove(sys);
               TabVMs.Remove(sys);
               sys.Dispose();
               sys = null;
           }
           else
           {
               sys.Delete();
           }
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
       public Collection<CommandViewModel> TreeViewCommands
       {
           get { return _treeviewCommands; }
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
                   _linkButtonCommand = new RelayCommand(param => (DisplayedComponent as SystemViewModel).ComponentSelected += 
                       (DisplayedComponent as SystemViewModel).StartTempConnector);

               return _linkButtonCommand;
           }
       }

       #endregion
   }
}