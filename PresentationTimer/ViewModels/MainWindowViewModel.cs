using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private CountDownTimer _model;

        public MainWindowViewModel() {
            _model = new CountDownTimer();
            ViewModelHelper.BindNotifyChanged(
                _model, this,
                (sender, e) => {
                    if (e.PropertyName == "TimeRemaining") {
                        RaisePropertyChanged("TimeRemaining");
                    }
                });
            ViewModelHelper.BindNotifyChanged(
                _model, this,
                (sender, e) => {
                    if (e.PropertyName == "State") {
                        // TODO: PauseCommand の生成のときに設定できるようにする。
                        PauseCommand.RaiseCanExecuteChanged();
                        RaisePropertyChanged("TimerState");
                    }
                });
        }

        public virtual TimeSpan TimeRemaining {
            get {
                return _model.TimeRemaining;
            }
        }

        #region Command for Start Timer
        /// <summary>
        /// タイマーを開始するためのコマンドです。
        /// </summary>
        private ViewModelCommand _StartCommand;

        public ViewModelCommand StartCommand {
            get {
                if (_StartCommand == null) {
                    _StartCommand = new ViewModelCommand(Start);
                }
                return _StartCommand;
            }
        }

        public void Start() {
            _model.Start();
        }
        #endregion

        #region Command for Pause Timer
        private ViewModelCommand _PauseCommand;

        public ViewModelCommand PauseCommand {
            get {
                if (_PauseCommand == null) {
                    _PauseCommand = new ViewModelCommand(Pause, CanPause);
                }
                return _PauseCommand;
            }
        }

        public void Pause() {
            _model.Pause();
        }

        public bool CanPause() {
            switch (_model.State) {
                case StateType.Running:
                    return true;
                default:
                    return false;
            }
        }
        #endregion

        #region Command for Resume Timer
        private ViewModelCommand _ResumeCommand;

        public ViewModelCommand ResumeCommand {
            get {
                if (_ResumeCommand == null) {
                    _ResumeCommand = new ViewModelCommand(Resume);
                }
                return _ResumeCommand;
            }
        }

        public void Resume() {
            _model.Start();
        }
        #endregion

        #region Command for Cancel Timer
        /// <summary>
        /// タイマーの動作を取り消すためのコマンドです。
        /// </summary>
        private ViewModelCommand _CancelCommand;

        public ViewModelCommand CancelCommand {
            get {
                if (_CancelCommand == null) {
                    _CancelCommand = new ViewModelCommand(Cancel);
                }
                return _CancelCommand;
            }
        }

        public void Cancel() {
            _model.Reset();
        }
        #endregion

        #region Property for Timer State
        public StateType TimerState {
            get { return _model.State; }
        }
        #endregion

        #region Property for Preseted Timer Settings Commands
        public IEnumerable<CommandViewModel> PresetTimerCommands {
            get {
                return from t in TimerPresets
                       select new CommandViewModel(
                           t.ToString(@"mm\:ss"),
                           CreateTimeSetterCommand(t));
            }
        }
        #endregion

        #region Private members for preseted time settings.
        List<TimeSpan> _TimerPresets;
        private List<TimeSpan> TimerPresets {
            get {
                if (_TimerPresets == null) {
                    _TimerPresets = new List<TimeSpan> {
                        new TimeSpan(0, 5, 0),
                        new TimeSpan(0, 10, 0),
                        new TimeSpan(0, 15, 0),
                        new TimeSpan(0, 20, 0),
                        new TimeSpan(0, 25, 0),
                        new TimeSpan(0, 30, 0),
                    };
                }
                return _TimerPresets;
            }
        }

        /// <summary>
        /// Create ViewModelCommand object for setup the timer.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private ICommand CreateTimeSetterCommand(TimeSpan time) {
            return new ViewModelCommand(() => { _model.TimeSetting = time; });
        }
        #endregion

    }
}
