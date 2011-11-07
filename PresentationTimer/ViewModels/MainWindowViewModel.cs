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
        private ICommand _startCommand;
        private ICommand _stopCommand;
        private CountDownTimer _model;

        public MainWindowViewModel() {
            _model = new CountDownTimer();
            ViewModelHelper.BindNotifyChanged(
                _model, this,
                (sender, e) => {
                    RaisePropertyChanged(e.PropertyName);
                });
        }

        public virtual string TimeRemaining {
            get {
                return _model.TimeRemaining.ToString("hh\\:mm\\:ss");
            }
        }

        public ICommand StartCommand {
            get {
                if (_startCommand == null) {
                    _startCommand = new ViewModelCommand(StartTimer);
                }
                return _startCommand;
            }
        }

        public ICommand StopCommand {
            get {
                if (_stopCommand == null) {
                    _stopCommand = new ViewModelCommand(StopTimer);
                }
                return _stopCommand;
            }
        }

        private void StartTimer() {
            _model.Start();
        }

        private void StopTimer() {
            _model.Stop();
        }
    }
}
