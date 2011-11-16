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
        TimeSpan _TimeSetting;
        IDisposable _Timer;
        int _TimerResolusion = 100;

        public CountDownTimer() {
            _TimeSetting = new TimeSpan(0, 5, 0);
            Reset();
        }

        StateType _State;
        public StateType State {
            get { return _State; }
            protected set {
                if (_State == value) {
                    return;
                }
                _State = value;
                base.RaisePropertyChanged("State");
            }
        }

        TimeSpan _TimeRemaining;
        public TimeSpan TimeRemaining {
            get { return _TimeRemaining; }
            protected set {
                if (_TimeRemaining == value) {
                    return;
                }
                _TimeRemaining = value;
                if (_TimeRemaining < TimeSpan.Zero) {
                    _TimeRemaining = TimeSpan.Zero;
                }
                base.RaisePropertyChanged("TimeRemaining");
            }
        }

        public void Start() {
            StartTimer();
            State = StateType.Running;
        }

        public void Stop() {
            Reset();
        }

        public void Pause() {
            StopTimer();
            State = StateType.Pause;
        }

        public void Reset() {
            StopTimer();
            TimeRemaining = _TimeSetting.Duration();
            State = StateType.Neutral;
        }

        private void StartTimer() {
            if (_Timer != null) { return; }
            _Timer = Observable
                .Timer(TimeSpan.FromMilliseconds(_TimerResolusion))
                .Repeat()
                .TimeInterval()
                .Subscribe(
                time => {
                    TimeRemaining -= time.Interval;
                    if (TimeRemaining <= TimeSpan.Zero) {
                        Stop();
                    }
                },
                ex => { Pause(); },
                () => { }
                );
        }

        private void StopTimer() {
            if (_Timer == null) { return; }
            _Timer.Dispose();
            _Timer = null;
        }
    }
}
