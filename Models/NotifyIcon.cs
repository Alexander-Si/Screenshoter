using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Screenshoter.Models
{
	public class NotifyIcon : IDisposable
	{
		[DllImport("shell32.dll")]
		private static extern bool Shell_NotifyIcon(uint dwMessage, [In] ref NotifyIconData lpData);
		private NotifyIconData _Data;
		private HwndSource _Src;
		private static int _ID = 1;

		public static bool IsChangedClick;
		public event Action LeftClick, RightClick;

		public NotifyIcon(Window parent, System.Drawing.Icon ico)
		{
			_Data = new()
			{
				cbSize = Marshal.SizeOf(typeof(NotifyIconData)),
				uID = _ID++,
				uFlags = 0x8 | 0x2 | 0x10 | 0x1 | 0x4,
				dwState = 0x0,
				hIcon = ico.Handle,
				hWnd = new WindowInteropHelper(parent).Handle,
				uCallbackMessage = 0x5700,
				szTip = "Screenshoter created by Si\nRight click to show/hide menu.\nLeft Click to create Screenshot.",
			};
			_Src = HwndSource.FromHwnd(_Data.hWnd);
			_Src.AddHook(new HwndSourceHook(WndProc));
			Shell_NotifyIcon(0x0, ref _Data);
		}

		public void Hide()
		{
			_Data.dwState = 0x1;
			_Data.dwStateMask = 0x1;
			Shell_NotifyIcon(0x1, ref _Data);
		}

		public void Show()
		{
			_Data.dwState = 0x0;
			_Data.dwStateMask = 0x1;
			Shell_NotifyIcon(0x1, ref _Data);
		}

		private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
		{
			if (msg == 0x5700 && (int)wParam == _Data.uID && IsChangedClick)
			{
				switch ((int)lParam)
				{
					//case 0x201: // WM_LBUTTONDOWN
					//	break;
					case 0x202: // WM_LBUTTONUP
						LeftClick?.Invoke();
						break;
					//case 0x203: // WM_LBUTTONDBLCLK
					//	DoubleClick?.Invoke();
						//break;
					//case 0x204: // WM_RBUTTONDOWN
					//	break;
					case 0x205: // WM_RBUTTONUP
						RightClick?.Invoke();
						break;
					//case 0x200: // WM_MOUSEMOVE
					//	break;
					//case 0x402: // NIN_BALLOONSHOW
					//	break;
					//case 0x403: // NIN_BALLOONHIDE
					//	break;
					//case 0x404: // NIN_BALLOONTIMEOUT
					//	break;
					//case 0x405: // NIN_BALLOONUSERCLICK
					//	break;
				}
			}
			return IntPtr.Zero;
		}

		public void Dispose()
		{
			Shell_NotifyIcon(0x2, ref _Data);
			_Src.RemoveHook(new HwndSourceHook(WndProc));
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct NotifyIconData
	{
		public int cbSize;
		public IntPtr hWnd;
		public int uID;
		public uint uFlags;
		public int uCallbackMessage;
		public IntPtr hIcon;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string szTip;
		public int dwState;
		public int dwStateMask;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string szInfo;
		public int uTimeoutOrVersion;
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string szInfoTitle;
		public int dwInfoFlags;
	}
}
