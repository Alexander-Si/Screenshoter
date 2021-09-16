using System.Drawing;
using System.Windows;

namespace Screenshoter.Models
{
	public partial class ScreenshotMaker
	{
		/// <summary> Сохранить всю область экрана. </summary>
		/// <returns> Вся облась экрана в виде Bitmap. </returns>
		public static Bitmap MakeFullScreen()
		{
			(int wight, int height) size = ((int)SystemParameters.VirtualScreenWidth, (int)SystemParameters.VirtualScreenHeight);
			Bitmap bmp = new Bitmap(size.wight, size.height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			Graphics graphics = Graphics.FromImage(bmp);
			graphics.CopyFromScreen(0, 0, 0, 0, bmp.Size);
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
			CheckScreen(ref left, ref top, ref wight, ref height);
			Bitmap bmp = new Bitmap(wight, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			Graphics graphics = Graphics.FromImage(bmp);
			graphics.CopyFromScreen(left, top, 0, 0, bmp.Size);
			return bmp;
		}

		public static Bitmap CutTheScreen(Bitmap screen, System.Drawing.Point location, System.Drawing.Size size)
		{
			var rect = new Rectangle(location, size);
			Bitmap bmp = screen.Clone(rect, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			screen.Dispose();
			return bmp;
		}

		public static Bitmap CutTheScreen(Bitmap screen, int left, int top, int wight, int height)
		{
			CheckScreen(ref left, ref top, ref wight, ref height);
			var rect = new Rectangle(left, top, wight, height);
			Bitmap bmp = screen.Clone(rect, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			screen.Dispose();
			return bmp;
		}

		private static void CheckScreen(ref int left, ref int top, ref int wight, ref int height)
		{
			(int wight, int height) size = ((int)SystemParameters.VirtualScreenWidth, (int)SystemParameters.VirtualScreenHeight);
			if (left < 0)
			{
				wight += left;
				left = 0;
			}
			if (top < 0)
			{
				height += top;
				top = 0;
			}
			var x = left + wight;
			if (x > size.wight)
				wight -= x - size.wight;
			var y = top + height;
			if (y > size.height)
				height -= y - size.height;
		}
	}
}
