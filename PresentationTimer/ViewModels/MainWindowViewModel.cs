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
                        OnTimerStateChanged();
                    }
                });
            OnTimerStateChanged();
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
                    _PauseCommand = new ViewModelCommand(Pause);
                }
                return _PauseCommand;
            }
        }

        public void Pause() {
            _model.Pause();
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

        #region Property for Start Button Visibility
        private Visibility _StartButtonVisibility;

        public Visibility StartButtonVisibility {
            get { return _StartButtonVisibility; }
            set {
                if (_StartButtonVisibility.Equals(value)) { return; }
                _StartButtonVisibility = value;
                RaisePropertyChanged("StartButtonVisibility");
            }
        }
        #endregion

        #region Property for Pause and Cancel Button Visibility
        private Visibility _PauseCancelButtonVisibility;

        public Visibility PauseCancelButtonVisibility {
            get { return _PauseCancelButtonVisibility; }
            set {
                if (_PauseCancelButtonVisibility.Equals(value)) { return; }
                _PauseCancelButtonVisibility = value;
                RaisePropertyChanged("PauseCancelButtonVisibility");
            }
        }
        #endregion

        #region Property for Resume and Cancel Button Visibility
        private Visibility _ResumeCancelButtonVisibility;

        public Visibility ResumeCancelButtonVisibility {
            get { return _ResumeCancelButtonVisibility; }
            set {
                if (_ResumeCancelButtonVisibility.Equals(value)) { return; }
                _ResumeCancelButtonVisibility = value;
                RaisePropertyChanged("ResumeCancelButtonVisibility");
            }
        }
        #endregion

        private void OnTimerStateChanged() {
            switch (_model.State) {
                case StateType.Neutral:
                    StartButtonVisibility = Visibility.Visible;
                    PauseCancelButtonVisibility = Visibility.Hidden;
                    ResumeCancelButtonVisibility = Visibility.Hidden;
                    break;
                case StateType.Running:
                    StartButtonVisibility = Visibility.Hidden;
                    PauseCancelButtonVisibility = Visibility.Visible;
                    ResumeCancelButtonVisibility = Visibility.Hidden;
                    break;
                case StateType.Pause:
                    StartButtonVisibility = Visibility.Hidden;
                    PauseCancelButtonVisibility = Visibility.Hidden;
                    ResumeCancelButtonVisibility = Visibility.Visible;
                    break;
            }
        }
    }
}
