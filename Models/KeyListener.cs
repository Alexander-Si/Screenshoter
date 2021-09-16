using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Screenshoter.Models
{
	public delegate void ReturnKey(in KeyEventHandle key);
	public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
	public static class KeyListener
	{
		public static event ReturnKey OnPressedKey;

		private const int WH_KEYBOARD_LL = 13;
		private static LowLevelKeyboardProc _Proc = HookCallback;
		private static IntPtr Description;

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		private static extern bool UnhookWindowsHookEx(IntPtr hhk);

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		public static IntPtr SetHook()
		{
			using (Process curProcess = Process.GetCurrentProcess())
			using (ProcessModule curModule = curProcess.MainModule)
				return Description = SetWindowsHookEx(WH_KEYBOARD_LL, _Proc, GetModuleHandle(curModule.ModuleName), 0);
		}

		public static bool ClearHook()
		{
			using (Process curProcess = Process.GetCurrentProcess())
			using (ProcessModule curModule = curProcess.MainModule)
				return UnhookWindowsHookEx(Description);
		}

		private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
		{
			int vkCode = Marshal.ReadInt32(lParam);
			var key = new KeyEventHandle(vkCode, (KeyPressedType)wParam);
			OnPressedKey?.Invoke(in key);
			return (IntPtr)0;
		}
	}
	public enum KeyPressedType
	{
		WM_KEYDOWN = 0x100,
		WM_KEYUP = 0x101,
		WM_SYS_KEYDOWN = 0x104,
		WM_SYS_KEYUP = 0x105,
	}
	public record KeyEventHandle(int Key, KeyPressedType PressedType);
}
