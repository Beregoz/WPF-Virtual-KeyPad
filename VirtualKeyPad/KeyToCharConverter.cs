using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows.Data;
using System.Windows.Input;

// ReSharper disable InconsistentNaming

namespace VirtualKeyPad
{
	/*
	 * http://www.pinvoke.net/default.aspx/user32.tounicode
	 */
	internal class KeyToCharConverter : IValueConverter
	{
		[DllImport("user32.dll")]
		static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState, [Out, MarshalAs(UnmanagedType.LPWStr)] System.Text.StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhkl);

		[DllImport("user32.dll")]
		static extern bool GetKeyboardState(byte[] lpKeyState);

		[DllImport("user32.dll")]
		static extern uint MapVirtualKey(uint uCode, uint uMapType);

		[DllImport("user32.dll")]
		static extern IntPtr GetKeyboardLayout(uint idThread);

		public static string VKCodeToUnicode(uint VKCode)
		{
			System.Text.StringBuilder sbString = new System.Text.StringBuilder();

			byte[] bKeyState = new byte[255];
			bool bKeyStateStatus = GetKeyboardState(bKeyState);
			if (!bKeyStateStatus)
			{
				return "";
			}
			uint lScanCode = MapVirtualKey(VKCode, 0);
			IntPtr HKL = GetKeyboardLayout(0);

			ToUnicodeEx(VKCode, lScanCode, bKeyState, sbString, (int)5, (uint)0, HKL);
			return sbString.ToString();
		}
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var key = (Key)value;
			var keyCode = (uint)KeyInterop.VirtualKeyFromKey(key);
			var x = VKCodeToUnicode(keyCode);
			return x;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}