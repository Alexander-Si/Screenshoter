using System;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace Screenshoter.Models
{
	public class Screenshot
	{
		public Screenshot(Bitmap bmp)
		{
			if (bmp is null)
				throw new NullReferenceException();
			ScreenshotImage = BitmapConverter.ConvertToBitmapSourse(bmp);
		}

		/// <summary> Передоваемый сюда скриншот. </summary>
		public Bitmap ScreenshotBitmap { get; set; }
		/// <summary> Созданое представление скриншота. </summary>
		public BitmapSource ScreenshotImage { get; set; }

		~Screenshot()
		{
			ScreenshotBitmap?.Dispose();
			ScreenshotBitmap = null;
			ScreenshotImage = null;
			GC.Collect();
		}
	}
}
