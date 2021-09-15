using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Screenshoter.Models
{
	public class BitmapConverter
	{
		/// <summary>
		/// Конвертирует Bitmap в BitmapSourse
		/// </summary>
		/// <param name="bitmap"> Bitmap (картинка) </param>
		/// <returns></returns>
		public static BitmapSource ConvertToBitmapSourse(Bitmap bitmap)
		{
			if(bitmap is null)
				return null;
			var bitmapData = bitmap.LockBits(
				new Rectangle(0, 0, bitmap.Width, bitmap.Height),
				System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

			var bitmapSource = BitmapSource.Create(
				bitmapData.Width, bitmapData.Height,
				bitmap.HorizontalResolution, bitmap.VerticalResolution,
				PixelFormats.Bgr24, null,
				bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

			bitmap.UnlockBits(bitmapData);
			bitmapSource.Freeze();

			return bitmapSource;
		}
	}
}
