using System;
using System.Drawing;
using System.Windows.Forms;

namespace AntInvasion {
	class MainForm : Form {
		public MainForm() {
			// Size and position
			Size = Screen.PrimaryScreen.WorkingArea.Size;
			StartPosition = FormStartPosition.Manual;
			Location = new Point(0, 0);

			// Sets window topmost, rendering with highest z-order.
			TopMost = true;

			// Disables window frame and other properties making window
			// visible.
			ShowIcon = false;
			ShowInTaskbar = false;
			FormBorderStyle = FormBorderStyle.None;

			// Make form transparent ------------------------------------------
			// Set TransparencyKey equal to background color, keying out
			// everything, resulting in fully transparancy.
			AllowTransparency = true;
			BackColor = Color.DimGray;
			TransparencyKey = BackColor;

			// Extended window styles -----------------------------------------
			// Enables window click passthrough.
			NativeMethods.EnableWindowPassthrough(Handle);

			// Make window a toolbar. Results in hiding window from ALT+TAB
			// Menu.
			NativeMethods.EnableToolbarStyle(Handle);
		}
	}

	class BufferedPanel : Panel {
		public BufferedPanel() {
			DoubleBuffered = true;

			Dock = DockStyle.Fill;
			BackColor = Color.Transparent;
			BringToFront();
		}
	}
}