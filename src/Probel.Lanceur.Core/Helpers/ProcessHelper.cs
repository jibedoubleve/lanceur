using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Probel.Lanceur.Core.Helpers
{
    public class ProcessHelper
    {
        #region Methods

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(out Win32Point lpPoint);

        private static Win32Point GetMousePosition()
        {
            GetCursorPos(out var p);
            return Win32Point.NewPoint(p.X, p.Y);
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr WindowFromPoint(Win32Point point);

        public static PsInfo GetExecutablePath()
        {
            var c = GetMousePosition();
            var point = Win32Point.NewPoint(c.X, c.Y);
            var hWnd = WindowFromPoint(point);

            var threadId = GetWindowThreadProcessId(hWnd, out uint processId);

            var ps = Process.GetProcessById((int)processId);
            var fileName = ps.MainModule.FileName;

            return new PsInfo()
            {
                HWnd = $"ThreadId: {threadId} - ProcessId: {processId} - hWnd: {hWnd}",
                FileName = fileName.ToString()
            };
        }

        #endregion Methods

        #region Structs

        [StructLayout(LayoutKind.Sequential)]
        private struct Win32Point
        {
            public static Win32Point NewPoint(int x, int y)
            {
                return new Win32Point
                {
                    X = x,
                    Y = y,
                };
            }

            public int X;
            public int Y;
        };

        #endregion Structs
    }
}