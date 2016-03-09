using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace wilhelm
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        private NotifyIconWrapper notyfyIcon;

        #region Override

        #region OnStartup
        /// <summary>
        /// OnStartup のオーバーライド
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            this.notyfyIcon = new NotifyIconWrapper();
        }
        #endregion

        #region OnExit
        /// <summary>
        /// OnExit のオーバーライド
        /// </summary>
        /// <param name="e">イベント引数</param>
        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);
            this.notyfyIcon.Dispose();
        }
        #endregion

        #endregion
    }
}
