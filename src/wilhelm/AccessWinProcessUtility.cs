using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace wilhelm
{
    class AccessWinProcessUtility
    {
        #region WinAPI
        [DllImport("user32")]
        private static extern bool EnumWindows(WNDENUMPROC lpEnumFunc, IntPtr lParam);

        // EnumWindowsから呼び出されるコールバック関数WNDENUMPROCのデリゲート
        private delegate bool WNDENUMPROC(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("user32")]
        private static extern IntPtr GetCurrentProcess();
        #endregion

        #region プロパティ
        private List<string> _inputList;

        public List<string> InputList
        {
            get
            {
                return _inputList;
            }

            set
            {
                _inputList = value;
            }
        }
        #endregion

        #region コンストラクタ
        public AccessWinProcessUtility()
        {
            InputList = new List<string>();
            EnumWindows(EnumerateWindow, IntPtr.Zero);
        }
        #endregion

        #region プライベートメソッド
        // ウィンドウを列挙するためのコールバックメソッド
        private bool EnumerateWindow(IntPtr hWnd, IntPtr lParam)
        {
            // ウィンドウが可視かどうか調べる
            if (IsWindowVisible(hWnd) )
                // 可視の場合
                PrintCaptionAndProcess(hWnd);

            // ウィンドウの列挙を継続するにはtrueを返す必要がある
            return true;
        }

        // ウィンドウのキャプションとプロセス名を表示する
        private  void PrintCaptionAndProcess(IntPtr hWnd)
        {
            // ウィンドウのキャプションを取得・表示
            StringBuilder caption = new StringBuilder(0x1000);

            GetWindowText(hWnd, caption, caption.Capacity);

            // キャプションが空、もしくはProgram Manager のときは除外する
            if (caption.Length != 0 && caption.ToString() != "Program Manager")
            {
                Console.Write("'{0}' ", caption);
                string inputCandidate = caption.ToString();
                // ウィンドウハンドルからプロセスIDを取得
                int processId;

                GetWindowThreadProcessId(hWnd, out processId);

                // プロセスIDからProcessクラスのインスタンスを取得
                Process p = Process.GetProcessById(processId);

                // プロセス名を表示
                Console.WriteLine("({0})", p.ProcessName);
                inputCandidate += p.ProcessName;

                InputList.Add(inputCandidate);
            }
        }
        #endregion
    }
}