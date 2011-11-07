using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reactive.Linq;

using Livet;

namespace PresentationTimer.Models
{
    class CountDownTimer : NotificationObject
    {
        TimeSpan _timeRemaining;
        IDisposable _timer;

        public CountDownTimer() {
            _timeRemaining = new TimeSpan(0, 5, 0);
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
            _timer = Observable
                .Timer(TimeSpan.FromMilliseconds(100))
                .Repeat()
                .TimeInterval()
                .Subscribe(time => {
                    TimeRemaining -= time.Interval;
                    if (_timeRemaining == TimeSpan.Zero) {
                        Stop();
                    }
                },
                ex => {
                    Stop();
                },
                () => { });
        }

        public void Stop() {
            if (_timer == null) {
                return;
            }
            _timer.Dispose();
            _timer = null;
        }
    }
}
