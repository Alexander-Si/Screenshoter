using System;
using System.Drawing;
using System.Windows;
using System.Windows.Threading;

namespace Screenshoter.Models
{
	public partial class ScreenshotMaker
	{
		/// <summary> Вызывается перед созданием скриншота. </summary>
		public static event Action StartScreenshotting;
		/// <summary> вызывается после окончания создания скриншота, но перед возвратом результата работы. </summary>
		public static event Action EndScreenshotting;
		/// <summary> Поток интерфейса. </summary>
		public static Dispatcher UIThread;
		/// <summary> Не вызывать эвенты StartScreenshotting, EndScreenshotting - в этом случае UIThread может быть null. </summary>
		public static bool NoUpdateUI;
		/// <summary> Сохранить всю область экрана. </summary>
		/// <returns> Вся облась экрана в виде Bitmap. </returns>
		public static Bitmap MakeFullScreen()
		{
			if (!NoUpdateUI)
				UIThread?.Invoke(() => StartScreenshotting?.Invoke());
			(int wight, int height) size = ((int)SystemParameters.VirtualScreenWidth, (int)SystemParameters.VirtualScreenHeight);
			Bitmap bmp = new Bitmap(size.wight, size.height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			Graphics graphics = Graphics.FromImage(bmp);
			graphics.CopyFromScreen(0, 0, 0, 0, bmp.Size);
			if (!NoUpdateUI)
				UIThread?.Invoke(() => EndScreenshotting?.Invoke());
			return bmp;
		}
		/// <summary> Сохранить область экрана. </summary>
		/// <param name="left"> Отступ от левого края экрана. </param>
		/// <param name="top"> Отступ от правого края экрана. </param>
		/// <param name="wight"> Ширина сохраняемой области. </param>
		/// <param name="height"> Высота сохраняемой области. </param>
		/// <returns> Облась экрана в виде Bitmap. </returns>
		public static Bitmap MakeAreaScreen(int left, int top, int wight, int height)
		{
			if (!NoUpdateUI)
				UIThread?.Invoke(() => StartScreenshotting?.Invoke());
			Bitmap bmp = new Bitmap(wight, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			Graphics graphics = Graphics.FromImage(bmp);
			graphics.CopyFromScreen(left, top, 0, 0, bmp.Size);
			if (!NoUpdateUI)
				UIThread?.Invoke(() => EndScreenshotting?.Invoke());
			return bmp;
		}
	}
}
