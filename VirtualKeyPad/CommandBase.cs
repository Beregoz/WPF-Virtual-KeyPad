using System;
using System.Windows.Input;

namespace VirtualKeyPad
{
	public class CommandBase : ICommand
	{
		private static readonly Predicate<object> _defaultCanExecute = a => true;
		private Predicate<object> _canExecute;
		private Action<object> _execute;

		private static Predicate<object> DefaultCanExecute => _defaultCanExecute;


        public CommandBase()
		{
		}

		public CommandBase(Action<object> execute)
		{
			this._canExecute = DefaultCanExecute;
			this._execute = execute;
		}

		public CommandBase(Action<object> execute, Predicate<object> canExecute)
		{
			this._canExecute = canExecute;
			this._execute = execute;
		}

		public virtual event EventHandler CanExecuteChanged
		{
			add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

		public bool CanExecute(object parameter)
		{
			return this._canExecute(parameter);
		}

		public void Execute(object parameter)
		{
			this._execute(parameter);
		}

		public void SetExecuteMethods(Action<object> execute, Predicate<object> canExecute = null)
		{
			this._canExecute = canExecute ?? _defaultCanExecute;
			this._execute = execute;
		}
	}
}