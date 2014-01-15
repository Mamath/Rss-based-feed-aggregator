using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Client_WinPhone.Utils
{
    public class RelayCommand : ICommand
    {
        private Action<object> exec;
        private Func<object, bool> canExecute;

        private RelayCommand() { }

        public RelayCommand(Action<object> ex)
        {
            this.exec = ex;
            this.canExecute = parem => true;
        }

        public RelayCommand(Action<object> ex, Func<object, bool> canExec)
        {
            this.exec = ex;
            this.canExecute = canExec;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this.exec(parameter);
        }
    }
}
