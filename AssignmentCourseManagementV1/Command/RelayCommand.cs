using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AssignmentCourseManagementV1.Command
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute {  get; set; }
        private Predicate<object> canExecute { get; set; }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null) 
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            execute(parameter);
        }
    }
}
