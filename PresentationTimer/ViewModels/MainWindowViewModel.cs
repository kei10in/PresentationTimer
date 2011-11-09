using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

using Livet;
using Livet.Commands;

using PresentationTimer.Models;

namespace PresentationTimer.ViewModels
{
    class MainWindowViewModel : ViewModel
    {
        private ViewModelCommand _startStopCommand;
        private ViewModelCommand _resetCommand;
        private CountDownTimer _model;

        public MainWindowViewModel() {
            _model = new CountDownTimer();
            ViewModelHelper.BindNotifyChanged(
                _model, this,
                (sender, e) => {
                    if (e.PropertyName == "TimeRemaining") {
                        RaisePropertyChanged(e.PropertyName);
                    }
                });
            ResetTimer();
        }

        public virtual TimeSpan TimeRemaining {
            get {
                return _model.TimeRemaining;
            }
        }

        private string _startStop;
        public string StartStop {
            get { return _startStop; }
            protected set {
                _startStop = value;
                RaisePropertyChanged("StartStop");
            }
        }

        public ViewModelCommand StartStopCommand {
            get {
                if (_startStopCommand == null) {
                    _startStopCommand = new ViewModelCommand(Operate);
                }
                return _startStopCommand;
            }
        }

        public ViewModelCommand ResetCommand {
            get {
                if (_resetCommand == null) {
                    _resetCommand = new ViewModelCommand(
                        ResetTimer,
                        () => _model.State == StateType.Pause
                        );
                    ViewModelHelper.BindNotifyChanged(
                        _model, this,
                        (sender, e) => {
                        if (e.PropertyName == "State") {
                            _resetCommand.RaiseCanExecuteChanged();
                        }
                    });
                }
                return _resetCommand;
            }
        }

        private void Operate() {
            if (_model.State == StateType.Running) {
                _model.Pause();
                StartStop = "Start";
            } else {
                _model.Start();
                StartStop = "Stop";
            }
        }

        private void ResetTimer() {
            _model.Reset();
            StartStop = "Start";
        }
    }
}
