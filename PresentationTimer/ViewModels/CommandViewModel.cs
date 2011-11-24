using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

using Livet;

namespace PresentationTimer.ViewModels
{
    public class CommandViewModel : ViewModel
    {
        public CommandViewModel(string text, ICommand command) {
            DisplayName = text;
            Command = command;
        }

        #region Property DisplayName
        string _DisplayName;
        public string DisplayName {
            get { return _DisplayName; }
            protected set {
                if (_DisplayName == value) {
                    return;
                }
                _DisplayName = value;
                RaisePropertyChanged("DisplayName");
            }
        }
        #endregion

        #region Property Command
        ICommand _Command;
        public ICommand Command {
            get { return _Command; }
            protected set {
                if (_Command == value) {
                    return;
                }
                _Command = value;
                RaisePropertyChanged("Command");
            }
        }
        #endregion
    }
}
