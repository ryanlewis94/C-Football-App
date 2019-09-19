using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FootballApp.Commands
{
    public class DelegateCommand<T> : ICommand
    {
        private Action<T> _action;
        private Func<bool> _predicate;

        public DelegateCommand(Action<T> _action, Func<bool> _predicate)
        {
            this._action = _action;
            this._predicate = _predicate;
        }

        private void Execute(T parameter)
        {
            if (_action != null)
            {
                _action(parameter);
            }
        }

        public bool CanExecute(object parameter)
        {
            if (_predicate != null)
            {
                return _predicate();
            }

            return true;
        }

        public void Execute(object parameter)
        {
            if (_action != null)
            {
                _action((T)parameter);
            }
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
