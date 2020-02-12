using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using System.IO;
using System.Reflection;

namespace AntInvasion {
    static class NativeMethods {
        
		// --------------------------------------------------------------------
		//   USER32 Dll imports
		// --------------------------------------------------------------------
        [DllImport("user32.dll", SetLastError = true)]
		public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll")]
		public static extern int SetWindowLong(IntPtr hWnd, int nIndex, 
			int dwNewLong);

		[DllImport("user32.dll")]
		public static extern bool SetLayeredWindowAttributes(IntPtr hwnd, 
			uint crKey, byte bAlpha, uint dwFlags);

		[DllImport("user32.dll")]
		public static extern int PeekMessage(out NativeMessage message, 
			IntPtr window, uint filterMin, uint filterMax, uint remove);

		[DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(int vKey);

		// --------------------------------------------------------------------
		//   Dll import structures
		// --------------------------------------------------------------------
        public struct NativeMessage {
			public IntPtr Handle;
			public uint Message;
			public IntPtr WParameter;
			public IntPtr LParameter;

			public uint Time;
			public System.Drawing.Point Location;
		}

		// --------------------------------------------------------------------
		//   Wrapper Methods
		// --------------------------------------------------------------------
		public static bool IsApplicationIdle() {
			NativeMessage nativeMessage;
			return PeekMessage(out nativeMessage, IntPtr.Zero,0U, 0U, 0U) == 0;
		}

        public static string GetPathToFileInAssembly(string relativePath) {
			string assemblyLocation = Assembly.GetExecutingAssembly().Location;
			return Path.Combine(Path.GetDirectoryName(assemblyLocation),
				relativePath);
		}


		// Extended styles ----------------------------------------------------

		const int GWL_EXSTYLE = -20;

		public static void EnableWindowPassthrough(IntPtr windowHandle) {
			// Setting extended styles. Layered and Transparent styles needed
			// for click-through.
			const int WS_EX_LAYERED = 0x80000;
			const int WS_EX_TRANSPARENT = 0x20;
			var style = GetWindowLong(windowHandle, GWL_EXSTYLE);

			SetWindowLong(windowHandle, GWL_EXSTYLE, style | WS_EX_LAYERED
				| WS_EX_TRANSPARENT);
		}

		public static void EnableToolbarStyle(IntPtr windowHandle) {
			// Setting extended style.
			const int WS_EX_TOOLWINDOW = 0x80;
			var style = GetWindowLong(windowHandle, GWL_EXSTYLE);

			SetWindowLong(windowHandle, GWL_EXSTYLE, style | WS_EX_TOOLWINDOW);
		}
    }
}