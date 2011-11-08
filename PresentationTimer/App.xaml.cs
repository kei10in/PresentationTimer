using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

using Livet;

using PresentationTimer.ViewModels;

namespace PresentationTimer
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        static App() {
        }

        private void AppStartup(object sender, StartupEventArgs e) {
            DispatcherHelper.UIDispatcher = Dispatcher;
        }
    }
}
