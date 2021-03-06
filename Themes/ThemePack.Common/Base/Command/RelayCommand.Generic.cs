﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ThemePack.Common.Helpers;
using ThemePack.Common.Models;

namespace ThemePack.Common.Base.Command
{
    /// <summary>
    /// Relay command.
    /// </summary>
    /// <date>12:49 05/15/2015</date>
    /// <author>Anton Liakhovich</author>
    public class RelayCommand<T> : ICommand
    {
        /// <summary>
        /// <see cref="Action{T}"/> to execute.
        /// </summary>
        private readonly Action<T> execute;

        /// <summary>
        /// <see cref="Predicate{T}"/> to check if <see cref="execute"/> can be executed.
        /// </summary>
        private readonly Predicate<T> canExecute;

        /// <summary>
        /// Initializes a new instance of <see cref="RelayCommand{T}"/>
        /// </summary>
        /// <exception cref="ArgumentException">execute</exception>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute = null)
        {
            if (execute == null)
            {
                throw new ArgumentException("execute");
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Event to handle before <see cref="execute"/> execution.
        /// </summary>
        public event EventHandler<DataEventArgs<object>> PreExecution;

        /// <summary>
        /// Event to handle after <see cref="execute"/> execution.
        /// </summary>
        public event EventHandler<DataEventArgs<object>> PostExecution;

        #region ICommand Implementation

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <returns>true - if this command can be executed; otherwise, false.</returns>
        /// <exception cref="Exception"><see cref="canExecute"/>'s callback throws an exception.</exception>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            if (canExecute == null)
            {
                return true;
            }

            T converted;
            ConversionHelper.TryConvertValue(parameter, out converted);

            return canExecute(converted);
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (canExecute != null)
                {
                    CommandManager.RequerySuggested += value;
                }
            }
            remove
            {
                if (canExecute != null)
                {
                    CommandManager.RequerySuggested -= value;
                }
            }
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to null.</param>
        /// <exception cref="Exception"><see cref="PreExecution"/>'s, <see cref="PostExecution"/>'s or <see cref="execute"/>'s callback throws an exception.</exception>
        public void Execute(object parameter)
        {
            PreExecution?.Invoke(this, new DataEventArgs<object>(parameter));
            execute(ConversionHelper.ConvertValue<T>(parameter));
            PostExecution?.Invoke(this, new DataEventArgs<object>(parameter));
        }

        #endregion

        #region Static Method

        /// <summary>
        /// Create <see cref="RelayCommand{T}"/>.
        /// </summary>
        /// <param name="command"><see cref="RelayCommand{T}"/> to reference created <see cref="RelayCommand{T}"/> to.</param>
        /// <param name="execute"><see cref="Action{T}"/> to execute by <paramref name="command"/>.</param>
        /// <returns>if <paramref name="command"/> is not null, returns <paramref name="command"/>; created <see cref="RelayCommand{T}"/> otherwise.</returns>
        /// <exception cref="ArgumentException">execute</exception>
        public static RelayCommand<T> CreateCommand(ref RelayCommand<T> command, Action<T> execute)
        {
            return CreateCommand(ref command, execute, null);
        }

        /// <summary>
        /// Create <see cref="RelayCommand{T}"/>.
        /// </summary>
        /// <param name="command"><see cref="RelayCommand{T}"/> to reference created <see cref="RelayCommand{T}"/> to.</param>
        /// <param name="execute"><see cref="Action{T}"/> to execute by <paramref name="command"/>.</param>
        /// <param name="canExecute"><see cref="RelayCommand{T}"/>'s can execute <see cref="Predicate{T}"/>.</param>
        /// <returns>if <paramref name="command"/> is not null, returns <paramref name="command"/>; created <see cref="RelayCommand{T}"/> otherwise.</returns>
        /// <exception cref="ArgumentException">execute</exception>
        public static RelayCommand<T> CreateCommand(ref RelayCommand<T> command, Action<T> execute, Predicate<T> canExecute)
        {
            if (command != null)
            {
                return command;
            }

            command = canExecute != null
                ? new RelayCommand<T>(execute, canExecute)
                : new RelayCommand<T>(execute);

            return command;
        }

        #endregion
    }
}
