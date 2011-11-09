using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reactive.Linq;

using Livet;

namespace PresentationTimer.Models
{
    public enum StateType
    {
        Neutral,
        Running,
        Pause,
        Stopped,
    }

    class CountDownTimer : NotificationObject
    {
        TimeSpan _timeRemaining;
        TimeSpan _timeSetting;
        IDisposable _timer;
        StateType _state;

        public CountDownTimer() {
            _timeSetting = new TimeSpan(0, 5, 0);
            Reset();
        }

        public StateType State {
            get { return _state; }
            protected set {
                if (_state == value) {
                    return;
                }
                _state = value;
                base.RaisePropertyChanged("State");
            }
        }

        public TimeSpan TimeRemaining {
            get { return _timeRemaining; }
            protected set {
                if (_timeRemaining == value) {
                    return;
                }
                _timeRemaining = value;
                if (_timeRemaining < TimeSpan.Zero) {
                    _timeRemaining = TimeSpan.Zero;
                }
                base.RaisePropertyChanged("TimeRemaining");
            }
        }

        public void Start() {
            if (State == StateType.Running) {
                return;
            }
            _timer = Observable
                .Timer(TimeSpan.FromMilliseconds(100))
                .Repeat()
                .TimeInterval()
                .Subscribe(
                    time => {
                        TimeRemaining -= time.Interval;
                        if (TimeRemaining == TimeSpan.Zero) {
                            Pause();
                        }
                    },
                    ex => {
                        Pause();
                    },
                    () => {
                        Stop();
                    });
            State = StateType.Running;
        }

        public void Stop() {
            StopTimer();
            State = StateType.Stopped;
        }

        public void Pause() {
            StopTimer();
            State = StateType.Pause;
        }

        public void Reset() {
            StopTimer();
            TimeRemaining = _timeSetting.Duration();
            State = StateType.Neutral;
        }

        private void StopTimer() {
            if (_timer == null) {
                return;
            }
            _timer.Dispose();
            _timer = null;
        }
    }
}
