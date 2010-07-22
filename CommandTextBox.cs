using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace QuickArch
{
    public class CommandTextBox : TextBox, ICommandSource
    {
        public CommandTextBox() : base() 
        {
        }

        #region Dependency Properties
        // Make Command a dependency property so it can use databinding.
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
          "Command",
          typeof(ICommand),
          typeof(CommandTextBox),
          new PropertyMetadata((ICommand)null,
          new PropertyChangedCallback(CommandChanged)));

        public ICommand Command
        {
            get
            {
                return (ICommand)GetValue(CommandProperty);
            }
            set
            {
                SetValue(CommandProperty, value);
            }
        }

        public static readonly DependencyProperty CommandTargetProperty = DependencyProperty.Register(
          "CommandTarget",
          typeof(IInputElement),
          typeof(CommandTextBox),
          new PropertyMetadata((IInputElement)null));

        public IInputElement CommandTarget
        {
            get
            {
                return (IInputElement)GetValue(CommandTargetProperty);
            }
            set
            {
                SetValue(CommandTargetProperty, value);
            }
        }

        public static readonly DependencyProperty CommandParameterProperty =
      DependencyProperty.Register(
          "CommandParameter",
          typeof(object),
          typeof(CommandTextBox),
          new PropertyMetadata((object)null));

        public object CommandParameter
        {
            get
            {
                return (object)GetValue(CommandParameterProperty);
            }
            set
            {
                SetValue(CommandParameterProperty, value);
            }
        }
        #endregion

        private static void CommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CommandTextBox cb = (CommandTextBox)d;
            cb.HookUpCommand((ICommand)e.OldValue, (ICommand)e.NewValue);
        }

        private void HookUpCommand(ICommand oldCommand, ICommand newCommand)
        {
            // If oldCommand is not null, then we need to remove the handlers.
            if (oldCommand != null)
            {
                RemoveCommand(oldCommand, newCommand);
            }
            AddCommand(oldCommand, newCommand);
        }

        private void RemoveCommand(ICommand oldCommand, ICommand newCommand)
        {
            EventHandler handler = CanExecuteChanged;
            oldCommand.CanExecuteChanged -= handler;
        }

        // Add the command.
        private void AddCommand(ICommand oldCommand, ICommand newCommand)
        {
            EventHandler handler = new EventHandler(CanExecuteChanged);
            canExecuteChangedHandler = handler;
            if (newCommand != null)
            {
                newCommand.CanExecuteChanged += canExecuteChangedHandler;
            }
        }
        private void CanExecuteChanged(object sender, EventArgs e)
        {

            if (this.Command != null)
            {
                RoutedCommand command = this.Command as RoutedCommand;

                // If a RoutedCommand.
                if (command != null)
                {
                    if (command.CanExecute(CommandParameter, CommandTarget))
                    {
                        this.IsEnabled = true;
                    }
                    else
                    {
                        this.IsEnabled = false;
                    }
                }
                // If a not RoutedCommand.
                else
                {
                    if (Command.CanExecute(CommandParameter))
                    {
                        this.IsEnabled = true;
                    }
                    else
                    {
                        this.IsEnabled = false;
                    }
                }
            }
        }
        //If Command is defined, hitting enter button will invoke the command
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.Key == Key.Enter)
            {
                if (this.Command != null)
                {
                    RoutedCommand command = Command as RoutedCommand;

                    if (command != null)
                    {
                        command.Execute(CommandParameter, CommandTarget);
                        this.Clear();
                    }
                    else
                    {
                        ((ICommand)Command).Execute(CommandParameter);
                        this.Clear();
                    }
                }
            }
        }
        private static EventHandler canExecuteChangedHandler;
    }

}
