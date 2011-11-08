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
        private ViewModelCommand _startCommand;
        private ViewModelCommand _pauseCommand;
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
        }

        public virtual TimeSpan TimeRemaining {
            get {
                return _model.TimeRemaining;
            }
        }

        public ViewModelCommand StartCommand {
            get {
                if (_startCommand == null) {
                    _startCommand = new ViewModelCommand(
                        StartTimer,
                        () => _model.State != StateType.Running
                        );
                    ViewModelHelper.BindNotifyChanged(
                        _model, this,
                        (sender, e) => {
                        if (e.PropertyName == "State") {
                            _startCommand.RaiseCanExecuteChanged();
                        }
                    });
                }
                return _startCommand;
            }
        }

        public ViewModelCommand PauseCommand {
            get {
                if (_pauseCommand == null) {
                    _pauseCommand = new ViewModelCommand(
                        PauseTimer,
                        () => _model.State == StateType.Running
                        );
                    ViewModelHelper.BindNotifyChanged(
                        _model, this,
                        (sender, e) => {
                        if (e.PropertyName == "State") {
                            _pauseCommand.RaiseCanExecuteChanged();
                        }
                    });
                }
                return _pauseCommand;
            }
        }

        private void StartTimer() {
            _model.Start();
        }

        private void PauseTimer() {
            _model.Pause();
        }
    }
}
