namespace ShowMeTheWeather.Utils
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Describes specific command to a spefici object
    /// </summary>
    public class DelegateCommand : ICommand
    {
        // some conditions to execute a command as a predicate
        private readonly Predicate<object> _canExecute;
        // executable action - reference to method
        private readonly Action<object> _execute;
        // handler to determine if execution of some action is possible or not
        public event EventHandler CanExecuteChanged;

        public DelegateCommand(Action<object> execute) : this(execute, null) { }

        public DelegateCommand(Action<object> execute, Predicate<object> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        /// <summary>
        /// Checks if it's possible to start action
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
            {
                return true;
            }
            return _canExecute(parameter);
        }
        /// <summary>
        /// Starts making some action with specific parameter
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
        /// <summary>
        /// Raises event that allows some program parts to know if it's possible to start some action
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
